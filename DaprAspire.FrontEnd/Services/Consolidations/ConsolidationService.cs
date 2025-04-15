using DaprAspire.FrontEnd.Models.Projections;
using System.Net.Http.Json;

namespace DaprAspire.FrontEnd.Services.Consolidations
{
    public class ConsolidationService
    {
        private readonly HttpClient _httpClient;

        public ConsolidationService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("AuthorizedClient");
        }

        public async Task<DailyBalanceResponse?> GetDailyBalanceAsync(string ledgerId)
        {
            var response = await _httpClient.GetAsync($"/consolidation/projections/daily/{ledgerId}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao buscar projeção: {response.StatusCode}");

            return await response.Content.ReadFromJsonAsync<DailyBalanceResponse>();
        }
    }
}
