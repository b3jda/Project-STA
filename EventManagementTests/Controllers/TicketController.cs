using EventManagementTests.DTOs;
using EventManagementTests.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class TicketController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TicketDTO>> GetTicketById(int id)
    {
        try
        {
            var ticket = await _ticketService.GetTicketById(id);
            return Ok(ticket);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Ticket not found" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> AddTicket(TicketRequestDTO ticketDto)
    {
        if (ticketDto == null)
        {
            return BadRequest(new { message = "TicketRequestDTO cannot be null" });
        }

        try
        {
            await _ticketService.AddTicket(ticketDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTicket(int id)
    {
        try
        {
            await _ticketService.DeleteTicket(id);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Ticket not found" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("{id},{quantity}/sell")]
    public async Task<ActionResult> SellTicket(int id, int quantity)
    {
        try
        {
            await _ticketService.SellTicket(id, quantity);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Ticket not found" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }


    [HttpPost("{id},{quantity}/refund")]
    public async Task<ActionResult> RefundTicket(int id, int quantity)
    {
        try
        {
            await _ticketService.RefundTicket(id, quantity);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Ticket not found" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTicket(int id, TicketRequestDTO ticketDto)
    {
        if (ticketDto == null)
        {
            return BadRequest(new { message = "TicketRequestDTO cannot be null" });
        }

        try
        {
            await _ticketService.UpdateTicket(ticketDto, id);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Ticket not found" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
