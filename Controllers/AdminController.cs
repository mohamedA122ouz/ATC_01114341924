using eventsBook.Models;
using eventsBook.Models.HelperModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eventsBook.Controllers
{
    [Route("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private AppDbContext db;
        private AdminTaskHandler adminTask;
        public AdminController(AppDbContext db, ILogger<AdminController> logger, IWebHostEnvironment env)
        {
            adminTask = new(db, env, logger);
            this.db = db;
        }
        // GET: AdminController
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }
        [HttpGet("Create")]
        public ActionResult Create()
        {
            List<Category> catigories = db.Categories.ToList();
            ViewData["catigories"] = catigories;
            return View("Index");
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
        [ValidateAntiForgeryToken]
        public ActionResult Edit([FromForm] EventInput input)
        {
            if(adminTask.Edit(input))
                TempData["SuccessMessage"] = "Event Edited successfully!";
            return RedirectToAction("Index");
        }
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult create([FromForm] EventInput input)
        {
            if (adminTask.createEvent(input)) {
                TempData["SuccessMessage"] = "Event Created successfully!";
                return RedirectToAction("Index");
            }
            else
                return View("Error");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Find the event by id
            var eventToDelete = db.Events.Include(ev=>ev.Images).FirstOrDefault(ev=>ev.Id == id);

            // If the event doesn't exist, return NotFound
            if (eventToDelete == null)
            {
                return NotFound();
            }
            foreach (var img in eventToDelete.Images)
            {
                if (!adminTask.removeImages(img.Url.Substring(1)))
                {
                    TempData["SuccessMessage"] = "Faild to delete the Event!";
                    return RedirectToAction("Index");
                }
            }
            // Remove the event from the database
            db.Events.Remove(eventToDelete);
            await db.SaveChangesAsync();

            // Optionally, add a success message or redirect
            TempData["SuccessMessage"] = "Event deleted successfully!";

            // Redirect back to the events index or another appropriate page
            return RedirectToAction("Index");
        }

    }
}
