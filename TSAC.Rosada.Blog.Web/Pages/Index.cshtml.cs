using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TSAC.Rosada.Blog.Data;
using TSAC.Rosada.Blog.Data.Models;


namespace TSAC.Rosada.Blog.Web.Pages
{
    public class IndexModel : PageModel
    {
        public IDataAccess _data { get; set; }

        public IndexModel(IDataAccess dataAccess)
        {
            _data = dataAccess;
        }
        public IEnumerable<Post> Posts { get; set; }

        public void OnGet()
        {
            Posts = _data.GetPosts();
        }
    }
}
