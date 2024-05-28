using EventManagementTests.DTOs;
using EventManagementTests.Services.Implementations;
using EventManagementTests.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EventManagementTests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _service;

        public EventController(IEventService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetAllEvents()
        {
            try
            {
                var response = await _service.GetAllEvents();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventDTO>> GetEventById(int eventId)
        {
            try
            {
                var events = await _service.GetEventById(eventId);
                if (events == null)
                {
                    return NotFound();
                }
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddEvent(EventRequestDTO eventDto)
        {
            try
            {
                if (eventDto == null)
                {
                    return BadRequest("EventRequestDTO object is null");
                }

                await _service.AddEvent(eventDto);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvent(int id)
        {
            try
            {
                await _service.DeleteEvent(id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEvent(EventDTO eventDto, int eventId)
        {
            try
            {
                if (eventDto == null)
                {
                    return BadRequest("EventDTO object is null");
                }

                await _service.UpdateEvent(eventDto, eventId);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
