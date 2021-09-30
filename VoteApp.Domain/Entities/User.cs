using System.Collections.Generic;

namespace VoteApp.Domain.Entities
{
    public partial class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual ICollection<UserVoteDetail> UserVoteDetails { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
