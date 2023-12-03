using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Cloud_computing_project_LAST.Models
{
    public class HebcalService
    {
        private readonly HttpClient _httpClient;

        public HebcalService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5229/")
            };

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> HebcalRoot()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/Hebcal");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return $"Error: {response.StatusCode}";
            }
        }
    }
}