using Microsoft.AspNetCore.Mvc;
using PredescuAlexandru_API.Models.Authentification;
using PredescuAlexandru_API.Repository.Interfaces;
using System.Net;

namespace PredescuAlexandru_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserRepository _userRepository;
        private ILogger<AuthController> _logger;
        public AuthController(IUserRepository userRepository, ILogger<AuthController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(AuthentiticateRequest request)
        {
            var response = await _userRepository.Authenticate(request);
            if (response == null)
            {
                _logger.LogWarning("Cineva cu user si parola gresita vrea sa se conecteze");
                return StatusCode((int)HttpStatusCode.BadRequest, "User sau parola gresita");
            }
            return Ok(response);
        }
    }
}
