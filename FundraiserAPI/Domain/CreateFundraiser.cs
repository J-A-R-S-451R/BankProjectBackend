using System.Text.Json.Serialization;

namespace FundraiserAPI.Domain
{
    public class CreateFundraiser
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Goal { get; set; }
        public string ImageUrl { get; set; }
    }
}
