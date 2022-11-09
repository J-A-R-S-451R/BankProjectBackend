using System.Text.Json.Serialization;

namespace FundraiserAPI.Domain
{
    public class Fundraiser2
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("owner")]
        public int Owner { get; set; }

        [JsonPropertyName("goal")]
        public decimal Goal { get; set; }

        [JsonPropertyName("donationTotal")]
        public decimal DonationTotal { get; set; }
    }
}
