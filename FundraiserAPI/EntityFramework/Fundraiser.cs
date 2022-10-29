using System;
using System.Collections.Generic;

namespace FundraiserAPI.EntityFramework
{
    public partial class Fundraiser
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Owner { get; set; }
        public decimal Goal { get; set; }
        public decimal DonationTotal { get; set; }
        public byte[]? Picture { get; set; }
    }
}
