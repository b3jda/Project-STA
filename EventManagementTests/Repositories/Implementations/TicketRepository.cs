using EventManagementTests.Data;
using EventManagementTests.DTOs;
using EventManagementTests.Models;
using EventManagementTests.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventManagementTests.Repositories.Implementations
{
    public class TicketRepostiory : ITicketRepository
    {
        private readonly AppDbContext _context;
        public TicketRepostiory(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddTicket(TicketRequestDTO request)
        {
            Ticket requestBody = new Ticket();
            requestBody.Price = request.Price;
            requestBody.TicketType= request.TicketType;
            requestBody.QuantityAvailable = request.QuantityAvailable;

            _context.Tickets.Add(requestBody);
            await _context.SaveChangesAsync();
        }

       

        public async Task DeleteTicket(int ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }

 

        public async Task<Ticket> GetTicketById(int ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);

            return ticket ?? new Ticket();
        }

 

        public async Task UpdateTicket(TicketRequestDTO request, int ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);

            if (ticket != null)
            {
               
                ticket.Price = request.Price;
                ticket.TicketType = request.TicketType;
                ticket.QuantityAvailable = request.QuantityAvailable;

                _context.Entry(ticket).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }
}
