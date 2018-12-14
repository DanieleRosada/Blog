using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TSAC.Rosada.Blog.Data;
using TSAC.Rosada.Blog.Data.Models;
using TSAC.Rosada.Blog.Web.Hubs;

namespace TSAC.Rosada.Blog.Web.Controllers.api
{
    [Route("api/post/")]
    [ApiController]
    public class CommentsController : Controller
    {
        public IDataAccess _data { get; set; }
        private readonly UserManager<IdentityUser> _userManager;
        public CommentHub _hubs;
        public CommentsController(IDataAccess dataAccess, UserManager<IdentityUser> userManager)
        {
            _data = dataAccess;
            _userManager = userManager;
        }

        [HttpGet("{id}/comment")]
        public IEnumerable<Comment> Get(int id) {
            return _data.GetComments(id);
        }

        [HttpPost("insert/comment")]
        async public void Post(Comment comment) 
        {
            var newComment = new Comment
            {
                PostId = comment.Id,
                Author = User.Identity.Name,
                Email = User.Identity.Name,
                CommentText = comment.CommentText
            };
            _data.InsertComment(newComment);
            await _hubs.SendMessage(newComment);
        }

    }
}