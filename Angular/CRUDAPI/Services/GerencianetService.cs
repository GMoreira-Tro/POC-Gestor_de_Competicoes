using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using RestSharp;
using Efipay;
using System.Text;
using Microsoft.EntityFrameworkCore;
using GerencianetPix.Models;
using CRUDAPI.Models;

public class GerencianetService
{
    private readonly IConfiguration _configuration;
    private readonly Contexto _context;

    public GerencianetService(IConfiguration configuration, Contexto context)
    {
        _configuration = configuration;
        _context = context;
    }

    public async Task<string> CreatePixCharge(PixCharge charge)
    {
        dynamic efi = new EfiPay(_configuration["Gerencianet:ClientId"], _configuration["Gerencianet:ClientSecret"],
         false, _configuration["Gerencianet:CertificatePath"]);

        return efi.PixCreateImmediateCharge(null, charge);
    }

    public async Task<string> GetAccessToken(RestClient client)
    {
        // Autenticação
        string clientId = _configuration["Gerencianet:ClientId"];
        string clientSecret = _configuration["Gerencianet:ClientSecret"];
        string credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
        var tokenRequest = new RestRequest("/oauth/token", Method.Post);
        tokenRequest.AddHeader("Authorization", $"Basic {credentials}");
        tokenRequest.AddParameter("grant_type", "client_credentials");

        var tokenResponse = await client.ExecuteAsync(tokenRequest);
        if (!tokenResponse.IsSuccessful)
            throw new Exception($"Erro ao obter token: {tokenResponse.Content}");

        var tokenData = JsonConvert.DeserializeObject<dynamic>(tokenResponse.Content);
        return tokenData.access_token;
    }
    public async Task<string> GenerateBase64QrCode(string id)
    {
        var baseUrl = _configuration["Gerencianet:BaseUrl"];
        var clientOptions = new RestClientOptions(baseUrl)
        {
            ConfigureMessageHandler = handler =>
            {
                var httpHandler = new HttpClientHandler();
                httpHandler.ClientCertificates.Add(new X509Certificate2(_configuration["Gerencianet:CertificatePath"]));
                return httpHandler;
            }
        };

        var client = new RestClient(clientOptions);
        var request = new RestRequest($"/v2/loc/{id}/qrcode", Method.Get);

        string accessToken = await GetAccessToken(client);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        //request.AddHeader("Authorization", $"Basic {credentials}");

        // Executa a requisição
        var response = await client.ExecuteAsync(request);

        if (!response.IsSuccessful)
            throw new Exception($"Erro ao gerar base 64: {response.Content}");

        return response.Content;
    }

    public async Task<string> CancelPixCharge(string txid)
    {
        // Verifica se está em modo de simulação
        bool isSimulation = bool.Parse(_configuration["Gerencianet:Simulation"] ?? "false");

        if (isSimulation)
        {
            // Simulação de cancelamento de cobrança
            return $"Cobrança com txid {txid} cancelada com sucesso (simulação).";
        }

        var baseUrl = _configuration["Gerencianet:BaseUrl"];
        var clientOptions = new RestClientOptions(baseUrl)
        {
            ConfigureMessageHandler = handler =>
            {
                var httpHandler = new HttpClientHandler();
                httpHandler.ClientCertificates.Add(new X509Certificate2(_configuration["Gerencianet:CertificatePath"]));
                return httpHandler;
            }
        };

        var client = new RestClient(clientOptions);
        var request = new RestRequest($"/v2/cob/{txid}/cancelar", Method.Post);

        // Autenticação
        string clientId = _configuration["Gerencianet:ClientId"];
        string clientSecret = _configuration["Gerencianet:ClientSecret"];
        string credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
        request.AddHeader("Authorization", $"Basic {credentials}");

        // Executa a requisição
        var response = await client.ExecuteAsync(request);

        if (!response.IsSuccessful)
            throw new Exception($"Erro ao cancelar cobrança: {response.Content}");

        return "Cobrança cancelada com sucesso";
    }

    public async Task<string> ConsultaSaldo()
    {
        var baseUrl = _configuration["Gerencianet:BaseUrl"];
        var clientOptions = new RestClientOptions(baseUrl)
        {
            ConfigureMessageHandler = handler =>
            {
                var httpHandler = new HttpClientHandler();
                httpHandler.ClientCertificates.Add(new X509Certificate2(_configuration["Gerencianet:CertificatePath"]));
                return httpHandler;
            }
        };

        var client = new RestClient(clientOptions);
        var request = new RestRequest($"/v2/gn/saldo", Method.Get);

        string accessToken = await GetAccessToken(client);
        request.AddHeader("Authorization", $"Bearer {accessToken}");

        // Executa a requisição
        var response = await client.ExecuteAsync(request);

        if (!response.IsSuccessful)
            throw new Exception($"Erro ao consultar saldo: {response.Content}");

        return response.Content;
    }

