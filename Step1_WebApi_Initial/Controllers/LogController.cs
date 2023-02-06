using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Step1_WebApi_Initials.Logger;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Step1_WebApi_Initial.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogController : Controller
    {
        private ILogger<LogController> _logger;

        //GET /Log
        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IEnumerable<LogMessage>))]
        public IEnumerable<LogMessage> Get([FromServices] ILoggerProvider myLogger)
        {
            //Should have recieved my custom logger through DI
            if (myLogger is InMemoryLoggerProvider cl)
            {
                return cl.Messages;
            }
            return null;
        }

        public LogController(ILogger<LogController> logger)
        {
            _logger = logger;
            _logger.LogInformation($"LogController started");
        }
    }
}

