using DealService.Application.Common.Interfaces;
using DealService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DealService.Infrastructure.Persistence;

public class AppDbContext : DbContext, IApplicationDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Deal> Deals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Deal>().HasKey(d => d.Id);
        modelBuilder.Entity<Deal>().Property(d => d.Title).IsRequired().HasMaxLength(200);
        base.OnModelCreating(modelBuilder);
    }
}