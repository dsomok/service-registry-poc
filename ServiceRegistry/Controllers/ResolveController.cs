using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ServiceRegistry.Consul;
using ServiceRegistry.Exceptions;
using ServiceRegistry.Models.Response;

namespace ServiceRegistry.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ResolveController : ControllerBase
    {
        private readonly IServiceRegistry _serviceRegistry;

        public ResolveController(IServiceRegistry serviceRegistry)
        {
            _serviceRegistry = serviceRegistry;
        }

        [HttpGet]
        [Route("resolve")]
        [ProducesResponseType(typeof(ResolveResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Resolve(string name)
        {
            try
            {
                var resolveResponse = await _serviceRegistry.ResolveAsync(name, HttpContext.RequestAborted);
                return Ok(resolveResponse);
            }
            catch (ServiceNotFoundException)
            {
                return NotFound();
            }
            catch (ServiceUnhealthyException)
            {
                return StatusCode(StatusCodes.Status410Gone);
            }
        }
    }
}
