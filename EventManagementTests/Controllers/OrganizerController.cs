using AutoMapper;
using EventManagementTests.DTOs;
using EventManagementTests.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementTests.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizerController : ControllerBase
    {
        private readonly IOrganizerService _organizerService;

        public OrganizerController(IOrganizerService organizerService)
        {
            _organizerService = organizerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrganizerDTO>>> GetAllOrganizers()
        {
            var organizers = await _organizerService.GetAllOrganizers();
            return Ok(organizers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrganizerDTO>> GetOrganizerById(int id)
        {
            try
            {
                var organizer = await _organizerService.GetOrganizerById(id);
                return Ok(organizer);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Organizer not found" });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrganizer(OrganizerRequestDTO organizerDto)
        {
            if (organizerDto == null)
            {
                throw new ArgumentNullException(nameof(organizerDto), "OrganizerRequestDTO cannot be null");
            }

            await _organizerService.CreateOrganizer(organizerDto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrganizer(OrganizerRequestDTO organizerDto, int id)
        {
            if (organizerDto == null)
            {
                throw new ArgumentNullException(nameof(organizerDto), "OrganizerRequestDTO cannot be null");
            }

            try
            {
                await _organizerService.UpdateOrganizer(organizerDto, id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Organizer not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrganizer(int id)
        {
            try
            {
                await _organizerService.DeleteOrganizer(id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Organizer not found" });
            }
        }

    }
}
