using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace eventsBook.Models;

public class Events
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Venue { get; set; }
    public double Price { get; set; }

    // Foreign key for Category
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    // One-to-many: Event has many images
    public List<Images> Images { get; set; } = new();
    public int count { get; set; } = 0;

    // Many-to-many: Users attending
    [JsonIgnore]
    public List<User> Users { get; set; } = new();
}

