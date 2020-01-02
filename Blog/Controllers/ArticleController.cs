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
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ILogger<ArticleController> _logger;

        public ArticleController(IArticleService articleService, ILogger<ArticleController> logger)
        {
            _logger = logger;
            _articleService = articleService;
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
        public IActionResult GetArticlesById(int id)
        {
            try
            {
                var blog = _articleService.GetArticleById(id);
                if (blog == null) throw new ArgumentNullException(nameof(blog));
                _logger.LogInformation("User successfully got article information by id");
                return Ok(blog);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while user tried to get article info by id");
                throw;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "RegularUser")]
        public async Task<IActionResult> CreateArticle([FromBody] ArticleDTO article)
        {
            try
            {
                var result = await _articleService.CreateArticle(article, AuthInfo());
                if (result != null)
                {
                    _logger.LogInformation("User successfully created an account");
                    return CreatedAtAction(nameof(GetArticlesById), new { id = result.Id }, result);
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
        public IActionResult DeleteArticle([FromBody] ArticleDTO article)
        {
            try
            {
                _articleService.DeleteArticle(article, AuthInfo());
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
        [Authorize(Roles = "RegularUser")]
        public IActionResult UpdateBlogName([FromBody] ArticleDTO blog)
        {
            try
            {
                _articleService.UpdateArticle(blog, AuthInfo());
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