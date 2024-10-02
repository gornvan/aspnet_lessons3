using Microsoft.EntityFrameworkCore;

namespace SynopticumDAL.Services;

public partial class SynopticumDbContext : DbContext
{
    public SynopticumDbContext()
    {
    }

    public SynopticumDbContext(DbContextOptions<SynopticumDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_bin")
            .HasCharSet("utf8mb3");

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
