using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;
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
    public class TegController : ControllerBase
    {
        private readonly ITegService _tegService;
        private readonly ILogger<TegController> _logger;

        public TegController(ITegService tegService, ILogger<TegController> logger)
        {
            _tegService = tegService;
            _logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult GetTegsById(int id)
        {
            try
            {
                var article = _tegService.GetTegById(id);
                if (article == null) throw new ArgumentNullException(nameof(article));
                _logger.LogInformation("User successfully got teg information by id");
                return Ok(article);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while user tried to get teg info by id");
                throw;
            }
        }


        [HttpGet]
        [Route("articles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult GetArticlesByTeg([FromBody] IEnumerable<TegDTO> teg)
        {
            try
            {
                var article = _tegService.GetArticlesByTeg(teg);
                if (article == null) throw new ArgumentNullException(nameof(article));
                _logger.LogInformation("User successfully got articles information by tegs");
                return Ok(article);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while user tried to get articles information by tegs");
                throw;
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult GetTegs()
        {
            try
            {
                var article = _tegService.GetAllTegs();
                if (article == null) throw new ArgumentNullException(nameof(article));
                _logger.LogInformation("User successfully got tegs' information by id");
                return Ok(article);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while user tried to get tegs' info by id");
                throw;
            }
        }
    }
}