using TSAC.Rosada.Blog.Data;
using TSAC.Rosada.Blog.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baldin.SebEJ.Blog.Web.Hubs
{
    public class CommentHub : Hub
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IDataAccess _data;

        public CommentHub(UserManager<IdentityUser> userManager, IDataAccess dataAccess) : base()
        {
            _userManager = userManager;
            _data = dataAccess;
        }

        public async Task SendMessage(string user, int Post_Id, string message)
        {
            var comment = new Comment
            {
                Email = user,
                InsertDate = DateTime.UtcNow,
                PostId = Post_Id,
                CommentText = message
            };
            var identity = await _userManager.FindByEmailAsync(user);
            comment.Author = identity.Id;
            _data.InsertComment(comment);
            await Clients.Others.SendAsync("ReceiveComments", user, comment.InsertDate.ToString(), message, Post_Id);
        }

        public async Task IsWriting(string email)
        {
            await Clients.Others.SendAsync("UserIsWriting", email);
        }

        public async Task IsNotWriting(string email)
        {
            await Clients.Others.SendAsync("UserIsNotWriting", email);
        }
    }
}
