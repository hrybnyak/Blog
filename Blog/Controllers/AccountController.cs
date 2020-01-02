using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Authentication;
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

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                string accessToken = User.FindFirst("access_token")?.Value;
                if (accessToken == null) throw new ArgumentNullException("Couldn't get the token user authorized with");
                var user = await _accountService.GetUserById(id, accessToken);
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
                _logger.LogError(ex, ex.Message);
                return BadRequest();
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
                    return CreatedAtAction(nameof(GetUserById), new { id = result.Id }, result);
                }
                else throw new ArgumentNullException();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "User sign up caused argument null exception");
                return BadRequest();
            }
            catch (EmailIsAlreadyTakenException ex)
            {
                _logger.LogError(ex, "User tried to sign up with email that was already taken");
                return BadRequest(ex);
            }
            catch (UsernameIsAlreadyTakenException ex)
            {
                _logger.LogError(ex, "User tried to sign up with username that was alreasy taken");
                return BadRequest(ex);
            }
            catch (InvalidPasswordException ex)
            {
                _logger.LogError(ex, "User tried to sign up with invalid password");
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while user tried to sign up");
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser (string id)
        {
            try
            {
                string accessToken = User.FindFirst("access_token")?.Value;
                if (accessToken == null) throw new ArgumentNullException("Couldn't get the token user authorized with");
                bool result = await _accountService.DeleteUser(id, accessToken);
                if (result == true) return NoContent();
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
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }
    }
}