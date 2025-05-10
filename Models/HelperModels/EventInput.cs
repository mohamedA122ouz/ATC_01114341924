using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eventsBook.Models.HelperModels;

public class EventInput
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public double Price { get; set; }

    [Required]
    [Display(Name = "Event Date")]
    public DateTime EventDate { get; set; }

    [Required]
    public int CategoryId { get; set; }

    // public Category Category { get; set; }

    // public List<Images> Images { get; set; }

    [NotMapped]
    public List<IFormFile> ImageFile { get; set; }
    public string Venue { get; set; }
}

