using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using CRUDAPI.Models;
using Newtonsoft.Json;

namespace CRUDAPI.Services
{
    public class DatasetInjector
    {
        private readonly HttpClient _httpClient;

        public DatasetInjector(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task RunMockScript()
        {
            // Carregar dados dos arquivos JSON
            var usuariosJson = await File.ReadAllTextAsync("mock-usuarios.json");
            var competicoesJson = await File.ReadAllTextAsync("mock-competicoes.json");
            var categoriasJson = await File.ReadAllTextAsync("mock-categorias.json");
            var inscricoesJson = await File.ReadAllTextAsync("mock-inscricoes.json");
            var confrontosJson = await File.ReadAllTextAsync("mock-confrontos.json");
            var confrontoInscricoesJson = await File.ReadAllTextAsync("mock-confrontoInscricoes.json");
            // Adicionar mais arquivos JSON conforme necessário...

            // Converter JSON para objetos C#
            var usuarios = JsonConvert.DeserializeObject<Usuario[]>(usuariosJson);
            var competicoes = JsonConvert.DeserializeObject<Competicao[]>(competicoesJson);
            var categorias = JsonConvert.DeserializeObject<Categoria[]>(categoriasJson);
            var inscricoes = JsonConvert.DeserializeObject<Inscricao[]>(inscricoesJson);
            var confrontos = JsonConvert.DeserializeObject<Confronto[]>(confrontosJson);
            var confrontoInscricoes = JsonConvert.DeserializeObject<ConfrontoInscricao[]>(confrontoInscricoesJson);
            // Converter mais arquivos JSON conforme necessário...

            // Simular solicitações HTTP
            foreach (var usuario in usuarios)
            {
                var response = await _httpClient.PostAsJsonAsync("api/usuario", usuario);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Erro ao criar usuário: {response.ReasonPhrase}");
                }
            }

            foreach (var competicao in competicoes)
            {
                var response = await _httpClient.PostAsJsonAsync("api/competicao", competicao);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Erro ao criar competição: {response.ReasonPhrase}");
                }
            }

           foreach (var categoria in categorias)
            {
                var response = await _httpClient.PostAsJsonAsync("api/categoria", categoria);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Erro ao criar categoria: {response.ReasonPhrase}");
                }
            }

            foreach (var inscricao in inscricoes)
            {
                var response = await _httpClient.PostAsJsonAsync("api/inscricao", inscricao);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Erro ao criar inscricao: {response.ReasonPhrase}");
                }
            }

            foreach (var confronto in confrontos)
            {
                var response = await _httpClient.PostAsJsonAsync("api/confronto", confronto);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Erro ao criar confronto: {response.ReasonPhrase}");
                }
            }

            foreach (var confrontoInscricao in confrontoInscricoes)
            {
                var response = await _httpClient.PostAsJsonAsync("api/confrontoInscricao", confrontoInscricao);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Erro ao criar confrontoInscricao: {response.ReasonPhrase}");
                }
            }
        }
    }
}
