namespace VoteApp.Domain.Entities
{
    public class UserVoteDetail
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int TotalVote { get; set; }
    }
}
