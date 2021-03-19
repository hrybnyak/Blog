using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AdminController> _logger;
        private bool _broken = false;
        public AdminController(IAccountService accountService, ILogger<AdminController> logger)
        {
            _logger = logger;
            _accountService = accountService;
        }

        private string AuthInfo()
        {
            string accessToken = User.FindFirst("access_token")?.Value;
            if (accessToken == null) throw new ArgumentNullException("Couldn't get the token user authorized with");
            return accessToken;
        }


        [HttpGet]
        [Route("moderators")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetModerators()
        {
            try
            {
                var user = await _accountService.GetAllModerators();
                if (user == null) throw new ArgumentNullException(nameof(user));
                _logger.LogInformation("Admin successfully got information about all moderators");
                return Ok(user);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while admin tried to get all moderators' info");
                throw;
            }
        }

        [HttpGet]
        [Route("users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUsersAdmin()
        {
            try
            {
                if (_broken)
                {
                    Thread.Sleep(10000);
                }
                var user = await _accountService.GetAllUsers();
                if (user == null) throw new ArgumentNullException(nameof(user));
                _logger.LogInformation("Admin successfully got information about all users");
                return Ok(user);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while admin tried to get all users' info");
                throw;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("breakUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult BreakUsersAdmin()
        {
            _broken = true;
            return Ok();
        }

        [HttpGet]
        [Route("users/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var user = await _accountService.GetUserById(id, AuthInfo());
                if (user == null) throw new ArgumentNullException(nameof(user));
                _logger.LogInformation("User successfully got information about other user");
                return Ok(user);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound();
            }
            catch (NotEnoughtRightsException ex)
            {
                _logger.LogError(ex, ex.Message);
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while user tried to get user info");
                throw;
            }
        }

        [HttpDelete]
        [Route("moderators/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteModerator(string id)
        {
            try
            {
                bool result = await _accountService.DeleteUser(id, AuthInfo());
                if (result == true)
                {
                    _logger.LogInformation("Admin succesfully deleted moderator");
                    return NoContent();
                }
                else return BadRequest();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound();
            }
            catch (NotEnoughtRightsException ex)
            {
                _logger.LogError(ex, ex.Message);
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while admin tried to delete moderator");
                throw;
            }
        }


        [HttpPost]
        [Route("moderators")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateModerator([FromBody] UserDTO user)
        {
            try
            {
                var result = await _accountService.RegisterModerator(user);
                if (result != null)
                {
                    _logger.LogInformation("Admin successfully created a moderator account");
                    return CreatedAtAction(nameof(GetUserById), new { id = result.Id }, result);
                }
                else throw new ArgumentNullException();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
            catch (EmailIsAlreadyTakenException ex)
            {
                _logger.LogError(ex, "Admin tried to created a moderator account with email that was alreasy taken");
                return BadRequest(ex);
            }
            catch (NameIsAlreadyTakenException ex)
            {
                _logger.LogError(ex, "Admin tried to created a moderator account with username that was alreasy taken");
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while admin tried to create a moderator account");
                throw;
            }
        }
    }
}