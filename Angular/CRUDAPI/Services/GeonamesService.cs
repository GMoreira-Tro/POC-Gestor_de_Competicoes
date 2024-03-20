using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
            var url = $"{GeoNamesBaseUrl}/childrenJSON?geonameId={pais}&username=Guiru";

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

        public async Task<bool> CidadePertenceAoEstado(string estado, string cidade)
        {
            var url = $"{GeoNamesBaseUrl}/searchJSON?q={cidade}&adminCode1={estado}&username=Guiru";

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
