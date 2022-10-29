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
        public string? SecurityQuestion { get; set; }
        public string? SecurityQuestionAnswer { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public virtual ICollection<SessionToken> SessionTokens { get; set; }
    }
}
