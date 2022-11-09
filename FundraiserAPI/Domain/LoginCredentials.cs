using System.Text.Json.Serialization;

namespace FundraiserAPI.Domain
{
    public class LoginCredentials
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
