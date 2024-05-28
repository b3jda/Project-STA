using AutoMapper;
using EventManagementTests.DTOs;
using EventManagementTests.Models;
using EventManagementTests.Repositories.Implementations;
using EventManagementTests.Repositories.Interfaces;
﻿using EventManagementTests.DTOs;
using EventManagementTests.Services.Interfaces;

namespace EventManagementTests.Services.Implementations
{
    public class AttendeeService : IAttendeeService


    {
        public readonly IAttendeeRepository _repository;
        private readonly IMapper _mapper;
       


        public AttendeeService(IAttendeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AttendeeDTO> GetAttendeeById(int attendeeId)
        {
            var attendee = await _repository.GetAttendeeById(attendeeId);
            if (attendee == null)
            {
                throw new KeyNotFoundException("Attendee not found");
            }
            return _mapper.Map<AttendeeDTO>(attendee);
        }

        public async Task<IEnumerable<AttendeeDTO>> GetAllAttendees()
        {
            var attendees = await _repository.GetAllAttendees();
            return _mapper.Map<List<AttendeeDTO>>(attendees);
            
        }

        public async Task AddAttendee(AttendeeDTO attendeeDto)
        {
            if (attendeeDto == null)
            {
                throw new ArgumentNullException(nameof(attendeeDto), "AttendeeDTO cannot be null");
            }

            await _repository.AddAttendee(attendeeDto);
        }

        public async Task UpdateAttendee(AttendeeDTO attendeeDto, int attendeeId)
        {
            if (attendeeDto == null)
            {
                throw new ArgumentNullException(nameof(attendeeDto), "AttendeeDTO cannot be null");
            }

            var existingAttendee = await _repository.GetAttendeeById(attendeeId);
            if (existingAttendee == null)
            {
                throw new KeyNotFoundException("Attendee not found");
            }

            var attendee = _mapper.Map<Attendee>(attendeeDto);
            await _repository.UpdateAttendee(attendee, attendeeId);
        }





        public async Task DeleteAttendee(int attendeeId)
        {
            var existingAttendee = await _repository.GetAttendeeById(attendeeId);
            if (existingAttendee == null)
            {
                throw new KeyNotFoundException("Attendee not found");
            }

            await _repository.DeleteAttendee(attendeeId);
        }

    }

}



   