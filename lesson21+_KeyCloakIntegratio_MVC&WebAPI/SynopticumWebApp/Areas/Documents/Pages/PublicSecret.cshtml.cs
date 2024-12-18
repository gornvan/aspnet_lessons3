using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SynopticumWebApp.Areas.Documents
{
    [Authorize]
    public class PublicSecretModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
