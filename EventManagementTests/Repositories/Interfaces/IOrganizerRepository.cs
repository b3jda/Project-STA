using EventManagementTests.DTOs;
using EventManagementTests.Models;


namespace EventManagementTests.Repositories.Interfaces
{
    public interface IOrganizerRepository
    {
        Task<Organizer> GetOrganizerById(int organizerId);
        Task<IEnumerable<Organizer>> GetAllOrganizers();

        Task AddOrganizer(OrganizerRequestDTO request);
        Task UpdateOrganizer(OrganizerRequestDTO request, int organizerId);
        Task DeleteOrganizer(int organizerId);
      }
}
