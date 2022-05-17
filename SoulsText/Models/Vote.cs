namespace SoulsText.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public bool Upvote { get; set; }
        public int? UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
        public int MessageId { get; set; }
        public Message Message { get; set; }
    }
}
