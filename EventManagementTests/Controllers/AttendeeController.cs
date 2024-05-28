using EventManagementTests.DTOs;
using EventManagementTests.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementTests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendeeController : ControllerBase
    {
        private readonly IAttendeeService _service;

        public AttendeeController(IAttendeeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AttendeeDTO>>> GetAllAttendees()
        {
            var response = await _service.GetAllAttendees();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AttendeeDTO>> GetAttendeeById(int attendeeId)
        {
            var attendee = await _service.GetAttendeeById(attendeeId);
            if (attendee == null)
            {
                return NotFound();
            }
            return Ok(attendee);
        }

        [HttpPost]
        public async Task<ActionResult> AddAttendee(AttendeeDTO attendeeDto)
        {
            if (attendeeDto == null)
            {
                throw new ArgumentNullException(nameof(attendeeDto), "AttendeeDTO cannot be null");
            }

            await _service.AddAttendee(attendeeDto);
            return Ok();
        }


        [HttpDelete("{id}")]
      
        public async Task<ActionResult> DeleteAttendee(int id)
        {
            try
            {
                await _service.DeleteAttendee(id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAttendee(AttendeeDTO attendeeDto, int id)
        {
            if (attendeeDto == null)
            {
                throw new ArgumentNullException(nameof(attendeeDto), "AttendeeDTO cannot be null");
            }

            try
            {
                await _service.UpdateAttendee(attendeeDto, id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); 
            }
        }

    }


}

