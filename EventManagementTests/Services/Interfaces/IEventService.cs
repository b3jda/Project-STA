    using EventManagementTests.DTOs;

namespace EventManagementTests.Services.Interfaces
{
    public interface IEventService
    {
        Task<EventDTO> GetEventById(int eventId);
        Task<IEnumerable<EventDTO>> GetAllEvents();
        Task AddEvent(EventRequestDTO eventDto);
        Task UpdateEvent(EventDTO eventDto, int eventId);
        Task DeleteEvent(int eventId);
    }
}
