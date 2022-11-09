using System;
using System.Collections.Generic;

namespace FundraiserAPI.EntityFramework
{
    public partial class Payment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CardType { get; set; } = null!;
        public int CardNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int SecurityCode { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Zipcode { get; set; } = null!;
        public string BillingName { get; set; } = null!;
    }
}
