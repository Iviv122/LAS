using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models;

public class LinkContext : DbContext
{
    public LinkContext(DbContextOptions<LinkContext> options)
        : base(options)
    {
    }

    public DbSet<LinkItem> LinkItems { get; set; } = null!;
}