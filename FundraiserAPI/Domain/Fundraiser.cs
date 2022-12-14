using System.Text.Json.Serialization;

namespace FundraiserAPI.Domain
{
    public class Fundraiser
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Owner { get; set; }

        public decimal Goal { get; set; }

        public decimal DonationTotal { get; set; }
    }
}
