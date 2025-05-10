using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using eventsBook.Models;
using Microsoft.EntityFrameworkCore;
using eventsBook.Models.HelperModels;
using Microsoft.AspNetCore.Identity;

namespace eventsBook.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private AppDbContext db;
    private TaskHandler tasks;
    private readonly UserManager<User> userManager;
    public HomeController(ILogger<HomeController> logger, AppDbContext db, UserManager<User> userManager)
    {
        _logger = logger;
        this.db = db;
        tasks = new(db);
        this.userManager = userManager;
    }
    [HttpGet("details")]
    public async Task<IActionResult> Details(int i)
    {
        string username = User.Identity.Name!;
        User? currentUser = await db.Users
            .Include(u => u.Events)
            .FirstOrDefaultAsync(u => u.UserName == username);

        if (currentUser == null)
            return Forbid();

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
            return Forbid();

        var user = await userManager.GetUserAsync(User);
        if (await userManager.IsInRoleAsync(user, "Admin"))
            ViewData["isAdmin"] = true;
        else
            ViewData["isAdmin"] = false;

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
        return isDone ? View() : Forbid();
    }
    // [HttpGet("MyEvents")]
    // public async Task<IActionResult> MyEvents()
    // {
    //     string username = User.Identity.Name!;
    //     User? currentUser = await db.Users
    //         .Include(u => u.Events)
    //         .FirstOrDefaultAsync(u => u.UserName == username);
    //     if (currentUser == null)
    //         return Forbid();
    //     var user = await userManager.GetUserAsync(User);
    //     if (await userManager.IsInRoleAsync(user, "Admin"))
    //         ViewData["isAdmin"] = true;
    //     else
    //         ViewData["isAdmin"] = false;
    //     return View("Index", currentUser.Events);
    // }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
