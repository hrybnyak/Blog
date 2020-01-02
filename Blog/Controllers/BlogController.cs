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
    [Authorize(Roles = "RegularUser")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly ILogger<BlogController> _logger;

        public BlogController(IBlogService accountService, ILogger<BlogController> logger)
        {
            _logger = logger;
            _blogService = accountService;
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetBlogById(int id)
        {
            try
            {
                var blog = _blogService.GetBlogById(id);
                if (blog == null) throw new ArgumentNullException(nameof(blog));
                _logger.LogInformation("User successfully got blog information by id");
                return Ok(blog);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while user tried to get blog info by id");
                throw;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBlog([FromBody] BlogDTO blog)
        {
            try
            {
                var result = await _blogService.CreateBlog(blog, AuthInfo());
                if (result != null)
                {
                    _logger.LogInformation("User successfully created an account");
                    return CreatedAtAction(nameof(GetBlogById), new { id = result.Id }, result);
                }
                else throw new ArgumentNullException();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
            catch (NameIsAlreadyTakenException ex)
            {
                _logger.LogError(ex, "User tried to create blog with name that was already taken");
                return BadRequest(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while user tried to create blog");
                throw;
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteBlog([FromBody] BlogDTO blog)
        {
            try
            {
                _blogService.DeleteBlog(blog, AuthInfo());
                 return NoContent();
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
                _logger.LogError(ex, "Error occurred while user tried to delete blog");
                throw;
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateBlogName([FromBody] BlogDTO blog)
        {
            try
            {
                _blogService.UpdateBlogName(blog, AuthInfo());
                return NoContent();
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
                _logger.LogError(ex, "Error occurred while user tried to delete blog");
                throw;
            }
        }

    }
}