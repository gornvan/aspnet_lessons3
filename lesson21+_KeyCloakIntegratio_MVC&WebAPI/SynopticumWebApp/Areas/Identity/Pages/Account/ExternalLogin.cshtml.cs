// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Azure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SynopticumCore.Contract.Enums;
using SynopticumWebApp.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Principal;

namespace SynopticumWebApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<SynopticumUser> _signInManager;
        private readonly UserManager<SynopticumUser> _userManager;
        private readonly IUserStore<SynopticumUser> _userStore;
        private readonly ILogger<ExternalLoginModel> _logger;

        public ExternalLoginModel(
            SignInManager<SynopticumUser> signInManager,
            UserManager<SynopticumUser> userManager,
            IUserStore<SynopticumUser> userStore,
            ILogger<ExternalLoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _logger = logger;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ProviderDisplayName { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
        
        public IActionResult OnGet() => RedirectToPage("./Login");

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            var chalRes = new ChallengeResult(provider, properties);
            return chalRes;
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if(User.Identity?.IsAuthenticated ?? false)
            {
                var ourUser = await CreateUserIfNotKnown(User);
                await SignTheUserIn(ourUser);
                return LocalRedirect(returnUrl);
            }

            ErrorMessage = "Failed authenticating with external provider.";
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }

        private async Task<SynopticumUser> CreateUserIfNotKnown(ClaimsPrincipal principal)
        {
            var provider = GetAuthProvider(principal.Identity);
            var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            // in case of tight integration with a certain Identity Provider,
            // we would pull Roles from the Principal Claims and store them in the User to put into DB
            // also we would need to make sure the Claims are in sync

            // if exists, return it
            var existingUser = await _userManager.FindByEmailAsync(email);
            if(existingUser != null)
            {
                return existingUser;
            }

            // otherwise - create it
            var newUser = new SynopticumUser
            {
                UserName = email,
                Email = email,
                IdentityProvider = provider
            };

            var identityResult = await _userManager.CreateAsync(newUser);

            return newUser;
        }

        private async Task SignTheUserIn(SynopticumUser user)
        {
            _signInManager.AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            await _signInManager.SignInAsync(
                user,
                isPersistent: false,
                authenticationMethod: CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private IdentityProvider GetAuthProvider(IIdentity identity)
        {
            if(identity.AuthenticationType == "Google")
            {
                return IdentityProvider.Google;
            }

            return IdentityProvider.KeyCloak;
        }
    }
}
