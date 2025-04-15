using System.Text.Json.Serialization;

namespace DaprAspire.FrontEnd.Models.Account
{
    /// <summary>
    /// Represents the response from the login endpoint.
    /// </summary>
    public class LoginResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