    public async Task<string> DevolverPixAsync(PixRefundRequest refundRequest, string e2eId, string transactionId)
    {
        var baseUrl = _configuration["Gerencianet:BaseUrl"];
        var certPath = _configuration["Gerencianet:CertificatePath"];

        // Configuração do RestClient
        var clientOptions = new RestClientOptions(baseUrl)
        {
            ConfigureMessageHandler = handler =>
            {
                var httpHandler = new HttpClientHandler();
                httpHandler.ClientCertificates.Add(new X509Certificate2(certPath));
                return httpHandler;
            }
        };

        using var client = new RestClient(clientOptions);
        string accessToken = await GetAccessToken(client);

        // Criando a requisição
        var request = new RestRequest($"/v2/pix/{e2eId}/devolucao/{transactionId}", Method.Put);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddHeader("Content-Type", "application/json");  // Adiciona Content-Type
        request.AddJsonBody(new { valor = refundRequest.Valor }); // Converte o valor para string no formato correto

        // Executa a requisição
        var response = await client.ExecuteAsync(request);

        if (!response.IsSuccessful)
        {
            var errorMessage = $"Erro ao estornar cobrança. Código: {response.StatusCode}, Detalhes: {response.Content}";
            throw new Exception(errorMessage);
        }

        return response.Content;
    }

    public async Task<string> ConsultarDevolverPixAsync(string e2eId, string transactionId)
    {
        var baseUrl = _configuration["Gerencianet:BaseUrl"];
        var certPath = _configuration["Gerencianet:CertificatePath"];

        // Configuração do RestClient
        var clientOptions = new RestClientOptions(baseUrl)
        {
            ConfigureMessageHandler = handler =>
            {
                var httpHandler = new HttpClientHandler();
                httpHandler.ClientCertificates.Add(new X509Certificate2(certPath));
                return httpHandler;
            }
        };

        using var client = new RestClient(clientOptions);
        string accessToken = await GetAccessToken(client);

        // Criando a requisição
        var request = new RestRequest($"/v2/pix/{e2eId}/devolucao/{transactionId}", Method.Get);
        request.AddHeader("Authorization", $"Bearer {accessToken}");

        // Executa a requisição
        var response = await client.ExecuteAsync(request);

        if (!response.IsSuccessful)
        {
            var errorMessage = $"Erro ao estornar cobrança. Código: {response.StatusCode}, Detalhes: {response.Content}";
            throw new Exception(errorMessage);
        }

        return response.Content;
    }

    public async Task<string> EnviarPix(PixSent pixSent, string idEnvio)
    {
        var baseUrl = _configuration["Gerencianet:BaseUrl"];
        var certPath = _configuration["Gerencianet:CertificatePath"];

        // Configuração do RestClient
        var clientOptions = new RestClientOptions(baseUrl)
        {
            ConfigureMessageHandler = handler =>
            {
                var httpHandler = new HttpClientHandler();
                httpHandler.ClientCertificates.Add(new X509Certificate2(certPath));
                return httpHandler;
            }
        };

        using var client = new RestClient(clientOptions);
        string accessToken = await GetAccessToken(client);

        // Criando a requisição
        var request = new RestRequest($"/v2/gn/pix/{idEnvio}", Method.Put);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddHeader("Content-Type", "application/json");  // Adiciona Content-Type
        request.AddJsonBody(new
        {
            valor = pixSent.Valor,
            pagador = new
            {
                chave = pixSent.Pagador.Chave,
                infoPagador = pixSent.Pagador.InfoPagador
            },
            favorecido = new
            {
                chave = pixSent.Favorecido.Chave
            }
        });

        // Executa a requisição
        var response = await client.ExecuteAsync(request);

        if (!response.IsSuccessful)
        {
            var errorMessage = $"Erro ao enviar pix. Código: {response.StatusCode}, Detalhes: {response.Content}";
            throw new Exception(errorMessage);
        }

        return response.Content;
    }

    public async Task UpdatePaymentStatus(string txid, string status, int userId)
    {
        var payment = await _context.Pagamentos.FirstOrDefaultAsync(p => p.Txid == txid);

        if (payment == null)
        {
            payment = new Pagamento
            {
                Txid = txid,
                UserId = userId,  // Salva o ID do usuário!
            };
            _context.Pagamentos.Add(payment);
        }
        else
        {

        }

        await _context.SaveChangesAsync();
    }

