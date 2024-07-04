using CsvHelper;
using CsvHelper.Configuration;
using EnsekCodingExercise.ApiService.Infrastructure.Contexts;
using EnsekCodingExercise.ApiService.Models.Database;
using EnsekCodingExercise.ApiService.Models.External;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;

namespace EnsekCodingExercise.ApiService.Services
{
    /// <summary>
    /// Collection of Readings Services
    /// </summary>
    public class ReadingsService
    {
        private readonly IDbContextFactory<CustomerContext> _dbContextFactory;

        /// <summary>
        /// Readings Service Constructor
        /// </summary>
        /// <param name="dbContextFactory"></param>
        public ReadingsService(IDbContextFactory<CustomerContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        /// <summary>
        /// Get a list of all Readings
        /// </summary>
        /// <returns>A list of all Readings</returns>
        public async Task<List<ReadingDto>> GetAllReadings()
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            return await context.Readings.Select(x => new ReadingDto
            {
                ReadingId = x.ReadingId,
                AccountId = x.AccountId,
                ReadingDateTime = x.ReadingDateTime,
                MeterReadValue = x.MeterReadValue
            }).ToListAsync();
        }

        /// <summary>
        /// Get Reading by ID
        /// </summary>
        /// <returns>A Reading matching the ID given or null if there is no Reading found</returns>
        public async Task<ReadingDto?> GetReadingById(int id)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var reading = await context.Readings.FindAsync(id);
            if (reading == null)
            {
                return null;
            }
            else
            {
                return new ReadingDto
                {
                    ReadingId = reading.ReadingId,
                    AccountId = reading.AccountId,
                    ReadingDateTime = reading.ReadingDateTime,
                    MeterReadValue = reading.MeterReadValue
                };
            }
        }

        /// <summary>
        /// Create a Reading
        /// </summary>
        /// <param name="createReadingModel">Model containing the details of the reading to be created</param>
        /// <returns>The ID of the created Reading</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the Account that the reading is being added to cannot be found</exception>
        public async Task<int> CreateReading(CreateReadingModel createReadingModel)
        {
            // Check that we have a valid account
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var account = await context.Accounts.FindAsync(createReadingModel.AccountId);
            if (account == null)
            {
                throw new KeyNotFoundException($"Account with ID {createReadingModel.AccountId} not found");
            }
            else
            {
                var reading = new Reading
                {
                    AccountId = createReadingModel.AccountId,
                    ReadingDateTime = createReadingModel.ReadingDateTime,
                    MeterReadValue = createReadingModel.MeterReadValue
                };

                account.Readings.Add(reading);
                await context.SaveChangesAsync();
                return reading.ReadingId;
            }
        }

        /// <summary>
        /// Update a Reading
        /// </summary>
        /// <param name="editReadingModel">Model containing the details of the reading to be updated</param>
        /// <exception cref="KeyNotFoundException">Thrown if the Reading is not found</exception>
        public async Task EditReading(EditReadingModel editReadingModel)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var reading = await context.Readings.FindAsync(editReadingModel.ReadingId);
            if (reading == null)
            {
                throw new KeyNotFoundException($"Reading with ID {editReadingModel.ReadingId} not found");
            }
            else
            {
                if (editReadingModel.ReadingDateTime != reading.ReadingDateTime)
                {
                    reading.ReadingDateTime = editReadingModel.ReadingDateTime;
                }

                if (editReadingModel.MeterReadValue != reading.MeterReadValue)
                {
                    reading.MeterReadValue = editReadingModel.MeterReadValue;
                }

                if (context.ChangeTracker.HasChanges())
                {
                    await context.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Delete a Reading by ID
        /// </summary>
        /// <param name="id">The ID of the Reading to be deleted</param>
        /// <exception cref="KeyNotFoundException">Thrown if the Reading is not found</exception>
        public async Task DeleteReadingById(int id)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var reading = await context.Readings.FindAsync(id);
            if (reading == null)
            {
                throw new KeyNotFoundException($"Reading with ID {id} not found");
            }
            else
            {
                context.Readings.Remove(reading);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public async Task<UploadResults> UploadReadingsFromFile(IFormFile formFile)
        {
            // Probably a bit over the top...
            var successfulReadings = new List<Reading>();
            var failedReadings = new List<Reading>();

            using var context = await _dbContextFactory.CreateDbContextAsync();
            using var reader = new StreamReader(formFile.OpenReadStream());
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var readings = csv.GetRecords<dynamic>().ToList();

            // Ok bed time but I suggest that tomorrow we add a booking object that exactly matches the csv.
            //foreach (var reading in readings)
            //{
            //    var isNew = !await context.Readings.AnyAsync(x => x.ReadingId == reading.ReadingId);
            //    var accountExists = await context.Accounts.AnyAsync(x => x.AccountId == reading.AccountId);
            //    var isLatestReading = !await context.Readings.AnyAsync(x => x.AccountId == reading.AccountId && x.ReadingDateTime > reading.ReadingDateTime);
            //    var meterReadValueIsValid = Regex.IsMatch(reading.MeterReadValue.ToString(), @"^[0-9]{5}$", RegexOptions.None, TimeSpan.FromMilliseconds(100));
            //    if (isNew && accountExists && isLatestReading && meterReadValueIsValid)
            //    {
            //        successfulReadings.Add(reading);
            //    }
            //    else
            //    {
            //       failedReadings.Add(reading);
            //    }
            //}

            await context.Readings.AddRangeAsync(successfulReadings);
            await context.SaveChangesAsync();

            var uploadResults = new UploadResults
            {
                Successful = successfulReadings.Count,
                Failed = failedReadings.Count
            };

            return uploadResults;
        }
    }
}
