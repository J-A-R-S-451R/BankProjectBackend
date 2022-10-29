using System;
using System.Collections.Generic;

namespace FundraiserAPI.EntityFramework
{
    public partial class SessionToken
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string SessionId { get; set; } = null!;
        public DateTime ExpiresOn { get; set; }

        public virtual Login? User { get; set; }
    }
}
