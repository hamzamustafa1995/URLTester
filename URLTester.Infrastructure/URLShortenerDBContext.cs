using Microsoft.EntityFrameworkCore;
using URLTester.Domain.Models;

namespace URLTester.Infrastructure;

public class URLTesterDBContext : DbContext
{
	public URLTesterDBContext(DbContextOptions<URLTesterDBContext> options)
		: base(options)
	{
	}

	public DbSet<URL> URLs { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<URL>().HasKey(x => x.Id);
		// Add other entity configurations if needed
	}
}
