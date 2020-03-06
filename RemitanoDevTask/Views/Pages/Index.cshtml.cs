using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RemitanoDevTask.Views.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            ViewData["tekOnGet"] = "nOnGet";
        }

        public int OnPost ()
        {
            return 0;
        }
    }
}
