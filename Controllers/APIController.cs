using eventsBook.Models;
using eventsBook.Models.HelperModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eventsBook.Controllers
{
    [Route("/api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] loginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = JWTHelper.GenerateJwtToken(user.Id, user.Email, "^p0!Qv@8L#x*Yf3WzA6&N$kRbT1uJg9e");
                return Ok(new { token });
            }
            return Unauthorized();
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
            if (ev != null)
                return Ok(ev);
            return NotFound();
        }
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            string username = User.Identity.Name!;
            User? currentUser = await db.Users
                .Include(u => u.Events)
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (currentUser == null)
                return Unauthorized();

            var cardsDetails = tasks.GetEvents(currentUser, page, pageSize,out double PagesCount);

            return Ok(new { data = cardsDetails, PagesCount=PagesCount });
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
        public async Task<IActionResult> MyEvents(int page = 1, int pageSize = 10)
        {
            string username = User.Identity.Name!;
            User? currentUser = await db.Users
                .Include(u => u.Events)
                .FirstOrDefaultAsync(u => u.UserName == username);
            if (currentUser == null)
                return Unauthorized();
            return Ok(tasks.GetMyEvents(currentUser,page,pageSize,db));
        }
    }
}
