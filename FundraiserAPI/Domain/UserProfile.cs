using System.Text.Json.Serialization;

namespace FundraiserAPI.Domain
{
    public class UserProfile
    {
        public string? Username { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Password { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? AddressCountry { get; set; }

        public string? AddressState { get; set; }
        
        public string? AddressCity { get; set; }
        
        public string? AddressStreet1 { get; set; }
        
        public string? AddressStreet2 { get; set; }
        
        public string? AddressZip { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
