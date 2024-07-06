using CsvHelper;
using EnsekCodingExercise.ApiService.Infrastructure.Contexts;
using EnsekCodingExercise.ApiService.Models.Database;
using EnsekCodingExercise.ApiService.Models.External;
using EnsekCodingExercise.ApiService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;

namespace EnsekCodingExercise.ApiService.Services
{
    /// <summary>
    /// Collection of Readings Services
    /// </summary>
    public class ReadingsService : IReadingsService
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
        public async Task<Dictionary<string, string>> CreateReading(CreateReadingModel createReadingModel)
        {
            var result = new Dictionary<string, string>();
            // Check that we have a valid account
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var account = await context.Accounts.FindAsync(createReadingModel.AccountId);
            if (account == null)
            {
                result.Add("error", $"Account with ID {createReadingModel.AccountId} not found");
            }
            else if (await context.Readings.AnyAsync(x => x.AccountId == createReadingModel.AccountId && x.ReadingDateTime > createReadingModel.ReadingDateTime))
            {
                result.Add("error", "Reading date is not the latest reading for this account");
            }
            else if (await context.Readings.AnyAsync(x => x.AccountId == createReadingModel.AccountId
                                                          && x.ReadingDateTime == createReadingModel.ReadingDateTime
                                                          && x.MeterReadValue == createReadingModel.MeterReadValue))
            {
                result.Add("error", "Reading already exists");
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
                result.Add("success", reading.ReadingId.ToString());
            }
            
            return result;
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
        /// Import readings from an uploaded file.
        /// </summary>
        /// <param name="formFile">The uploaded file containing the readings to be imported</param>
        /// <returns></returns>
        public async Task<UploadResults> UploadReadingsFromFile(IFormFile formFile)
        {
            // This will blow up on rubbish or poorly formatted date strings but I've not been told to validate date so assuming it's correctly formatted.
            var readings = new List<Reading>();
            var successfulReadings = 0;
            var failedReadings = 0;

            using var context = await _dbContextFactory.CreateDbContextAsync();
            using var reader = new StreamReader(formFile.OpenReadStream());
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var anonymousTypeDefinition = new
            {
                AccountId = -1,
                MeterReadingDateTime = string.Empty,
                MeterReadValue = string.Empty
            };

            var inputLines = csv.GetRecords(anonymousTypeDefinition).ToList();
            var totalReadings = inputLines.Count;
            foreach (var line in inputLines)
            {
                var reading = new Reading
                {
                    AccountId = line.AccountId,
                    ReadingDateTime = DateTime.Parse(line.MeterReadingDateTime),
                    MeterReadValue = line.MeterReadValue
                };

                var isNew = !await context.Readings.AnyAsync(x => x.AccountId == reading.AccountId
                                                                  && x.ReadingDateTime == reading.ReadingDateTime
                                                                  && x.MeterReadValue == reading.MeterReadValue);
                var accountExists = await context.Accounts.AnyAsync(x => x.AccountId == reading.AccountId);
                var isLatestReading = !await context.Readings.AnyAsync(x => x.AccountId == reading.AccountId && x.ReadingDateTime > reading.ReadingDateTime);
                var meterReadValueIsValid = Regex.IsMatch(reading.MeterReadValue.ToString(), @"^[0-9]{5}$", RegexOptions.None, TimeSpan.FromMilliseconds(100));

                if (isNew && accountExists && isLatestReading && meterReadValueIsValid)
                {
                    await context.Readings.AddAsync(reading); // Add to context so we can save it later but also so we can validate
                    await context.SaveChangesAsync();
                    successfulReadings++;
                }
                else
                {
                    failedReadings++;
                }
            }

            var uploadResults = new UploadResults
            {
                Successful = successfulReadings,
                Failed = failedReadings,
                TotalRecords = totalReadings
            };

            return uploadResults;
        }
    }
}
