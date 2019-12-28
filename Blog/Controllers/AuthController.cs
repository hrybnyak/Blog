using System;
using System.Collections.Generic;
using System.Linq;
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
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Authenticate([FromBody] UserDTO user)
        {
            try
            {
                var result = await _authService.Authenticate(user);
                if (result == null) throw new ArgumentNullException();
                _logger.LogInformation($"User {user.UserName} succesfully logged in");
                return Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "User authentication caused null exception");
                return BadRequest(ex);
            }
            catch (WrongCredentialsException ex)
            {
                _logger.LogError(ex, "User entered wrong credentials");
                return NotFound(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while user authentication");
                return BadRequest();
            }
        }
    }
}