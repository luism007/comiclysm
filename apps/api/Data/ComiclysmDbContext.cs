using Comiclysm.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Comiclysm.Api.Data;

public class ComiclysmDbContext: DbContext
{
    public ComiclysmDbContext (DbContextOptions<ComiclysmDbContext> options) : base(options)
    {}

    public DbSet<Product> Products => Set<Product>();
    public DbSet<PublicationDetails> PublicationDetails => Set<PublicationDetails>();
    public DbSet<ComicSeries> ComicSeries => Set<ComicSeries>();
    public DbSet<ComicIssue> ComicIssues => Set<ComicIssue>();
    public DbSet<Store> Stores => Set<Store>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ComicSeries -> ComicIssue (One-to-Many)
        modelBuilder.Entity<ComicSeries>()
        .HasMany(series => series.Issues)
        .WithOne(issue => issue.ComicSeries)
        .HasForeignKey(issue => issue.ComicSeriesId)
        .OnDelete(DeleteBehavior.Restrict);

        // Product -> PublicationDetails (One-to-One)
        modelBuilder.Entity<PublicationDetails>().HasKey(details => details.ProductId);

        modelBuilder.Entity<Product>()
        .HasOne(product => product.PublicationDetails)
        .WithOne(details => details.Product)
        .HasForeignKey<PublicationDetails>(details => details.ProductId);


        // ComicIssue -> PublicationDetails (One-to-Many)
        modelBuilder.Entity<PublicationDetails>()
        .HasOne(details => details.ComicIssue)
        .WithMany()
        .HasForeignKey(details => details.ComicIssueId)
        .OnDelete(DeleteBehavior.Restrict);

        // Store -> InventoryItem (One-to-Many)
        modelBuilder.Entity<InventoryItem>()
        .HasOne(item => item.Store)
        .WithMany()
        .HasForeignKey(item => item.StoreId)
        .OnDelete(DeleteBehavior.Restrict);

        // Product -> InventoryItem (One-to-Many)
        modelBuilder.Entity<InventoryItem>()
        .HasOne(item => item.Product)
        .WithMany()
        .HasForeignKey(item => item.ProductId)
        .OnDelete(DeleteBehavior.Restrict);

        // Unique storefront slug
        modelBuilder.Entity<Store>()
        .HasIndex(store => store.Slug)
        .IsUnique();

        // One Inventory Record 
        modelBuilder.Entity<InventoryItem>()
        .HasIndex(item => new
        {
            item.StoreId,
            item.ProductId
        })
        .IsUnique();
    }
}