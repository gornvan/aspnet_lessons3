using Microsoft.AspNetCore.Identity;
using SynopticumCore.Contract.Enums;

namespace SynopticumWebApp.Data.Entities
{
    public class SynopticumUser: IdentityUser
    {
        public IdentityProvider IdentityProvider { get; set; }
    }
}
