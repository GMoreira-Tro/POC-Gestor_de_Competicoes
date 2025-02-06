using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/webhook")]
public class PixWebhookController : ControllerBase
{
    private readonly ILogger<PixWebhookController> _logger;
    private readonly GerencianetService _gerencianetService; // Service para salvar info no banco

    public PixWebhookController(ILogger<PixWebhookController> logger, GerencianetService paymentService)
    {
        _logger = logger;
        _gerencianetService = paymentService;
    }

    [HttpPost("efi")]
    public async Task<IActionResult> ReceivePixWebhook([FromBody] PixWebhookNotification notification)
    {
        if (notification == null || notification.pix == null || notification.pix.Count == 0)
        {
            _logger.LogWarning("Webhook recebido sem dados.");
            return BadRequest("Dados inválidos.");
        }

        // Pegamos os dados do primeiro pagamento recebido no webhook
        var pagamento = notification.pix.First();
        var txid = pagamento.txid;
        var status = pagamento.status; // Provavelmente será "CONCLUIDO"
        var userId = pagamento.userId;

        _logger.LogInformation($"Pagamento recebido. Txid: {txid}, Status: {status}, User Id: {userId}");

        // Salva no banco de dados
        await _gerencianetService.UpdatePaymentStatus(txid, status, userId);

        return Ok(new { message = "Webhook recebido com sucesso." });
    }
}
