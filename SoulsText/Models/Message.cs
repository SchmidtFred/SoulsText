using System.Collections.Generic;

namespace SoulsText.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
        public List<Vote> Votes { get; set; }
        public int VoteCount { get; set; }
    }
}
