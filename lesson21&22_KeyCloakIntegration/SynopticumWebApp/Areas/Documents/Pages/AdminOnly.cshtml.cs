using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SynopticumWebApp.Areas.Documents
{
    [Authorize(Roles = "Admin")]
    public class AdminOnlyModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
