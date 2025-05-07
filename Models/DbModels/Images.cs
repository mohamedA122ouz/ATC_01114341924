using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace eventsBook.Models;

public class Images
{
    public int Id { get; set; }
    public string Url { get; set; }
    
    [ForeignKey("EventId")]
    public int EventId { get; set; }
    [JsonIgnore]
    public Events Event { get; set; }

}
