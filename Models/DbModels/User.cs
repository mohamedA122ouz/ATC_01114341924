using System;
using Microsoft.AspNetCore.Identity;

namespace eventsBook.Models;

public class User : IdentityUser
{
    public List<Events> Events { get; set; } = new();
}
