using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TSAC.Rosada.Blog.Data;
using TSAC.Rosada.Blog.Data.Models;

namespace TSAC.Rosada.Blog.Web.Pages.Posts
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public IDataAccess _data { get; set; }
        private readonly UserManager<IdentityUser> _userManager;

        public DeleteModel(IDataAccess dataAccess, UserManager<IdentityUser> userManager)
        {
            _data = dataAccess;
            _userManager = userManager;
        }



        public IActionResult OnPost(int Id)
        {
            _data.DeletePost(Id);
            return RedirectToPage("/index");
        }

        public void OnGet()
        {
            var userId = _userManager.GetUserId(User);
            var list = _data.GetOwnPost(userId);
            foreach (var post in list)
            {
                var item = post;
                var file = Path.Combine(
                       Directory.GetCurrentDirectory(),
                       "wwwroot", "files", $"{post.Title}.jpg"
           );
                if (System.IO.File.Exists(file))
                    item.ImageExist = true;
                else
                    item.ImageExist = false;
            }
            Posts = list;
        }
    }
}