using System;
using System.Collections.Generic;
using System.Text;
using TSAC.Rosada.Blog.Data.Models;

namespace TSAC.Rosada.Blog.Data
{
    public interface IDataAccess
    {
        IEnumerable<Post> GetPosts();

        Post GetPost(int postId);

        IEnumerable<Post> GetOwnPost(string userId);

        int GetPostsCount();

        void InsertPost(Post post);

        void UpdatePost(Post post);

        void DeletePost(int postId);

        IEnumerable<Comment> GetComments(int idPhoto);

        void InsertComment(Comment comment);

        
    }
}
