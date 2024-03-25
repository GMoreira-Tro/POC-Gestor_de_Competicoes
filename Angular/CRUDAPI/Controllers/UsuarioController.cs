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

        // POST: api/Usuario
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            try
            {
                usuario = await _usuarioService.ValidarUsuario(usuario);

                // Se todas as validações passaram, salva o usuário no banco de dados
                _contexto.Usuarios.Add(usuario);
                await _contexto.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Método para enviar e-mail de confirmação
        [HttpPost("email-confirmation")]
        public async Task<IActionResult> EnviarEmailConfirmacao(string email)
        {
            using (var client = new SmtpClient("smtp.webmail.com", 587))
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("guilherme.moreira@rodwin.com.br", "Gm$1403s");

                var message = new MailMessage();
                message.From = new MailAddress("guilherme.moreira@rodwin.com.br");
                message.To.Add(email);
                message.Subject = "Confirmação de Cadastro";
                message.Body = "Olá, obrigado por se cadastrar! Por favor, confirme seu e-mail para ativar sua conta.";

                await client.SendMailAsync(message);
            }

            return Ok();
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
            var user = await _contexto.Usuarios.FirstOrDefaultAsync(u => u.Email == model.Email && u.SenhaHash == model.Senha);

            if (user == null /*|| !user.EmailConfirmado*/)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("sua-chave-secreta-aqui");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7), // Define a expiração do token (opcional)
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }

    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}