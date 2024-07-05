using EnsekCodingExercise.ApiService.Infrastructure.Contexts;
using EnsekCodingExercise.ApiService.Models.External;
using EnsekCodingExercise.ApiService.Services;
using EnsekCodingExercise.ApiService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace EnsekCodingExercise.Tests
{
    public class ReadingServiceTests
    {
        private readonly IReadingsService _readingsService;
        private readonly ServiceProvider _serviceProvider;

        public ReadingServiceTests()
        {
            // Set up a service collection
            var serviceCollection = new ServiceCollection();

            // Add DbContextFactory with a real database connection string
            var connectionString = "Server=(local);Database=EnsekCodingExercise;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
            serviceCollection.AddDbContextFactory<CustomerContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            // Add the ReadingsService
            serviceCollection.AddScoped<IReadingsService, ReadingsService>();

            // Build the service provider
            _serviceProvider = serviceCollection.BuildServiceProvider();

            // Get the ReadingsService instance
            _readingsService = _serviceProvider.GetRequiredService<IReadingsService>();
        }

        [Fact]
        public async Task TestCrud()
        {
            // Doing this as one test to ensure the order of operations with the intention being to leave the database in the same state it started in.
            // We don't have seed data for readings so we will do all our operations in one test.

            // Create
            var createModel = new CreateReadingModel
            {
                AccountId = 1234,
                ReadingDateTime = DateTime.Now,
                MeterReadValue = "12345"
            };

            var readingId = await _readingsService.CreateReading(createModel);
            Assert.True(readingId > 0);

            // Read
            var reading = await _readingsService.GetReadingById(readingId);
            Assert.NotNull(reading);
            Assert.Equal(createModel.AccountId, reading.AccountId);
            Assert.Equal(createModel.ReadingDateTime, reading.ReadingDateTime);
            Assert.Equal(createModel.MeterReadValue, reading.MeterReadValue);

            // Edit
            var editModel = new EditReadingModel
            {
                ReadingId = reading.ReadingId,
                ReadingDateTime = DateTime.Now.AddDays(1),
                MeterReadValue = "54321"
            };
            await _readingsService.EditReading(editModel);

            var readingFromDb = await _readingsService.GetReadingById(reading.ReadingId);
            Assert.NotNull(readingFromDb);
            Assert.Equal(editModel.ReadingDateTime, readingFromDb.ReadingDateTime);
            Assert.Equal(editModel.MeterReadValue, readingFromDb.MeterReadValue);

            // Delete
            await _readingsService.DeleteReadingById(reading.ReadingId);
            var deletedReading = await _readingsService.GetReadingById(reading.ReadingId);
            Assert.Null(deletedReading);
        }

        [Fact]
        public async Task UploadReadingsFromFile_ShouldProcessFileCorrectly()
        {
            // Arrange
            // Again relying on Account seed data here and also assuming that the readings database is empty to begin with.
            var csvContent = "AccountId,MeterReadingDateTime,MeterReadValue\n" +
                "1234,2023-01-01T00:00:00,12345\n" +
                "1239,2023-01-01T00:00:00,12345\n" +
                "1234,2023-01-01T00:00:00,12345\n" + // Fail. Duplicate reading
                "1234,2022-01-01T00:00:00,22345\n" + // Fail. ReadingDateTime is not latest
                "1240,2023-01-01T00:00:00,7\n" + // Fail. Wrong format for MeterReadValue
                "5,2023-01-02T00:00:00,32345"; // Fail. Account does not exist
            var formFile = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(csvContent)), 0, csvContent.Length, "file", "readings.csv");

            // Act
            var result = await _readingsService.UploadReadingsFromFile(formFile);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.TotalRecords);
            Assert.Equal(2, result.Successful);
            Assert.Equal(4, result.Failed);

            // Check the readings were created
            var readings = await _readingsService.GetAllReadings();
            Assert.NotNull(readings);
            Assert.Equal(2, readings.Count);

            // Clean up
            foreach (var reading in readings)
            {
                await _readingsService.DeleteReadingById(reading.ReadingId);
            }

            readings = await _readingsService.GetAllReadings();
            Assert.NotNull(readings);
            Assert.Empty(readings);
        }
    }
}
