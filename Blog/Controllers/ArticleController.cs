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

        public IActionResult GetArticleById(int id)
        {
            try
            {
                var article = _articleService.GetArticleById(id);
                if (article == null) throw new ArgumentNullException(nameof(article));
                _logger.LogInformation("User successfully got article information by id");
                return Ok(article);
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllArticles()
        {
            try
            {
                var articles = _articleService.GetAllArticles();
                if (articles == null) throw new ArgumentNullException(nameof(articles));
                _logger.LogInformation("User successfully got articles' information");
                return Ok(articles);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while user tried to get articles' info");
                throw;
            }
        }

        [HttpPost]
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
                    _logger.LogInformation("User successfully posted an article");
                    return CreatedAtAction(nameof(GetArticleById), new { id = result.Id }, result);
                }
                else throw new ArgumentNullException();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while user tried to post an article");
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
                _logger.LogError(ex, "Error occurred while user tried to delete an article");
                throw;
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "RegularUser")]
        public IActionResult UpdateArticle([FromBody] ArticleDTO article)
        {
            try
            {
                _articleService.UpdateArticle(article, AuthInfo());
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
                _logger.LogError(ex, "Error occurred while user tried to update article");
                throw;
            }
        }

        [HttpGet]
        [Route("filter/{filter}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetArticlesWithTextFilter(string filter)
        {
            try
            {
                var articles = _articleService.GetArticlesWihtTextFilter(filter);
                if (articles == null) throw new ArgumentNullException(nameof(articles));
                _logger.LogInformation("User successfully got article information by id");
                return Ok(articles);
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

        [HttpGet]
        [Route("{id}/tegs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetArticlesTegs(int id)
        {
            try
            {
                var tegs = _articleService.GetTegsByArticleId(id);
                if (tegs == null) throw new ArgumentNullException(nameof(tegs));
                _logger.LogInformation("User successfully got all tegs of article by article id");
                return Ok(tegs);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while user tried to get all tegs of article by article id");
                throw;
            }
        }

        [HttpGet]
        [Route("{id}/comments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetArticlesComments(int id)
        {
            try
            {
                var comments = _articleService.GetCommentsByArticleId(id);
                if (comments == null) throw new ArgumentNullException(nameof(comments));
                _logger.LogInformation("User successfully all comments of article by article id");
                return Ok(comments);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while user tried to get all comments of article by article id");
                throw;
            }
        }
    }
}