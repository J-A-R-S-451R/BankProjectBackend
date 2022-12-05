using System.Text.Json.Serialization;

namespace FundraiserAPI.Domain
{
    public class Donation
    {
        public int? Id { get; set; }
        public decimal Amount { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PaymentType { get; set; } = null!;
        public string? CreditCardNumber { get; set; }
        public string? Cvv { get; set; }
        public string? BankAccountNumber { get; set; }
        public string AddressCountry { get; set; } = null!;
        public string AddressState { get; set; } = null!;
        public string AddressCity { get; set; } = null!;
        public string AddressStreet1 { get; set; } = null!;
        public string? AddressStreet2 { get; set; }
        public string AddressZip { get; set; } = null!;
        public int FundraiserId { get; set; }
        public string? FundraiserName { get; set; }
        public string Note { get; set; } = null!;
        public DateTime Date { get; set; }
    }

}