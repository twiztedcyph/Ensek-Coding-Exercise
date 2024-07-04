using Asp.Versioning;
using EnsekCodingExercise.ApiService.Infrastructure.BaseControllers;
using EnsekCodingExercise.ApiService.Models.External;
using Microsoft.AspNetCore.Mvc;

namespace EnsekCodingExercise.ApiService.Controllers
{
    /// <summary>
    /// Controller for managing accounts
    /// </summary>
    public class AccountsController : BaseController
    {
        /// <summary>
        /// Get a list of all accounts
        /// </summary>
        /// <returns>A list of accounts</returns>
        [ApiVersion("1")]
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<AccountDto>>> GetAllAccounts()
        {
            return Ok();
        }
    }
}
