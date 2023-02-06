using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.Metrics;

using Step1_WebApi_Initials.Models;
using Microsoft.AspNetCore.Authorization;

namespace Step1_WebApi_Initial.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuotesController : ControllerBase
    {
        private IMockupData _repo;
        private ILogger<QuotesController> _logger;


        //GET: api/Quotes
        //GET: api/Quotes/?count={count}
        //Below are good practice decorators to use for a GET request
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GoodQuote>))]
        [ProducesResponseType(400, Type = typeof(string))]
         public IActionResult GetQoutes(string count)
        {
            _logger.LogInformation("GetQuotes initiated");

            if (string.IsNullOrWhiteSpace(count))
            {
                _logger.LogInformation("GetQuotes returned {count} items", _repo.Quotes.Count);
                return Ok(_repo.Quotes);
            }

            if (!int.TryParse(count, out int _count))
            {
                return BadRequest("count format error");
            }

            _count = Math.Min(_count, _repo.Quotes.Count);
            _logger.LogInformation("GetQuotes returned {_count} items", _count);
            return Ok(_repo.Quotes.Take(_count));
        }

        public QuotesController(IMockupData repo, ILogger<QuotesController> logger)
        {
            _repo = repo;
            _logger = logger;

            _logger.LogInformation($"QuotesController started.");
        }
    }
}