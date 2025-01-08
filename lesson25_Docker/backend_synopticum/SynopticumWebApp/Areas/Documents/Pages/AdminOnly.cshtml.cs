using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SynopticumWebApp.Areas.Documents
{
    [Authorize(Roles = "Administrator")]
    public class AdminOnlyModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
