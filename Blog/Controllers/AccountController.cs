﻿using System;
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
        [Route("{id}")]
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
                _logger.LogInformation("User successfully got information by id");
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
                _logger.LogError(ex, "Error occurred while user tried to get user info by id");
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
                _logger.LogError(ex, "User tried to sign up with email that was alreasy taken");
                return BadRequest(ex);
            }
            catch (NameIsAlreadyTakenException ex)
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
                throw;
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
                bool result = await _accountService.DeleteUser(id, AuthInfo());
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
            catch (NotEnoughtRightsException ex)
            {
                _logger.LogError(ex, ex.Message);
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while user tried to delete account");
                throw;
            }
        }
    }
}