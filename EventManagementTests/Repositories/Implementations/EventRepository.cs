using EventManagementTests.Data;
using EventManagementTests.DTOs;
using EventManagementTests.Models;
using EventManagementTests.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;



namespace EventManagementTests.Repositories.Implementations
{
    public class EventRepository : IEventRepository
    {

        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Event> GetEventById(int eventId)
        {
            return await _context.Events.FindAsync(eventId);
        }

        public async Task<IEnumerable<Event>> GetAllEvents()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByOrganizerId(int organizerId)
        {
            return await _context.Events.Where(e => e.OrganizerId == organizerId).ToListAsync();
        }

        public async Task AddEvent(EventRequestDTO @event)
        {
            var eventBody = new Event();

            eventBody.Name = @event.Name;
            eventBody.Description = @event.Description;
            eventBody.StartDate= @event.StartDate;
            eventBody.EndDate= @event.EndDate;  
            eventBody.Location = @event.Location;
            eventBody.Capacity = @event.Capacity;
            eventBody.OrganizerId = @event.OrganizerId;

            _context.Events.Add(eventBody);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEvent(EventRequestDTO @event, int eventId)
        {
            var existingEvent = await _context.Events.FindAsync(eventId);
            if (existingEvent != null)
            {
                existingEvent.Name = @event.Name;
                existingEvent.Description = @event.Description;
                existingEvent.StartDate = @event.StartDate;
                existingEvent.EndDate = @event.EndDate;
                existingEvent.Location = @event.Location;
                existingEvent.Capacity = @event.Capacity;
                existingEvent.OrganizerId = @event.OrganizerId;

                _context.Events.Update(existingEvent);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteEvent(int eventId)
        {
            var @event = await _context.Events.FindAsync(eventId);
            if (@event != null)
            {
                _context.Events.Remove(@event);
                await _context.SaveChangesAsync();
            }
        }


    }
}
