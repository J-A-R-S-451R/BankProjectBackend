using System;
using System.Collections.Generic;

namespace FundraiserAPI.EntityFramework
{
    public partial class Login
    {
        public Login()
        {
            SessionTokens = new HashSet<SessionToken>();
        }

        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? AddressCountry { get; set; }
        public string? AddressState { get; set; }
        public string? AddressCity { get; set; }
        public string? AddressStreet1 { get; set; }
        public string? AddressStreet2 { get; set; }
        public string? AddressZip { get; set; }

        public virtual ICollection<SessionToken> SessionTokens { get; set; }
    }
}
