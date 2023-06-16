global using Microsoft.EntityFrameworkCore;

namespace Data;

public class SampleDbContext : DbContext
{
	public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options)
    {
    }

    public DbSet<Person> People => Set<Person>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>(b =>
        {
            b.Property(p => p.Firstname).HasMaxLength(64);
            b.Property(p => p.Lastname).HasMaxLength(64);

            b.HasIndex(p => new { p.Firstname, p.Lastname });
        });
    }
}

public class Person
{
    public int Id { get; set; }

    public required string Firstname { get; set; }

    public required string Lastname { get; set; }
}

