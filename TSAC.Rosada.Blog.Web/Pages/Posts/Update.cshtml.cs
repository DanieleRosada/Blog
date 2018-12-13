using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
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
            [Required]
            [Display(Name = "Title")]
            [DataType(DataType.Text)]
            public string Title { get; set; }

            [Required]
            [Display(Name = "Content")]
            [DataType(DataType.Text)]
            public string Content { get; set; }
        }

        [BindProperty]
        public UpdatePost Post { get; set; }

        public IActionResult OnPost()
        {
            
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                _data.UpdatePost(new Post
                {
                    Id = PostUpdate.Id,
                    Title = Post.Title,
                    Content = Post.Content,
                    UserUpdate = userId,
                    DataUpdate = DateTime.Now
                });
                return RedirectToPage("/index");
            }
            return Page();
        }

        public void OnGet(int id)
        {  
            var userId = _userManager.GetUserId(User);
            Posts = _data.GetOwnPost(userId);
            if (id != null) {
                PostUpdate = _data.GetPost(id);
                
            }
        }
    }
}