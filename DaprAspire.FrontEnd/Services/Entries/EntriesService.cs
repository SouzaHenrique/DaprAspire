using System.Net.Http.Json;

using DaprAspire.Domain.CrossCutting.DTO.Entries;
using DaprAspire.FrontEnd.Models.Ledgers;

namespace DaprAspire.FrontEnd.Services.Entries
{
    public class EntriesService(IHttpClientFactory factory)
    {
        private readonly HttpClient _httpClient = factory.CreateClient("AuthorizedClient");

        public async Task<IEnumerable<LedgerEntryResponse>> GetAllAsync(int page = 1, int pageSize = 10)
        {
            var response = await _httpClient.GetAsync($"/entries?PageNumber={page}&PageSize={pageSize}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erro ao buscar lançamentos: {response.StatusCode}");
            }

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<LedgerEntryResponse>>();
            return result ?? Enumerable.Empty<LedgerEntryResponse>();
        }

        public async Task<string?> CreateAsync()
        {
            var response = await _httpClient.PostAsync("/entries", null);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao criar lançamento: {response.StatusCode}");

            // Espera-se que o ID esteja no corpo da resposta como { id: "xyz" }
            var result = await response.Content.ReadFromJsonAsync<CreateEntryResponse>();
            return result?.Id;
        }

        public async Task CreditAsync(string entryId, decimal value)
        {
            var request = new ValueRequest { Value = value };
            var response = await _httpClient.PostAsJsonAsync($"/entries/{entryId}/credit", request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao aplicar crédito: {response.StatusCode}");
        }

        public async Task DebitAsync(string entryId, decimal value)
        {
            var request = new ValueRequest { Value = value };
            var response = await _httpClient.PostAsJsonAsync($"/entries/{entryId}/debit", request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao aplicar débito: {response.StatusCode}");
        }
    }
}
