using EnsekCodingExercise.ApiService.Controllers;
using EnsekCodingExercise.ApiService.Models.External;
using EnsekCodingExercise.ApiService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EnsekCodingExercise.Tests
{
    public class ReadingsControllerTests
    {
        private readonly Mock<IReadingsService> _mockReadingsService;
        private readonly ReadingsController _controller;

        public ReadingsControllerTests()
        {
            _mockReadingsService = new Mock<IReadingsService>();
            _controller = new ReadingsController(_mockReadingsService.Object);
        }

        [Fact]
        public async Task GetAllReadings_Returns200OK_WithReadings()
        {
            // Arrange
            var mockReadings = new List<ReadingDto> { new ReadingDto(), new ReadingDto() };
            _mockReadingsService.Setup(service => service.GetAllReadings()).ReturnsAsync(mockReadings);

            // Act
            var result = await _controller.GetAllReadings();

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<ReadingDto>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(mockReadings, okResult.Value);
        }

        [Fact]
        public async Task GetReadingById_Returns200OK_WithReading()
        {
            // Arrange
            var mockReading = new ReadingDto { ReadingId = 1 };
            _mockReadingsService.Setup(service => service.GetReadingById(It.IsAny<int>())).ReturnsAsync(mockReading);

            // Act
            var result = await _controller.GetReadingById(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ReadingDto>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(mockReading, okResult.Value);
        }

        [Fact]
        public async Task CreateReading_Returns201Created()
        {
            // Arrange
            var createReadingModel = new CreateReadingModel();
            _mockReadingsService.Setup(service => service.CreateReading(It.IsAny<CreateReadingModel>())).ReturnsAsync(1);

            // Act
            var result = await _controller.CreateReading(createReadingModel);

            // Assert
            var actionResult = Assert.IsType<ActionResult<int>>(result);
            var createdAtActionResult = Assert.IsType<CreatedResult>(actionResult.Result);
            Assert.Equal(201, createdAtActionResult.StatusCode);
        }

        [Fact]
        public async Task EditReading_Returns204NoContent()
        {
            // Arrange
            var editReadingModel = new EditReadingModel { ReadingId = 1 };
            _mockReadingsService.Setup(service => service.EditReading(It.IsAny<EditReadingModel>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.EditReading(1, editReadingModel);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task DeleteReadingById_Returns204NoContent()
        {
            // Arrange
            _mockReadingsService.Setup(service => service.DeleteReadingById(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteReadingById(1);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task MeterReadingUpload_Returns200OK()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var content = "AccountId,MeterReadingDateTime,MeterReadValue\n2344,22/04/2020 09:24,01002";
            var fileName = "readings.csv";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            var uploadResults = new UploadResults { Successful = 1, Failed = 0 };
            _mockReadingsService.Setup(service => service.UploadReadingsFromFile(It.IsAny<IFormFile>())).ReturnsAsync(uploadResults);

            // Act
            var result = await _controller.MeterReadingUpload(fileMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(uploadResults, okResult.Value);
        }
    }
}
