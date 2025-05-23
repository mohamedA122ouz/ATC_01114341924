using Azure;
using Microsoft.EntityFrameworkCore;

namespace eventsBook.Models.HelperModels;

public class TaskHandler
{
    AppDbContext db;
    public TaskHandler(AppDbContext db)
    {
        this.db = db;
    }
    public double PagesCount(int page,int pageSize,int totalItems)
    {
        return Math.Ceiling((double)totalItems / pageSize);
    }
    public List<EventCardDetails> GetEvents(User currentUser, int page, int pageSize, out double count)
    {
        var totalItems = db.Events.Count();
        count = PagesCount(page, pageSize, totalItems);
        var userRegisteredEventIds = currentUser.Events.Select(e => e.Id).ToHashSet();

        List<EventCardDetails> cardsDetails = db.Events.Include(ev => ev.Images).Include(ev => ev.Users)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(ev => new EventCardDetails
            {
                Event = ev,
                isRegister = userRegisteredEventIds.Contains(ev.Id),
            })
            .ToList();
        return cardsDetails;
    }
    public async Task<List<EventCardDetails>> GetMyEvents(User currentUser, int page, int pageSize, AppDbContext db)
    {
        var cardsDetails = currentUser.Events
        .Select(ev =>
        {

            var images = db.Images.Where(img => img.EventId == ev.Id).ToList();
            return new EventCardDetails
            {
                Event = ev,
                isRegister = true
            };
        })
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        return cardsDetails;
    }
    public async Task<EventCardDetails?> GetEvent(User currentUser, int evId)
    {
        try
        {
            var totalItems = await db.Events.CountAsync();
            var userRegisteredEventIds = currentUser.Events.Select(e => e.Id).ToHashSet();
            Events eve = db.Events.Include(ev => ev.Images).Include(ev => ev.Category).FirstOrDefault(ev => ev.Id == evId);
            EventCardDetails cardDetails = new();
            cardDetails.Event = eve;
            cardDetails.isRegister = userRegisteredEventIds.Contains(eve.Id);
            return cardDetails;
        }
        catch (Exception ex)
        {
            return null;
        }

    }
    public async Task<bool> RegisterEvent(User currentUser, int evId)
    {
        Events ev = db.Events.FirstOrDefault(e => e.Id == evId)!;
        if (ev.Date > DateTime.Now)
        {
            currentUser.Events.Add(ev);
            ev.count += 1;
            db.Users.Update(currentUser);
            db.SaveChanges();
            return true;
        }
        return false;
    }
}
