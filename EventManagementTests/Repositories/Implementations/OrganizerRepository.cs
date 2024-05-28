using EventManagementTests.Data;
using EventManagementTests.DTOs;
using EventManagementTests.Models;
using EventManagementTests.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventManagementTests.Repositories.Implementations
{
    public class OrganizerRepository : IOrganizerRepository
    {
        private readonly AppDbContext _context;
        public OrganizerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddOrganizer(OrganizerRequestDTO request)
        {
            Organizer requestBody = new Organizer();
            requestBody.Name = request.Name;
            requestBody.Phone=request.Phone;

            _context.Organizers.Add(requestBody);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteOrganizer(int organizerId)
        {
            var organizer = await _context.Organizers.FindAsync(organizerId);
            if (organizer != null)
            {
                _context.Organizers.Remove(organizer);
                await _context.SaveChangesAsync();
            }  
        }

        public async Task<IEnumerable<Organizer>> GetAllOrganizers()
        {
            return await _context.Organizers.ToListAsync();
        }

        public async Task<Organizer> GetOrganizerById(int organizerId)
        {
            var organizer = await _context.Organizers.FindAsync(organizerId);

            return organizer ?? new Organizer();
        }

        public async Task UpdateOrganizer(OrganizerRequestDTO requestBody, int organizerId)
        {
            var organizer = await _context.Organizers.FindAsync(organizerId);

            if (organizer != null)
            {
                organizer.Name=requestBody.Name;
                organizer.Phone=requestBody.Phone;

                _context.Entry(organizer).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }
}
