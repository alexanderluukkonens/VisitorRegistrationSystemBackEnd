using Microsoft.EntityFrameworkCore;

public class VisitorDbContext : DbContext
{
    public VisitorDbContext(DbContextOptions<VisitorDbContext> options)
        : base(options) { }

    public DbSet<Visitor> Visitors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Visitor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(30).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(50).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
    }
}
