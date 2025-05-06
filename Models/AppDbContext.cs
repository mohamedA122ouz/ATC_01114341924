using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eventsBook.Models;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) :
        base(options)
    { }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Images> Images { get; set; }
    public DbSet<Events> Events { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Events>().HasOne(ev => ev.Category);
        builder.Entity<Events>().HasMany(ev => ev.Images).WithOne(img => img.Event).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<User>().HasMany(user => user.Events).WithMany(ev => ev.Users).UsingEntity(u => u.ToTable("UsersEvents"));
        builder.Entity<Category>().HasData([
            new(){
                Id=1,
                Name="Arts & Culture"
            },
            new(){
                Id=2,
                Name="Entertainment"
            },
            new(){
                Id=3,
                Name="Education"
            },
            new(){
                Id=4,
                Name="Political & Social"
            },
            new(){
                Id=5,
                Name="Business & Networking"
            },
            new(){
                Id=6,
                Name="Tech & Innovation"
            },
            new(){
                Id=7,
                Name="Health & Wellness"
            },
            new(){
                Id=8,
                Name="Recreation"
            }
        ]);
        builder.Entity<Events>().HasData(
            new Events
            {
                Id = 1,
                CategoryId = 6,
                Date = DateTime.Now + TimeSpan.FromDays(20),
                Description = "Join industry leaders, researchers, and developers at the forefront of artificial intelligence innovation. Discover how AI is transforming industries from healthcare to finance, and learn about the latest breakthroughs in machine learning, robotics, and ethics in AI.",
                Venue = "TechHub Convention Center, San Francisco, CA",
                Price = 900,
                Name = "AI Future Summit 2025",
            },
                new Events
                {
                    Id = 2,
                    Name = "Picnic for Families",
                    Description = "Relaxing day in the park with games and food.",
                    Venue = "Green Field",
                    Price = 5,
                    CategoryId = 4
                }
        );


        builder.Entity<Images>().HasData(
    new Images
    {
        Id = 1,
        Url = "/images/tech-expo.jpg",
        EventId = 1
    },
    new Images
    {
        Id = 2,
        Url = "/images/picnic.jpg",
        EventId = 2
    }
);

    }
}