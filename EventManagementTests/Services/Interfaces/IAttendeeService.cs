using EventManagementTests.DTOs;

namespace EventManagementTests.Services.Interfaces
{
    public interface IAttendeeService
    {
        Task<AttendeeDTO> GetAttendeeById(int attendeeId);
        Task<IEnumerable<AttendeeDTO>> GetAllAttendees();
        Task AddAttendee(AttendeeDTO attendeeDto);
        Task UpdateAttendee(AttendeeDTO attendeeDto, int attendeeId);
        Task DeleteAttendee(int attendeeId);

       

    }
}
