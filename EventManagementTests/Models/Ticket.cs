using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EventManagementTests.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        public decimal Price { get; set; }
        public string TicketType { get; set; }
        public int QuantityAvailable { get; set; }
        public Event Event { get; set; }
    }
}
