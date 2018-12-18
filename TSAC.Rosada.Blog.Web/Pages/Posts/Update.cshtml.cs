using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TSAC.Rosada.Blog.Data;
using TSAC.Rosada.Blog.Data.Models;

namespace TSAC.Rosada.Blog.Web.Pages.Posts
{
    [Authorize]
    public class UpdateModel : PageModel
    {
        public IEnumerable<Post> Posts { get; set; }

        [BindProperty]
        public Post PostUpdate { get; set; }
        public IDataAccess _data { get; set; }
        private readonly UserManager<IdentityUser> _userManager;

        public UpdateModel(IDataAccess dataAccess, UserManager<IdentityUser> userManager)
        {
            _data = dataAccess;
            _userManager = userManager;
        }

        public class UpdatePost
        {
            public IFormFile Image { get; set; }
        }

        [BindProperty]
        public UpdatePost Post { get; set; }

        public async Task<IActionResult> OnPost(string value, int id)
        {
            
            if (ModelState.IsValid)
            {
                var file = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot", "files", $"{PostUpdate.Title}.jpg"
                );

                if (Post.Image != null)
                {
                    using (var stream = new FileStream(file, FileMode.Create))
                    {
                        await Post.Image.CopyToAsync(stream);
                    }
                }

                var userId = _userManager.GetUserId(User);
                DateTime? PublishedDate = null;
                if (value == "Public")
                {
                    PublishedDate = DateTime.Now;
                }
                _data.UpdatePost(new Post
                {
                    Id = id,
                    Title = PostUpdate.Title,
                    Content = PostUpdate.Content,
                    UserUpdate = userId,
                    DataUpdate = DateTime.Now,
                    PublishedDate = PublishedDate
                });
                return RedirectToPage("/index");
            }
            return Page();
        }

        public void OnGet(int id)
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
            PostUpdate = _data.GetPost(id);
        }
    }
}