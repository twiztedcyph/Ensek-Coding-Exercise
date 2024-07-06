using Asp.Versioning;
using EnsekCodingExercise.ApiService.Infrastructure.BaseControllers;
using EnsekCodingExercise.ApiService.Models.External;
using EnsekCodingExercise.ApiService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EnsekCodingExercise.ApiService.Controllers
{
    /// <summary>
    /// Controller for managing meter readings
    /// </summary>
    public class ReadingsController : BaseController
    {
        private readonly IReadingsService _readingsService;

        /// <summary>
        /// Readings Controller Constructor
        /// </summary>
        /// <param name="readingsService">The Readings Service</param>
        public ReadingsController(IReadingsService readingsService)
        {
            _readingsService = readingsService;
        }

        /// <summary>
        /// Get a list of all meter readings
        /// </summary>
        /// <returns>A list of meter readings</returns>
        [ApiVersion("1")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ReadingDto>>> GetAllReadings()
        {
            var readings = await _readingsService.GetAllReadings();
            return Ok(readings);
        }

        /// <summary>
        /// Get a meter reading by its ID
        /// </summary>
        /// <param name="id">The meter reading ID</param>
        /// <returns>A meter reading matching the given ID</returns>
        [ApiVersion("1")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReadingDto>> GetReadingById(int? id)
        {
            if (id == null)
            {
                return BadRequest("A meter reading ID is required");
            }
            else
            {
                var reading = await _readingsService.GetReadingById(id.Value);
                if (reading == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(reading);
                }
            }
        }

        /// <summary>
        /// Create a new meter reading
        /// </summary>
        /// <param name="createReadingModel">The meter reading to be created</param>
        /// <returns>The ID of the created meter reading</returns>
        [ApiVersion("1")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Dictionary<string, string>>> CreateReading([FromBody] CreateReadingModel createReadingModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _readingsService.CreateReading(createReadingModel);
                if (result.ContainsKey("error"))
                {
                    return BadRequest(result);
                }
                else
                {
                    return Created(string.Empty, result);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Edit a meter reading
        /// </summary>
        /// <param name="id">The ID of the meter reading to be edited</param>
        /// <param name="editReadingModel">The meter reading to be edited</param>
        /// <returns>A no content status if the meter reading was edited</returns>
        [ApiVersion("1")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> EditReading(int? id, [FromBody] EditReadingModel editReadingModel)
        {
            if (ModelState.IsValid)
            {
                if (id.HasValue && id == editReadingModel.ReadingId)
                {
                    await _readingsService.EditReading(editReadingModel);
                    return NoContent();
                }
                else
                {
                    ModelState.AddModelError(nameof(id), "The meter reading IDs do not match");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Delete a meter reading by its ID
        /// </summary>
        /// <param name="id">The meter reading ID</param>
        /// <returns>No content if the meter reading is deleted or not found if there is no meter reading with that ID</returns>
        [ApiVersion("1")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteReadingById(int? id)
        {
            if (id == null)
            {
                return BadRequest("A meter reading ID is required");
            }
            else
            {
                await _readingsService.DeleteReadingById(id.Value);
                return NoContent();
            }
        }


        /// <summary>
        /// Upload meter readings
        /// </summary>
        /// <param name="formFile">The meter readings file</param>
        /// <returns>The result of the upload showing the number of successful and failed readings</returns>
        [ApiVersion("1")]
        [HttpPost("meter-reading-uploads")]
        [Consumes("multipart/form-data")] // Special allowance here because we are expecting a file upload
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> MeterReadingUpload(IFormFile formFile)
        {
            var allowedFileExtensions = new List<string> { ".csv" }; // There could be more but for now we are only allowing .CSV files
            if (formFile == null)
            {
                return BadRequest("A file is required");
            }
            else if (formFile.Length == 0)
            {
                return BadRequest("The file provided is empty");
            }
            else if (!allowedFileExtensions.Contains(Path.GetExtension(formFile.FileName).ToLower()))
            {
                return BadRequest("The file must be a CSV");
            }
            else
            {
                UploadResults uploadResults = await _readingsService.UploadReadingsFromFile(formFile);
                return Ok(uploadResults);
            }
        }
    }
}
