using System.Net;
using System.Text.Json.Serialization;

namespace FundraiserAPI.Domain
{
    public class ErrorResponseData
    {
        [JsonPropertyName("errorMessage")]
        public string? ErrorMessage { get; set; }
        
        [JsonPropertyName("errorCode")]
        public string? ErrorCode { get; set; }

        [JsonIgnore]
        public HttpStatusCode HttpCode { get; set; }
    }
}
