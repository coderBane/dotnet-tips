using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly IDbContextFactory<SampleDbContext> _dbContextFactory;

        public PeopleController(IDbContextFactory<SampleDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        // GET: api/values
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Person>))]
        [ProducesResponseType(500, Type = typeof(string))]
        public IActionResult Get()
        {
            try
            {
                using var context = _dbContextFactory.CreateDbContext();
                return Ok(context.People.ToList());
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message)
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }
    }
}

