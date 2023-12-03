﻿using System.Net.Http.Headers;

namespace Cloud_computing_project_LAST.Models
{
    public class ImaggaService
    {
        private readonly HttpClient _httpClient;

        public ImaggaService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5229/")
            };

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> CheckImage(string imageUrl, string description)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"Imagga/CheckImage?imageUrl={imageUrl}&description={description}");

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