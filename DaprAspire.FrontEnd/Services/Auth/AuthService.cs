using Microsoft.AspNetCore.Components;

using Blazored.LocalStorage;
using DaprAspire.Domain.CrossCutting.DTO;
using DaprAspire.FrontEnd.Models.Account;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace DaprAspire.FrontEnd.Services.Auth
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        private readonly JwtAuthenticationStateProvider JwtAuthenticationStateProvider;

        public AuthService(IHttpClientFactory factory, ILocalStorageService localStorage, AuthenticationStateProvider jwtAuthenticationStateProvider)
        {
            _http = factory.CreateClient("PublicClient");
            _localStorage = localStorage;
            JwtAuthenticationStateProvider = (JwtAuthenticationStateProvider)jwtAuthenticationStateProvider;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            HttpResponseMessage response = await _http.PostAsJsonAsync("identity/api/Account/login", new LoginRequest(username, password));

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            LoginResponse? result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            await _localStorage.SetItemAsync("authToken", result?.AccessToken);

            var token = result?.AccessToken.Replace("\"", string.Empty);

            if(!string.IsNullOrWhiteSpace(token))
            {
                JwtAuthenticationStateProvider.NotifyUserAuthentication(token);
            }            

            return true;
        }
    }
}