    public async Task<string> ConsultarPix(string inicio, string fim)
    {
        var baseUrl = _configuration["Gerencianet:BaseUrl"];
        var certPath = _configuration["Gerencianet:CertificatePath"];

        var clientOptions = new RestClientOptions(baseUrl)
        {
            ConfigureMessageHandler = handler =>
            {
                var httpHandler = new HttpClientHandler();
                httpHandler.ClientCertificates.Add(new X509Certificate2(certPath));
                return httpHandler;
            }
        };

        using var client = new RestClient(clientOptions);
        string accessToken = await GetAccessToken(client);

        // Criando a requisição
        var request = new RestRequest("/v2/pix", Method.Get);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddHeader("Content-Type", "application/json");

        // Adicionando filtros de data (ISO 8601)
        request.AddParameter("inicio", inicio); // Exemplo: "2024-01-01T00:00:00Z"
        request.AddParameter("fim", fim);       // Exemplo: "2024-01-31T23:59:59Z"

        // Executa a requisição
        var response = await client.ExecuteAsync(request);

        if (!response.IsSuccessful)
        {
            var errorMessage = $"Erro ao consultar PIX. Código: {response.StatusCode}, Detalhes: {response.Content}";
            throw new Exception(errorMessage);
        }

        return response.Content;
    }

    public async Task<string> ConsultaPixPorE2EId(string e2eId)
    {
        var baseUrl = _configuration["Gerencianet:BaseUrl"];
        var certPath = _configuration["Gerencianet:CertificatePath"];

        var clientOptions = new RestClientOptions(baseUrl)
        {
            ConfigureMessageHandler = handler =>
            {
                var httpHandler = new HttpClientHandler();
                httpHandler.ClientCertificates.Add(new X509Certificate2(certPath));
                return httpHandler;
            }
        };

        using var client = new RestClient(clientOptions);
        string accessToken = await GetAccessToken(client);

        // Criando a requisição
        var request = new RestRequest($"/v2/pix/{e2eId}", Method.Get);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddHeader("Content-Type", "application/json");

        // Executa a requisição
        var response = await client.ExecuteAsync(request);

        if (!response.IsSuccessful)
        {
            var errorMessage = $"Erro ao consultar PIX por E2E ID. Código: {response.StatusCode}, Detalhes: {response.Content}";
            throw new Exception(errorMessage);
        }

        return response.Content;
    }

    public async Task<string> ConsultaPixPorTxId(string txid)
    {
        var baseUrl = _configuration["Gerencianet:BaseUrl"];
        var certPath = _configuration["Gerencianet:CertificatePath"];

        var clientOptions = new RestClientOptions(baseUrl)
        {
            ConfigureMessageHandler = handler =>
            {
                var httpHandler = new HttpClientHandler();
                httpHandler.ClientCertificates.Add(new X509Certificate2(certPath));
                return httpHandler;
            }
        };

        using var client = new RestClient(clientOptions);
        string accessToken = await GetAccessToken(client);

        // Criando a requisição
        var request = new RestRequest($"/v2/cob/{txid}", Method.Get);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddHeader("Content-Type", "application/json");

        // Executa a requisição
        var response = await client.ExecuteAsync(request);

        if (!response.IsSuccessful)
        {
            var errorMessage = $"Erro ao consultar PIX por TX ID. Código: {response.StatusCode}, Detalhes: {response.Content}";
            throw new Exception(errorMessage);
        }

        return response.Content;
    }

    public async Task<Pagamento> ValidarPagamento(Pagamento payment)
    {
        await ConsultaPixPorTxId(payment.Txid);
        var existingPayment = await _context.Pagamentos.FirstOrDefaultAsync(p => p.Txid == payment.Txid);

        if (existingPayment != null)
        {
            throw new Exception("Já existe um pagamento com este TXID.");
        }

        return payment;
    }

    public async Task<string> ConsultaLocationPorLocId(string locId)
    {
        var baseUrl = _configuration["Gerencianet:BaseUrl"];
        var certPath = _configuration["Gerencianet:CertificatePath"];

        var clientOptions = new RestClientOptions(baseUrl)
        {
            ConfigureMessageHandler = handler =>
            {
                var httpHandler = new HttpClientHandler();
                httpHandler.ClientCertificates.Add(new X509Certificate2(certPath));
                return httpHandler;
            }
        };

        using var client = new RestClient(clientOptions);
        string accessToken = await GetAccessToken(client);

        // Criando a requisição
        var request = new RestRequest($"/v2/loc/{locId}", Method.Get);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddHeader("Content-Type", "application/json");

        // Executa a requisição
        var response = await client.ExecuteAsync(request);

        if (!response.IsSuccessful)
        {
            var errorMessage = $"Erro ao consultar location por LOC ID. Código: {response.StatusCode}, Detalhes: {response.Content}";
            throw new Exception(errorMessage);
        }

        return response.Content;
    }
}
