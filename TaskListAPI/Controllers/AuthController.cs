using Microsoft.AspNetCore.Mvc;
using TaskListAPI.Model.DTOs;
using TaskListAPI.Model.Entities; // Assuming Usuario is here
using TaskListAPI.Repository; // Assuming IUsuarioRepository is here
using TaskListAPI.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration; // Needed for configuration access

namespace TaskListAPI.Controllers
{
    // The standard route for authentication endpoints
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Use the specialized repository for users
        private readonly IUsuarioRepository _userRepository;
        private readonly IAuthService _authService;

        // Inject dependencies: User repository and the token generation service
        public AuthController(IUsuarioRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        // POST: api/Auth/login
        // This is the endpoint where users submit credentials. It must be anonymous.
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 1. Retrieve User by Email
            var user = await _userRepository.GetByEmailAsync(model.Email);

            // 2. Validate Credentials
            // **IMPORTANT:** Replace 'false' with your actual secure password verification logic 
            // (e.g., using BCrypt, ASP.NET Identity, or a utility method).
            bool isPasswordValid = _userRepository.VerifyPassword(user, model.Password);

            if (user == null || !isPasswordValid)
            {
                // Return generic unauthorized message for security (don't reveal if user or password failed)
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // 3. Generate Token
            var tokenString = _authService.GenerateJwtToken(user);

            // 4. Return Token and User Info
            var response = new AuthResponseDTO
            {
                Token = tokenString,
                UserId = user.Id,
                UserEmail = user.Email
            };

            return Ok(response);
        }

        // POST: api/Auth/logout
        // For JWTs, logout is often done on the client side, but the server can optionally 
        // implement token revocation or blacklist logic here if required. 
        // For a basic setup, this endpoint can simply be omitted or return a success message.
        [HttpPost("logout")]
        // [Authorize] // If implementing server-side revocation
        public IActionResult Logout()
        {
            // If using JWT, no action is usually needed on the server side.
            // Client is expected to destroy the token.
            return Ok(new { message = "Logged out successfully (token discarded on client)." });
        }
    }
}