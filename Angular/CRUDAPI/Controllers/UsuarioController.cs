using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDAPI.Models;
using System.Net.Mail;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using CRUDAPI.Services;

namespace CRUDAPI.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly Contexto _contexto;
        private readonly UsuarioService _usuarioService;

        public UsuarioController(Contexto contexto, UsuarioService usuarioService)
        {
            _contexto = contexto;
            _usuarioService = usuarioService;
        }

        // GET: api/Usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _contexto.Usuarios.ToListAsync();
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(long id)
        {
            var usuario = await _contexto.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<Usuario>> GetUsuarioByEmail(string email)
        {
            var usuario = await _contexto.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // POST: api/Usuario
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            try
            {
                usuario = await _usuarioService.ValidarUsuario(usuario);

                // Se todas as validações passaram, salva o usuário no banco de dados
                // Gera token de confirmação
                usuario.TokenConfirmacao = _usuarioService.GenerateToken(usuario.Email);
                _contexto.Usuarios.Add(usuario);
                await _contexto.SaveChangesAsync();

                await _usuarioService.EnviarTokenConfirmacao(usuario.TokenConfirmacao, usuario.Email);

                return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Usuario/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(long id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(usuario).State = EntityState.Modified;

            try
            {
                usuario = await _usuarioService.ValidarUsuario(usuario);
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_usuarioService.UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        // DELETE: api/Usuario/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Usuario>> DeleteUsuario(long id)
        {
            var usuario = await _contexto.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _contexto.Usuarios.Remove(usuario);
            await _contexto.SaveChangesAsync();

            return usuario;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            // Buscar o usuário pelo e-mail no banco de dados
            var user = await _contexto.Usuarios.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                return Unauthorized(); // Usuário não encontrado
            }

            // Verificar se a senha fornecida corresponde ao hash no banco de dados
            if (BCrypt.Net.BCrypt.Verify(model.Senha, user.SenhaHash))
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes("k3ZvM4v9BpS+UdW7Y4XeFtHj2NsJ8bRa");

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, user.Id.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(7), // Token expira em 7 dias
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);

                    return Ok(new { Token = tokenString });
                }
                catch (Exception ex)
                {
                    // Log do erro
                    Console.WriteLine($"Erro ao gerar token JWT: {ex.Message}");
                    return StatusCode(500, "Erro interno no servidor ao gerar token JWT");
                }
            }
            else
            {
                return Unauthorized(); // Senha incorreta
            }
        }

        [HttpGet("confirmar-email")]
        public async Task<IActionResult> ConfirmarEmail(string token)
        {
            var user = await _contexto.Usuarios.FirstOrDefaultAsync(u => u.TokenConfirmacao == token);
            if (user == null) return BadRequest("Token inválido");

            user.EmailConfirmado = true;
            user.TokenConfirmacao = "";  // Remove o token após a confirmação
            await _contexto.SaveChangesAsync();

            return Ok("E-mail confirmado com sucesso! Agora você pode fazer login.");
        }

    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}