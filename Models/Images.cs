using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace eventsBook.Models;

public class Images
{
    public int Id { get; set; }
    public string Url { get; set; }
    
    [ForeignKey("EventId")]
    public int EventId { get; set; }
    public Events Event { get; set; }

}
