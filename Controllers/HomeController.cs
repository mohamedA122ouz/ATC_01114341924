using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using eventsBook.Models;
using Microsoft.EntityFrameworkCore;

namespace eventsBook.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private AppDbContext db;
    public HomeController(ILogger<HomeController> logger, AppDbContext db)
    {
        _logger = logger;
        this.db = db;
    }

    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
    {
        string username = User.Identity.Name!;
        User? currentUser = await db.Users
            .Include(u => u.Events)
            .FirstOrDefaultAsync(u => u.UserName == username);

        if (currentUser == null)
            return Unauthorized();

        var totalItems = await db.Events.CountAsync();
        var userRegisteredEventIds = currentUser.Events.Select(e => e.Id).ToHashSet();

        List<EventCardDetails> cardsDetails = await db.Events.Include(ev => ev.Images)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(ev => new EventCardDetails
            {
                Event = ev,
                isRegister = userRegisteredEventIds.Contains(ev.Id)
            })
            .ToListAsync();

        return View(cardsDetails);
    }
    [HttpGet("ThankYou")]
    public async Task<IActionResult> ThankYou(int evId)
    {
        Events ev = db.Events.FirstOrDefault(e => e.Id == evId)!;
        string username = User.Identity.Name!;
        User? currentUser = await db.Users
            .Include(u => u.Events)
            .FirstOrDefaultAsync(u => u.UserName == username);
        currentUser.Events.Add(ev);
        db.Users.Update(currentUser);
        db.SaveChanges();
        _logger.LogInformation(ev.Name);
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
