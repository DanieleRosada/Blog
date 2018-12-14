using TSAC.Rosada.Blog.Data;
using TSAC.Rosada.Blog.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSAC.Rosada.Blog.Web.Hubs
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

        public async Task SendMessage(Comment comment)
        {
            await Clients.All.SendAsync("ReceiveComments", comment.Author, comment.InsertDate.ToString(), comment.CommentText, comment.PostId); //Others
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
