using Microsoft.AspNetCore.Components;

using Blazored.LocalStorage;
using DaprAspire.Domain.CrossCutting.DTO;
using DaprAspire.FrontEnd.Models.Account;
using System.Net.Http.Json;

namespace DaprAspire.FrontEnd.Services.Auth
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;

        public AuthService(IHttpClientFactory factory, ILocalStorageService localStorage)
        {
            _http = factory.CreateClient("PublicClient");
            _localStorage = localStorage;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            HttpResponseMessage response = await _http.PostAsJsonAsync("identity/api/Account/login", new LoginRequest(username, password));

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            LoginResponse? result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            var a = response.Content.ReadAsStringAsync();
            await _localStorage.SetItemAsync("authToken", result?.AccessToken);

            return true;
        }
    }
}
