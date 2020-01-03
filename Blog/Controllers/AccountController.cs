using System;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var user = await _accountService.GetUserById(AuthInfo());
                if (user == null) throw new ArgumentNullException(nameof(user));
                _logger.LogInformation("User successfully got information about himself");
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

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRegularUser([FromBody] UserDTO user)
        {
            try
            {
                var result = await _accountService.RegisterRegularUser(user);
                if (result != null)
                {
                    _logger.LogInformation("User successfully created an account");
                    return CreatedAtAction(nameof(GetUser), result);
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
                _logger.LogError(ex, "User tried to sign up with email that was alreasy taken");
                return BadRequest(ex);
            }
            catch (NameIsAlreadyTakenException ex)
            {
                _logger.LogError(ex, "User tried to sign up with username that was alreasy taken");
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while user tried to sign up");
                throw;
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                bool result = await _accountService.DeleteUser(AuthInfo());
                if (result == true)
                {
                    _logger.LogInformation("User succesfully deleted account");
                    return NoContent();
                }
                else return BadRequest();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while user tried to delete account");
                throw;
            }
        }

        [HttpPut]
        [Route("username")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUsername([FromBody] UserDTO dto)
        {
            try
            {
                await _accountService.UpdateUsername(dto, AuthInfo());
                _logger.LogInformation("User succesfully updated his username");
                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound();
            }
            catch (NameIsAlreadyTakenException ex)
            {
                _logger.LogError(ex, "User tried to change username to the one that was already taken");
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while user tried to unpdate username");
                throw;
            }
        }

        [HttpPut]
        [Route("password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePassword([FromBody] PasswordDTO dto)
        {
            try
            {
                await _accountService.ChangePassword(dto, AuthInfo());
                _logger.LogInformation("User successfully updated password");
                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "User entered incorrect current password");
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while user tried to update passport");
                throw;
            }
        }
    }
}