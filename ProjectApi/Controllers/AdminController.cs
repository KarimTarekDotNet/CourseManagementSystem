using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectApi.DTOs;
using ProjectApi.Options;
using ProjectApi.Services;

namespace ProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly JwtService jwtService;

        public AdminController(JwtService jwtService)
        {
            this.jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login(AdminDTO dto)
        {
            if ((dto.Username != "admin" || dto.Email != "admin@test.com") || dto.Password != "admin123")
                return Unauthorized("Invalid credentials");

            var token = jwtService.GenerateToken();

            return Ok(new { token });
        }
    }
}
