using EventManagementTests.Data;
using EventManagementTests.DTOs;
using EventManagementTests.Models;
using EventManagementTests.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;



namespace EventManagementTests.Repositories.Implementations
{
    public class AttendeeRepository : IAttendeeRepository
    {

        private readonly AppDbContext _context;

        public AttendeeRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task AddAttendee(AttendeeDTO request)
        {
            Attendee requestBody = new Attendee
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
               
            };

            _context.Attendees.Add(requestBody);
            await _context.SaveChangesAsync();
        }



        public async Task DeleteAttendee(int attendeeId)
        {
            var attendee = await _context.Attendees.FindAsync(attendeeId);
            if (attendee != null)
            {
                _context.Attendees.Remove(attendee);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Attendee>> GetAllAttendees()
        {
            return await _context.Attendees.ToListAsync();
        }

        public async Task<Attendee> GetAttendeeById(int attendeeId)
        {
            return await _context.Attendees.FindAsync(attendeeId);
        }


        public async Task UpdateAttendee(Attendee attendee, int attendeeId)
        {
            var existingAttendee = await _context.Attendees.FindAsync(attendeeId);
            if (existingAttendee != null)
            {
                existingAttendee.Name = attendee.Name;
                existingAttendee.Email = attendee.Email;
                _context.Attendees.Update(existingAttendee);
                await _context.SaveChangesAsync();
            }
        }
    }
}
