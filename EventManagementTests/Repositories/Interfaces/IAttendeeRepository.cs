using EventManagementTests.DTOs;
using EventManagementTests.Models;
﻿using EventManagementTests.Models;


namespace EventManagementTests.Repositories.Interfaces
{
    public interface IAttendeeRepository
    {
        Task<Attendee> GetAttendeeById(int attendeeId);
        Task<IEnumerable<Attendee>> GetAllAttendees();
        Task AddAttendee(AttendeeDTO attendee);
        Task UpdateAttendee(Attendee attendee,int attendeeId);
        Task DeleteAttendee(int attendeeId);
     
       
    }
}
