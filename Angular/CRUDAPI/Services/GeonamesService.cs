using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CRUDAPI.Services
{
    public class GeoNamesService
    {
        private const string GeoNamesBaseUrl = "http://api.geonames.org";

        private readonly HttpClient _httpClient;

        public GeoNamesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> EstadoPertenceAoPais(string pais, string estado)
        {
            int paisId = await GetGeonameIdByCountryName(pais);
            var url = $"{GeoNamesBaseUrl}/childrenJSON?geonameId={paisId}&username=Guiru";
            Console.WriteLine(url);

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

        public async Task<bool> CidadePertenceAoPaisEEstado(string pais, string estado, string cidade)
        {
            int estadoId = await GetGeonameIdByCountryAndStateName(pais, estado);
            var url = $"{GeoNamesBaseUrl}/childrenJSON?geonameId={estadoId}&username=Guiru";

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

        private async Task<int> GetGeonameIdByCountryName(string countryName)
        {
            string countryCode = await GetCountryCode(countryName);
            // Construa a URL para consultar informações sobre o país pelo nome
            string url = $"{GeoNamesBaseUrl}/countryInfoJSON?formatted=true&country={countryCode}&username=Guiru";
            Console.Clear();
            Console.WriteLine(url);

            // Faça a chamada HTTP GET para a API do GeoNames
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            // Verifique se a solicitação foi bem-sucedida
            if (response.IsSuccessStatusCode)
            {
                // Leia a resposta como uma string
                string json = await response.Content.ReadAsStringAsync();
                Console.WriteLine(json);

                // Analise o JSON para obter o geonameId do país
                // Este é um exemplo hipotético, você precisa ajustar de acordo com a estrutura real da resposta JSON
                dynamic countryInfo = JsonSerializer.Deserialize<dynamic>(json);
                int geonameId = countryInfo.geonameId;

                return geonameId;
            }
            else
            {
                // Se a solicitação falhar, lance uma exceção ou lide com o erro de outra forma
                throw new Exception($"Falha ao obter informações do país {countryName}. Código de status: {response.StatusCode}");
            }
        }

        public async Task<int> GetGeonameIdByCountryAndStateName(string countryName, string stateName)
        {
            // Construa a URL para consultar informações sobre o estado pelo nome
            string countryCode = await GetCountryCode(countryName);
            string url = $"{GeoNamesBaseUrl}/searchJSON?formatted=true&name={stateName}&country={countryCode}&featureCode=ADM1&maxRows=1&username=Guiru";
            Console.Clear();
            Console.WriteLine(url);

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

        public async Task<string> GetCountryCode(string countryName)
        {
            try
            {
                string apiUrl = $"https://restcountries.com/v3.1/name/{countryName}";
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JArray countries = JArray.Parse(jsonResponse);

                    // Verifica se algum país foi retornado
                    if (countries.Count > 0)
                    {
                        // Retorna o código de país do primeiro país retornado
                        return countries[0]["cca2"].ToString();
                    }
                    else
                    {
                        throw new Exception("Country not found.");
                    }
                }
                else
                {
                    throw new Exception($"Failed to retrieve country information. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
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
