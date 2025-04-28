using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CRUDAPI.Services
{
    /// <summary>
    /// Serviço referente a API geonames, que faz a busca e validações de Países, Estados e Cidades.
    /// </summary>
    public class GeoNamesService
    {
        private const string GeoNamesBaseUrl = "https://api.geonames.org";

        private readonly HttpClient _httpClient;

        public GeoNamesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Verifica se um Estado pertence ao País fornecido.
        /// </summary>
        /// <param name="pais">Sigla do País em uppercase.</param>
        /// <param name="estado">Nome do Estado a ser verificado.</param>
        /// <returns></returns>
        public async Task<bool> EstadoPertenceAoPais(string pais, string estado)
        {
            int paisId = await GetGeonameIdByCountryName(pais);
            var url = $"{GeoNamesBaseUrl}/childrenJSON?geonameId={paisId}&username=Guiru";

            // Faz a chamada GET para a API GeoNames
            var response = await _httpClient.GetAsync(url);

            // Verifica se a resposta foi bem-sucedida
            response.EnsureSuccessStatusCode();

            // Converte a resposta para uma lista de estados
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GeoNamesResponse>(json);

            // Extrai os nomes dos estados da resposta
            var estados = result.geonames.Select(g => g.name);

            // Verifica se o estado selecionado está na lista de estados retornada pela API
            return estados.Contains(estado);
        }

        /// <summary>
        /// Verifica se a Cidade pertence ao País e Estado fornecidos.
        /// </summary>
        /// <param name="pais">Sigla do País em uppercase.</param>
        /// <param name="estado">Nome do Estado.</param>
        /// <param name="cidade">Nome da Cidade a ser verificada.</param>
        /// <returns></returns>
        public async Task<bool> CidadePertenceAoPaisEEstado(string pais, string estado, string cidade)
        {
            int estadoId = await GetGeonameIdByCountryAndStateName(pais, estado);
            var url = $"{GeoNamesBaseUrl}/childrenJSON?geonameId={estadoId}&username=Guiru&maxRows=10000";

            // Faz a chamada GET para a API GeoNames
            var response = await _httpClient.GetAsync(url);

            // Verifica se a resposta foi bem-sucedida
            response.EnsureSuccessStatusCode();

            // Converte a resposta para um objeto GeoNamesSearchResponse
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GeoNamesResponse>(json);

            // Extrai os nomes das cidades da resposta
            var cidades = result.geonames.Select(g => g.name);

            // Verifica se a cidade selecionada está na lista de cidades retornada pela API
            return cidades.Contains(cidade);
        }

        /// <summary>
        /// Pega o Id do País pela sua sigla.
        /// </summary>
        /// <param name="countryName">Sigla do País.</param>
        /// <returns>Id do País.</returns>
        /// <exception cref="Exception"></exception>
        private async Task<int> GetGeonameIdByCountryName(string countryName)
        {
            // Construa a URL para consultar informações sobre o país pelo nome
            string url = $"{GeoNamesBaseUrl}/countryInfoJSON?formatted=true&country={countryName}&username=Guiru";

            // Faça a chamada HTTP GET para a API do GeoNames
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            // Verifique se a solicitação foi bem-sucedida
            if (response.IsSuccessStatusCode)
            {
                // Leia a resposta como uma string
                string json = await response.Content.ReadAsStringAsync();

                // Analise o JSON para obter o geonameId do país
                // Este é um exemplo hipotético, você precisa ajustar de acordo com a estrutura real da resposta JSON
                dynamic countryInfo = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                int geonameId = countryInfo.geonames[0].geonameId;
                
                return geonameId;
            }
            else
            {
                // Se a solicitação falhar, lance uma exceção ou lide com o erro de outra forma
                throw new Exception($"Falha ao obter informações do país {countryName}. Código de status: {response.StatusCode}");
            }
        }

        /// <summary>
        /// Pega o Id do Estado pela sigla do País e nome do Estado.
        /// </summary>
        /// <param name="countryName">Sigla do País.</param>
        /// <param name="stateName">Nome do Estado.</param>
        /// <returns>Id do Estado.</returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> GetGeonameIdByCountryAndStateName(string countryName, string stateName)
        {
            // Construa a URL para consultar informações sobre o estado pelo nome
            string url = $"{GeoNamesBaseUrl}/searchJSON?formatted=true&name={stateName}&country={countryName}&featureCode=ADM1&maxRows=1&username=Guiru";

            // Faça a chamada HTTP GET para a API do GeoNames
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            // Verifique se a solicitação foi bem-sucedida
            if (response.IsSuccessStatusCode)
            {
                // Leia a resposta como uma string
                string json = await response.Content.ReadAsStringAsync();

                // Analise o JSON para obter o geonameId do estado
                // Este é um exemplo hipotético, você precisa ajustar de acordo com a estrutura real da resposta JSON
                dynamic searchResults = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                int geonameId = searchResults.geonames[0].geonameId;

                return geonameId;
            }
            else
            {
                // Se a solicitação falhar, lance uma exceção ou lide com o erro de outra forma
                throw new Exception($"Falha ao obter informações do estado {stateName} em {countryName}. Código de status: {response.StatusCode}");
            }
        }
    }

    public class GeoNamesResponse
    {
        public IEnumerable<GeoName> geonames { get; set; }
    }

    public class GeoName
    {
        public string name { get; set; }
    }
}
