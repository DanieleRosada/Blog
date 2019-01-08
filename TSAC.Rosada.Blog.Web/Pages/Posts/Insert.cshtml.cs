using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TSAC.Rosada.Blog.Data;
using TSAC.Rosada.Blog.Data.Models;
using System.IO;
using TSAC.Rosada.Blog.Web.Function;

namespace TSAC.Rosada.Blog.Web.Pages.Posts
{
    [Authorize]
    public class InsertModel : PageModel
    {
        public IDataAccess _data { get; set; }
        public Common _azure { get; set; }

        private readonly UserManager<IdentityUser> _userManager;

        public InsertModel(IDataAccess dataAccess, UserManager<IdentityUser> userManager, Common common)
        {
            _data = dataAccess;
            _azure = common;
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

            [Display(Name = "Image")]
            [DataType(DataType.Text)]
            public IFormFile Image { get; set; }

        }

        [BindProperty]
        public NewPost Post { get; set; }

        public async Task<IActionResult> OnPost(string value)
        {
            if (ModelState.IsValid)
            {
                string filename = null;
                var userId = _userManager.GetUserId(User);
                DateTime? PublishedDate = null;
                
                //insert photo on directory and azure
                if (Post.Image != null)
                {
                    filename = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Post.Image.FileName}";
                    await _azure.InsertPhoto(Post.Image, filename);
                    filename = "https://itsrosada.blob.core.windows.net/image/" + filename;
                }

                //insert post on db
                if (value == "Public")
                {
                    PublishedDate = DateTime.Now;
                }
                _data.InsertPost(new Post
                {
                    Title = Post.Title,
                    Content = Post.Content,
                    UserInsert = userId,
                    PublishedDate = PublishedDate,
                    Image = filename
                });
                
                return RedirectToPage("/index");
            }
            return Page();
        }
    }
}