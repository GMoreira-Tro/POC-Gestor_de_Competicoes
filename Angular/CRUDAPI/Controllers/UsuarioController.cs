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
        private readonly EmailService _emailService;

        public UsuarioController(Contexto contexto, UsuarioService usuarioService, EmailService emailService)
        {
            _contexto = contexto;
            _usuarioService = usuarioService;
            _emailService = emailService;
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
        public async Task<ActionResult<Usuario?>> GetUsuarioByEmail(string email)
        {
            var usuario = await _contexto.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

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

        [HttpPost("reenviar-email-confirmacao")]
        public async Task<ActionResult> ReenviarEmailConfirmacao(string email)
        {
            var user = await _contexto.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return BadRequest("Usuário não encontrado");

            if (user.EmailConfirmado)
            {
                return BadRequest("E-mail já confirmado");
            }

            user.TokenConfirmacao = _usuarioService.GenerateToken(user.Email);
            await _contexto.SaveChangesAsync();

            await _usuarioService.EnviarTokenConfirmacao(user.TokenConfirmacao, user.Email);

            return Ok("E-mail de confirmação reenviado com sucesso!");
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

        [HttpGet("confirmar-email/{token}")]
        public async Task<IActionResult> ConfirmarEmail(string token)
        {
            var user = await _contexto.Usuarios.FirstOrDefaultAsync(u => u.TokenConfirmacao == token);
            if (user == null) return BadRequest("Token inválido");

            user.EmailConfirmado = true;
            user.TokenConfirmacao = "";  // Remove o token após a confirmação
            await _contexto.SaveChangesAsync();

            return Ok("E-mail confirmado com sucesso! Agora você pode fazer login.");
        }

        [HttpPost("{id}/upload-imagem")]
        public async Task<IActionResult> UploadImagem(long id, IFormFile imagem)
        {
            var usuario = await _contexto.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound("Usuário não encontrado");

            if (imagem == null || imagem.Length == 0)
                return BadRequest("Nenhuma imagem foi enviada");

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}_{imagem.FileName}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imagem.CopyToAsync(stream);
            }

            usuario.ImagemUrl = $"/uploads/{fileName}";
            _contexto.Usuarios.Update(usuario);
            await _contexto.SaveChangesAsync();

            return Ok(new { imagemUrl = usuario.ImagemUrl });
        }

        [HttpPost("enviar-email-suporte/{id}")]
        public async Task<IActionResult> EnviarEmailSuporte(long id, [FromBody] EmailConfirmacaoRequest request)
        {
            var usuario = await _contexto.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound("Usuário não encontrado");

            var assunto = request.Assunto;
            var mensagem = request.Mensagem;

            // Adiciona o e-mail do usuário no corpo da mensagem
            var mensagemCompleta = $"Usuário: {usuario.Email}\n\n{mensagem}";
            await _emailService.SendEmailAsync(_emailService._emailFrom, assunto, mensagemCompleta);

            return Ok("E-mail enviado com sucesso!");
        }
        public class EmailConfirmacaoRequest
        {
            public string Assunto { get; set; }
            public string Mensagem { get; set; }
        }
    }
}