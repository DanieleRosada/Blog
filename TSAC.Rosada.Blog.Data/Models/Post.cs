using System;
using System.Collections.Generic;
using System.Text;

namespace TSAC.Rosada.Blog.Data.Models
{
     public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime Datainsert { get; set; }

        public string UserInsert { get; set; }

        public DateTime DataUpdate { get; set; }

        public string UserUpdate { get; set; }

        public DateTime? PublishedDate { get; set; }

        public string Image { get; set; }
    }
}
