using System.Collections.Generic;


namespace SoulsText.ConsoleApp.Models
{
    public class InMemoryData
    {
        public List<UserProfile> Users { get; set; }
        public UserProfile User { get; set; }
        public List<Message> Messages { get; set; }
    }
}
