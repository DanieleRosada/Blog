using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TSAC.Rosada.Blog.Data;
using TSAC.Rosada.Blog.Data.Models;

namespace TSAC.Rosada.Blog.Web.Controllers.api
{
    [Route("api/post/")]
    [ApiController]
    public class CommentsController : Controller
    {
        public IDataAccess _data { get; set; }
        private readonly UserManager<IdentityUser> _userManager;
        public CommentsController(IDataAccess dataAccess, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _data = dataAccess;
            _userManager = userManager;
        }

        [HttpGet("{id}/comment")]
        public IEnumerable<Comment> Get(int id) {
            return _data.GetComments(id);
        }

        [HttpPost("insert/comment")]
        public void Post(Comment comment) 
        {
            var Comment = comment.CommentText;
            var Username = User.Identity.Name;
            var Email = User.Identity.Name;
            var PostId = comment.Id;
            _data.InsertComment(new Comment {
                PostId = PostId,
                Author = Username,
                Email = Email,
                CommentText = Comment
            });
    
        }

    }
}