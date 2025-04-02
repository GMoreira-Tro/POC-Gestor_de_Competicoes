using CRUDAPI.Models;
using GerencianetPix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class PixController : ControllerBase
{
    private readonly GerencianetService _gerencianetService;
    private readonly Contexto _context;
    private readonly EmailService _emailService;

    public PixController(GerencianetService gerencianetService, Contexto context, EmailService emailService)
    {
        _gerencianetService = gerencianetService;
        _context = context;
        _emailService = emailService;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GeneratePix([FromBody] PixCharge charge)
    {
        try
        {
            string qrCodeUrl = await _gerencianetService.CreatePixCharge(charge);
            return Ok(new { qrCodeUrl });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("base64/{id}")]
    public async Task<IActionResult> GenerateBase64QrCode(string id)
    {
        try
        {
            string base64QrCode = await _gerencianetService.GenerateBase64QrCode(id);
            return Ok(new { base64QrCode });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("cancel/{txid}")]
    public async Task<IActionResult> CancelPix(string txid)
    {
        try
        {
            string result = await _gerencianetService.CancelPixCharge(txid);
            return Ok(new { message = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("consulta")]
    public async Task<IActionResult> ConsultaSaldo()
    {
        try
        {
            string result = await _gerencianetService.ConsultaSaldo();
            return Ok(new { message = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("devolucao/{e2eId}/{transactionId}")]
    public async Task<IActionResult> DevolverPix([FromBody] PixRefundRequest refundRequest, string e2eId, string transactionId)
    {
        try
        {
            var refundResponse = await _gerencianetService.DevolverPixAsync(refundRequest, e2eId, transactionId);
            return Ok(refundResponse);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao realizar devolução do Pix: {ex.Message}");
        }
    }

    [HttpGet("devolucao/{e2eId}/{transactionId}")]
    public async Task<IActionResult> ConsultarDevolucaoPix(string e2eId, string transactionId)
    {
        try
        {
            var refundResponse = await _gerencianetService.ConsultarDevolverPixAsync(e2eId, transactionId);
            return Ok(refundResponse);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao realizar devolução do Pix: {ex.Message}");
        }
    }

    [HttpPost("envio/{idEnvio}")]
    public async Task<IActionResult> EnviarPix([FromBody] PixSent pixSent, string idEnvio)
    {
        try
        {
            string result = await _gerencianetService.EnviarPix(pixSent, idEnvio);
            return Ok(new { message = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("payments/consulta")]
    public async Task<IActionResult> ConsultaPix(string inicio, string fim)
    {
        try
        {
            string result = await _gerencianetService.ConsultarPix(inicio, fim);
            return Ok(new { message = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("payments/consulta/{e2eId}")]
    public async Task<IActionResult> ConsultaPixPorE2EId(string e2eId)
    {
        try
        {
            string result = await _gerencianetService.ConsultaPixPorE2EId(e2eId);
            return Ok(new { message = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("payments/consulta-txid/{txid}")]
    public async Task<IActionResult> ConsultaPixPorTxId(string txid)
    {
        try
        {
            string result = await _gerencianetService.ConsultaPixPorTxId(txid);
            return Ok(new { message = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("payments/consulta-location/{locId}")]
    public async Task<IActionResult> ConsultaLocationPorLocId(string locId)
    {
        try
        {
            string result = await _gerencianetService.ConsultaLocationPorLocId(locId);
            return Ok(new { message = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("payments")]
    public async Task<ActionResult<IEnumerable<Pagamento>>> GetPagamentos()
    {
        return await _context.Pagamentos.ToListAsync();
    }

    [HttpGet("payments/{id}")]
    public async Task<ActionResult<Pagamento>> GetPagamentos(long id)
    {
        var pagamento = await _context.Pagamentos.FindAsync(id);

        if (pagamento == null)
        {
            return NotFound();
        }

        return pagamento;
    }

    [HttpDelete("payments/{id}")]
    public async Task<IActionResult> DeletePagamentos(long id)
    {
        var payment = await _context.Pagamentos.FindAsync(id);
        if (payment == null)
        {
            return NotFound();
        }

        _context.Pagamentos.Remove(payment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("payments/user")]
    public async Task<IActionResult> RegistrarPagamentoPorUserId([FromBody] Pagamento payment)
    {
        try
        {
            payment = await _gerencianetService.ValidarPagamento(payment);

            _context.Pagamentos.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPagamentos), new { id = payment.Id }, payment);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("receber-email-inscricao/{idInscricao}")]
    public async Task<IActionResult> ReceberEmailInscricao(long idInscricao)
    {
        var inscricao = await _context.Inscricoes.FindAsync(idInscricao);
        if (inscricao == null) return NotFound("Inscrição não encontrada");
        if (inscricao.Status != InscricaoStatus.Paga) return BadRequest("Inscrição não paga");
        var competidor = await _context.Competidores.FindAsync(inscricao.CompetidorId);
        if (competidor == null) return NotFound("Competidor não encontrado");
        var categoria = await _context.Categorias.FindAsync(inscricao.CategoriaId);
        if (categoria == null) return NotFound("Categoria não encontrada");
        var competicao = await _context.Competicoes.FindAsync(categoria.CompeticaoId);
        if (competicao == null) return NotFound("Competição não encontrada");
        var Organizador = await _context.Usuarios.FindAsync(competicao.CriadorUsuarioId);
        if (Organizador == null) return NotFound("Organizador não encontrado");

        var assunto = "Nova Inscrição Recebida: " + inscricao.Id;
        var mensagem = $"O organizador {Organizador.Nome}({Organizador.Id}) recebeu uma nova inscrição para a competição " + 
        $"{competicao.Titulo}({competicao.Id})" + $" na categoria '{categoria.Nome}'. " +
                   $"O valor da inscrição é R$ {categoria.ValorInscricao:F2}." +
                   $"O competidor é {competidor.Nome}({competidor.Id})." +
                   $"O PIX da competição é {competicao.ChavePix}.";
        
        await _emailService.SendEmailAsync(_emailService._emailFrom, assunto, mensagem);

        return Ok();
    }

    [HttpGet("payments/user/{userId}")]
    public async Task<IActionResult> GetUserPayments(int userId)
    {
        var payments = await _context.Pagamentos
            .Where(p => p.PagadorId == userId)
            .ToListAsync();

        return Ok(payments);
    }

}