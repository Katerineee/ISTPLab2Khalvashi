using System.Collections.Generic;

namespace LAB2ISTPP.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public User()
        {
            Comments = new List<Comment>();
        }
    }
}
