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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("server=localhost;user=Synopticum;password=123123;database=synopticum",
            ServerVersion.Parse("8.0.39-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_bin")
            .HasCharSet("utf8mb3");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
