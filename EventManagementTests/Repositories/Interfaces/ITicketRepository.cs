using EventManagementTests.Models;
﻿using EventManagementTests.DTOs;
using EventManagementTests.Models;


namespace EventManagementTests.Repositories.Interfaces
{
    public interface ITicketRepository
    {
        Task<Ticket> GetTicketById(int ticketId);
        Task AddTicket(TicketRequestDTO request);
        Task UpdateTicket(TicketRequestDTO request, int ticketId);
        Task DeleteTicket(int ticketId);
    }
}
   
