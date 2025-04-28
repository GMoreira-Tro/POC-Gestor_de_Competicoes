using Microsoft.AspNetCore.Mvc;

namespace CRUDAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeoNamesController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _username = "Guiru"; // Seu username do GeoNames

        public GeoNamesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("countries")]
        public async Task<IActionResult> GetCountries()
        {
            var url = $"http://api.geonames.org/countryInfoJSON?username={_username}";
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        [HttpGet("states/{geonameId}")]
        public async Task<IActionResult> GetStates(string geonameId)
        {
            var url = $"http://api.geonames.org/childrenJSON?geonameId={geonameId}&username={_username}&maxRows=10000";
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        [HttpGet("cities/{geonameId}")]
        public async Task<IActionResult> GetCities(string geonameId)
        {
            var url = $"http://api.geonames.org/childrenJSON?geonameId={geonameId}&username={_username}&maxRows=10000";
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }
    }
}
