using EventManagementTests.DTOs;

namespace EventManagementTests.Services.Interfaces
{
    public interface ITicketService
    {
        Task<TicketDTO> GetTicketById(int ticketId);
        Task AddTicket(TicketRequestDTO request);
        Task DeleteTicket(int ticketId);
        Task SellTicket(int ticketId, int quantity);
        Task RefundTicket(int ticketId, int quantity);
        Task UpdateTicket(TicketRequestDTO ticketDto, int ticketId);
    }
}

