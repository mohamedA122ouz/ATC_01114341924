using eventsBook.Models;
using eventsBook.Models.HelperModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eventsBook.Controllers
{
    [Route("Admin")]
    // [Authorize("admin")]
    public class AdminController : Controller
    {
        private AppDbContext db;
        public AdminController(AppDbContext db)
        {
            this.db = db;
        }
        // GET: AdminController
        public ActionResult Index()
        {
            List<Category> catigories = db.Categories.ToList();
            ViewData["catigories"] = catigories;
            return View();
        }
        [HttpGet("Edit")]
        public ActionResult Edit(int evId)
        {
            if (evId == 0)
                return RedirectToAction("index", "Home");
            Events ev = db.Events.Include(ev => ev.Images).Include(ev => ev.Category).FirstOrDefault(ev => ev.Id == evId)!;
            List<Category> catigories = db.Categories.ToList();
            ViewData["catigories"] = catigories;
            return View(ev);
        }
        [HttpPost("Edit")]
        public ActionResult Edit([FromForm] EventInput input)
        {
            return View("~/Home/ThankYou");
        }
        [HttpPost("Create")]
        public ActionResult create([FromForm] EventInput input)
        {
            return View("ThankYou");
        }
    }
}
