using Microsoft.EntityFrameworkCore;

namespace SynopticumDAL;

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
        f
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
