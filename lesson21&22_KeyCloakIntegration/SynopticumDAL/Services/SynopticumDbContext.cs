using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SynopticumWebApp.Data.Entities;

namespace SynopticumDAL.Services;

public partial class SynopticumDbContext : IdentityDbContext<SynopticumUser>
{
    public SynopticumDbContext(DbContextOptions<SynopticumDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .UseCollation("utf8mb3_bin")
            .HasCharSet("utf8mb3");

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
