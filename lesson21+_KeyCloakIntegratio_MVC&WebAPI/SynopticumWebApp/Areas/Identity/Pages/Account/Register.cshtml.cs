// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SynopticumWebApp.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        public RegisterModel()
        {}
        public async Task OnGetAsync(string returnUrl = null)
        {
            // redirect
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            return NotFound();
        }
    }
}
