using System;

namespace VoteApp.Domain.Entities
{
    public partial class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int Vote { get; set; }
        public int CreateBy { get; set; }
        public User User { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
