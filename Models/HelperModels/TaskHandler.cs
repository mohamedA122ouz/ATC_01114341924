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
    public async Task<List<EventCardDetails>> GetEvents(User currentUser, int page, int pageSize)
    {
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
        return cardsDetails;
    }
    public async Task<EventCardDetails> GetEvent(User currentUser, int evId)
    {
        var totalItems = await db.Events.CountAsync();
        var userRegisteredEventIds = currentUser.Events.Select(e => e.Id).ToHashSet();
        Events eve = db.Events.Include(ev => ev.Images).Include(ev=>ev.Category).FirstOrDefault(ev => ev.Id == evId);
        EventCardDetails cardDetails = new();
        cardDetails.Event = eve;
        cardDetails.isRegister = userRegisteredEventIds.Contains(eve.Id);
        return cardDetails;

    }
    public async Task<bool> RegisterEvent(User currentUser, int evId)
    {
        Events ev = db.Events.FirstOrDefault(e => e.Id == evId)!;
        if (ev.Date > DateTime.Now)
        {
            currentUser.Events.Add(ev);
            db.Users.Update(currentUser);
            db.SaveChanges();
            return true;
        }
        return false;
    }
}
