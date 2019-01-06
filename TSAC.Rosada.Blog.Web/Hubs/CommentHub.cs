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
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task IsWriting(string Author)
        {
            await Clients.Others.SendAsync("UserIsWriting", Author);
        }

        public async Task IsNotWriting(string Author)
        {
            await Clients.Others.SendAsync("UserIsNotWriting", Author);
        }
    }
}
