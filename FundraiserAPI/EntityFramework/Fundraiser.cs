using System;
using System.Collections.Generic;

namespace FundraiserAPI.EntityFramework
{
    public partial class Fundraiser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Goal { get; set; }
        public decimal DonationTotal { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}
