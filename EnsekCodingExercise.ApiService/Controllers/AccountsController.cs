using Asp.Versioning;
using EnsekCodingExercise.ApiService.Infrastructure.BaseControllers;
using EnsekCodingExercise.ApiService.Models.External;
using EnsekCodingExercise.ApiService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnsekCodingExercise.ApiService.Controllers
{
    /// <summary>
    /// Controller for managing accounts
    /// </summary>
    public class AccountsController : BaseController
    {
        private readonly IAccountsService _accountsService;

        /// <summary>
        /// Accounts Controller Constructor
        /// </summary>
        /// <param name="accountsService">The Account Service</param>
        public AccountsController(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        /// <summary>
        /// Get a list of all accounts
        /// </summary>
        /// <returns>A list of accounts</returns>
        [ApiVersion("1")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<AccountDto>>> GetAllAccounts()
        {
            // Not asked for but allows me to test that the seeding worked as intended.
            var accounts = await _accountsService.GetAllAccounts();
            return Ok(accounts);
        }

        /// <summary>
        /// Get an Account by its ID
        /// </summary>
        /// <param name="id">The Account ID</param>
        /// <returns>An Account matching the given ID</returns>
        [ApiVersion("1")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AccountDto>>> GetAccountById(int? id)
        {
            if (id == null)
            {
                return BadRequest("An Account ID is required");
            }
            else
            {
                // Not asked for but allows me to test that the seeding worked as intended.
                var account = await _accountsService.GetAccountById(id.Value);
                if (account == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(account);
                }
            }
        }

        /// <summary>
        /// Create a new Account
        /// </summary>
        /// <param name="editAccountModel">The Account to be created</param>
        /// <returns>The ID of the created account</returns>
        [ApiVersion("1")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> CreateAccount([FromBody] CreateAccountModel editAccountModel)
        {
            if (ModelState.IsValid)
            {
                var accountId = await _accountsService.CreateAccount(editAccountModel);
                return Created(string.Empty, accountId);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Edit an Account
        /// </summary>
        /// <param name="id">The ID of the Account to be edited</param>
        /// <param name="editAccountModel">The Account to be edited</param>
        /// <returns>A no content status if the Account was edited</returns>
        [ApiVersion("1")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> EditAccount(int? id, [FromBody] EditAccountModel editAccountModel)
        {
            // Might seem a bit odd to have the ID in the URL and the model but it's a common pattern and 
            // has saved many a headache when it comes to ensuring that he right data is being edited.
            if (ModelState.IsValid)
            {
                if (id.HasValue && id == editAccountModel.AccountId)
                {
                    await _accountsService.EditAccount(editAccountModel);
                    return NoContent();
                }
                else
                {
                    ModelState.AddModelError(nameof(id), "The Account IDs do not match");
                    return BadRequest(ModelState);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Delete an Account by its ID. All associated readings will also be deleted.
        /// </summary>
        /// <param name="id">The Account ID</param>
        /// <returns>No content if the account is deleted or not found if there is no account with that ID</returns>
        [ApiVersion("1")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAccountById(int? id)
        {
            if (id == null)
            {
                return BadRequest("An Account ID is required");
            }
            else
            {
                // Not asked for but allows me to test that the seeding worked as intended.
                if (await _accountsService.DeleteAccountById(id.Value))
                {
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}
