using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SynopticumWebApp.Data.Entities;

namespace SynopticumWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<SynopticumUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
