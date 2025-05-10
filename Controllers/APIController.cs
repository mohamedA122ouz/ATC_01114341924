using eventsBook.Models;
using eventsBook.Models.HelperModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eventsBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, User")]
    public class APIController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private AppDbContext db;
        private TaskHandler tasks;
        private readonly UserManager<User> userManager;
        public APIController(ILogger<HomeController> logger, AppDbContext db, UserManager<User> userManager)
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
                return Unauthorized();

            var ev = await tasks.GetEvent(currentUser, i);
            return Ok(ev);
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

            return Ok(cardsDetails);
        }
        [HttpGet("Book")]
        public async Task<IActionResult> Book(int evId)
        {
            string username = User.Identity.Name!;
            User? currentUser = await db.Users
                .Include(u => u.Events)
                .FirstOrDefaultAsync(u => u.UserName == username);
            bool isDone = await tasks.RegisterEvent(currentUser, evId);
            return isDone ? Ok("Booked") : BadRequest();
        }
        [HttpGet("MyEvents")]
        public async Task<IActionResult> MyEvents()
        {
            string username = User.Identity.Name!;
            User? currentUser = await db.Users
                .Include(u => u.Events)
                .FirstOrDefaultAsync(u => u.UserName == username);
            if (currentUser == null)
                return Unauthorized();
            return Ok(currentUser.Events);
        }
    }
}
