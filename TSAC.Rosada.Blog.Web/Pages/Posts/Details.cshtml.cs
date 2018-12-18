using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TSAC.Rosada.Blog.Data;
using TSAC.Rosada.Blog.Data.Models;

namespace TSAC.Rosada.Blog.Web.Pages.Posts
{
    public class DetailsModel : PageModel
    {
        public IDataAccess _data { get; set; }
        public Post Post { get; set; }
        public IEnumerable<Comment> Comments { get; set; }

        public DetailsModel(IDataAccess dataAccess)
        {
            _data = dataAccess;
        }

        public void OnGet(int id)
        {
            Post = _data.GetPost(id);
            var file = Path.Combine(
                   Directory.GetCurrentDirectory(),
                   "wwwroot", "files", $"{Post.Title}.jpg"
       );
            if (System.IO.File.Exists(file))
                Post.ImageExist = true;
            else
                Post.ImageExist = false;
        }

    }
}