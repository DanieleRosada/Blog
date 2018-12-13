using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class InsertModel : PageModel
    {
        public IDataAccess _data { get; set; }
        private readonly UserManager<IdentityUser> _userManager;

        public InsertModel(IDataAccess dataAccess, UserManager<IdentityUser> userManager)
        {
            _data = dataAccess;
            _userManager = userManager;
        }


        public class NewPost
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
        public NewPost Post { get; set; }

        public IActionResult OnPost(string value)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                DateTime? PublishedDate =  null;
                if (value == "Public")
                {
                    PublishedDate = DateTime.Now;
                }

                _data.InsertPost(new Post
                {
                    Title = Post.Title,
                    Content = Post.Content,
                    UserInsert = userId,
                    PublishedDate = PublishedDate
                });
                return RedirectToPage("/index");
            }
            return Page();
        }
    }
}