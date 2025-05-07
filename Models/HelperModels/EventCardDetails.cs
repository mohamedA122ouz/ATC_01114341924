using System;

namespace eventsBook.Models;

public class EventCardDetails
{
    public Events Event { get; set; } = new();
    public bool isRegister { get; set; } = false;
}
