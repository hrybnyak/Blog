using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRegularUser([FromBody] UserDTO user)
        {
            try
            {
                var result = await _accountService.RegisterRegularUser(user);
                if (result != null)
                {
                    return CreatedAtAction(nameof(GetUserById), result.Id, result);
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

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(string id)
        {
            return Ok();
        }
    }
}