using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using eventsBook.Models;
using Microsoft.EntityFrameworkCore;
using eventsBook.Models.HelperModels;

namespace eventsBook.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private AppDbContext db;
    private TaskHandler tasks;
    public HomeController(ILogger<HomeController> logger, AppDbContext db)
    {
        _logger = logger;
        this.db = db;
        tasks = new(db);
    }
    [HttpGet("details")]
    public async Task<IActionResult> Details(int i)
    {
        string username = User.Identity.Name!;
        User? currentUser = await db.Users
            .Include(u => u.Events)
            .FirstOrDefaultAsync(u => u.UserName == username);

        if (currentUser == null)
            return Unauthorized();
        
        var ev = await tasks.GetEvent(currentUser, i);
        return View(ev);
    }
    public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
    {
        string username = User.Identity.Name!;
        User? currentUser = await db.Users
            .Include(u => u.Events)
            .FirstOrDefaultAsync(u => u.UserName == username);

        if (currentUser == null)
            return Unauthorized();
        var cardsDetails = await tasks.GetEvents(currentUser, page, pageSize);

        return View(cardsDetails);
    }
    [HttpGet("ThankYou")]
    public async Task<IActionResult> ThankYou(int evId)
    {
        string username = User.Identity.Name!;
        User? currentUser = await db.Users
            .Include(u => u.Events)
            .FirstOrDefaultAsync(u => u.UserName == username);
        bool isDone = await tasks.RegisterEvent(currentUser, evId);
        return isDone?View():RedirectToAction("Error");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
