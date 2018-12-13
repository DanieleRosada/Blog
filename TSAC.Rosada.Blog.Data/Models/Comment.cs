using System;
using System.Collections.Generic;
using System.Text;

namespace TSAC.Rosada.Blog.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public string Author { get; set; }

        public string Email { get; set; }

        public string CommentText { get; set; } //[Comment] as CommentText in query

        public DateTime InsertDate { get; set; }
    }
}
