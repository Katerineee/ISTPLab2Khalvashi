using System.Collections.Generic;

namespace LAB2ISTPP.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<News> News { get; set; }

        public Category()
        {
            News = new List<News>();
        }
    }
}
