using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ServiceRegistry.Consul;
using ServiceRegistry.Models.Request;

namespace ServiceRegistry.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IServiceRegistry _serviceRegistry;


        public RegistrationController(IServiceRegistry serviceRegistry)
        {
            _serviceRegistry = serviceRegistry;
        }


        [HttpPost]
        [Route("register/{name}")]
        public async Task<IActionResult> Register(string name, [FromBody] RegistrationRequest registrationRequest)
        {
            await _serviceRegistry.RegisterAsync(name, registrationRequest, HttpContext.RequestAborted);
            return Ok();
        }

        [HttpPost]
        [Route("unregister/{name}")]
        public async Task<IActionResult> Unregister(string name)
        {
            await _serviceRegistry.UnregisterAsync(name, HttpContext.RequestAborted);
            return Ok();
        }
    }
}
