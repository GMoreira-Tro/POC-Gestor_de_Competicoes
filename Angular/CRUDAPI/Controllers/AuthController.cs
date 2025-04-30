using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDAPI.Models;

namespace CRUDAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly Contexto _contexto;
        
        private readonly JwtService _jwtService;

        public AuthController(Contexto contexto, JwtService jwtService)
        {
            _contexto = contexto;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _contexto.Usuarios.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);

            if (user == null || !user.EmailConfirmado || !BCrypt.Net.BCrypt.Verify(loginRequest.Senha, user.SenhaHash))
            {
                return Unauthorized(new CredenciaisInvalidasException().Message);
            }

            var token = _jwtService.GenerateToken(user.Id); // Gere um token JWT aqui

            return Ok(new { token, userId = user.Id });
        }
            
        public class LoginRequest
        {
            public string Email { get; set; }
            public string Senha { get; set; }
        }
    }
}