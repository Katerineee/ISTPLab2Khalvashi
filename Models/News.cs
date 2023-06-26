using System;
using System.Collections.Generic;

namespace LAB2ISTPP.Models
{
    public class News
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public News()
        {
            Comments = new List<Comment>();
        }
    }
}
