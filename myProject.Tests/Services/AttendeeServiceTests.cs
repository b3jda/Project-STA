using AutoMapper;
using EventManagementTests.DTOs;
using EventManagementTests.Models;
using EventManagementTests.Repositories.Interfaces;
using EventManagementTests.Services.Implementations;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myProject.Tests.Services
{

    public class AttendeeServiceTests
    {

        private readonly Mock<IAttendeeRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AttendeeService _service;

        public AttendeeServiceTests()
        {
            _repositoryMock = new Mock<IAttendeeRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new AttendeeService(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAttendeeById_ShouldReturnAttendee_WhenAttendeeExists()
        {
            // Arrange
            var attendeeId = 1;
            var attendee = new Attendee
            {
                AttendeeId = attendeeId,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Phone = "1234567890"
            };
            var attendeeDto = new AttendeeDTO
            {
                AttendeeId = attendeeId,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Phone = "1234567890"
            };

            _repositoryMock.Setup(r => r.GetAttendeeById(attendeeId)).ReturnsAsync(attendee);
            _mapperMock.Setup(m => m.Map<AttendeeDTO>(attendee)).Returns(attendeeDto);

            // Act
            var result = await _service.GetAttendeeById(attendeeId);

            // Assert
            Assert.Equal(attendeeDto.AttendeeId, result.AttendeeId);
            Assert.Equal(attendeeDto.Name, result.Name);
            Assert.Equal(attendeeDto.Email, result.Email);
            Assert.Equal(attendeeDto.Phone, result.Phone);
            _repositoryMock.Verify(r => r.GetAttendeeById(attendeeId), Times.Once);
            _mapperMock.Verify(m => m.Map<AttendeeDTO>(attendee), Times.Once);
        }

        [Fact]
        public async Task GetAttendeeById_ShouldThrowKeyNotFoundException_WhenAttendeeDoesNotExist()
        {
            // Arrange
            var attendeeId = 1;
            _repositoryMock.Setup(r => r.GetAttendeeById(attendeeId)).ReturnsAsync((Attendee)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetAttendeeById(attendeeId));
            _repositoryMock.Verify(r => r.GetAttendeeById(attendeeId), Times.Once);
            _mapperMock.Verify(m => m.Map<AttendeeDTO>(It.IsAny<Attendee>()), Times.Never);
        }

        [Fact]
        public async Task GetAttendeeById_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            var attendeeId = 1;
            _repositoryMock.Setup(r => r.GetAttendeeById(attendeeId)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.GetAttendeeById(attendeeId));
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.GetAttendeeById(attendeeId), Times.Once);
            _mapperMock.Verify(m => m.Map<AttendeeDTO>(It.IsAny<Attendee>()), Times.Never);
        }


        //metoda tjeter


        [Fact]
        public async Task GetAllAttendees_ShouldReturnAttendees_WhenAttendeesExist()
        {
            // Arrange
             var attendees = new List<Attendee> {
                new Attendee { AttendeeId = 1, Name = "John Doe", Email = "john.doe@example.com", Phone = "1234567890" },
                new Attendee { AttendeeId = 2, Name = "Jane Smith", Email = "jane.smith@example.com", Phone = "0987654321" }
             };

            var attendeeDtos = new List<AttendeeDTO>{
                new AttendeeDTO { AttendeeId = 1, Name = "John Doe", Email = "john.doe@example.com", Phone = "1234567890" },
                new AttendeeDTO { AttendeeId = 2, Name = "Jane Smith", Email = "jane.smith@example.com", Phone = "0987654321" }
             };

            _repositoryMock.Setup(r => r.GetAllAttendees()).ReturnsAsync(attendees);
            _mapperMock.Setup(m => m.Map<List<AttendeeDTO>>(attendees)).Returns(attendeeDtos);

            // Act
            var result = await _service.GetAllAttendees();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, r => r.AttendeeId == 1 && r.Name == "John Doe" && r.Email == "john.doe@example.com" && r.Phone == "1234567890");
            Assert.Contains(result, r => r.AttendeeId == 2 && r.Name == "Jane Smith" && r.Email == "jane.smith@example.com" && r.Phone == "0987654321");
            _repositoryMock.Verify(r => r.GetAllAttendees(), Times.Once);
            _mapperMock.Verify(m => m.Map<List<AttendeeDTO>>(attendees), Times.Once);
        }




        [Fact]
        public async Task GetAllAttendees_ShouldReturnEmptyList_WhenNoAttendeesExist()
        {
            // Arrange
            var attendees = new List<Attendee>();

            _repositoryMock.Setup(r => r.GetAllAttendees()).ReturnsAsync(attendees);
            _mapperMock.Setup(m => m.Map<List<AttendeeDTO>>(attendees)).Returns(new List<AttendeeDTO>());

            // Act
            var result = await _service.GetAllAttendees();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _repositoryMock.Verify(r => r.GetAllAttendees(), Times.Once);
            _mapperMock.Verify(m => m.Map<List<AttendeeDTO>>(attendees), Times.Once);
        }


        [Fact]
        public async Task GetAllAttendees_ShouldReturnNull_WhenRepositoryReturnsNull()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAllAttendees()).ReturnsAsync((List<Attendee>)null);

            // Act
            var result = await _service.GetAllAttendees();

            // Assert
            Assert.Null(result);
            _repositoryMock.Verify(r => r.GetAllAttendees(), Times.Once);
            _mapperMock.Verify(m => m.Map<AttendeeDTO>(It.IsAny<Attendee>()), Times.Never);
        }

        [Fact]
        public async Task GetAllAttendees_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAllAttendees()).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.GetAllAttendees());
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.GetAllAttendees(), Times.Once);
            _mapperMock.Verify(m => m.Map<AttendeeDTO>(It.IsAny<Attendee>()), Times.Never);
        }

        //metoda tjeter
        [Fact]
        public async Task AddAttendee_ShouldCallRepositoryAddAttendee_WhenAttendeeDtoIsValid()
        {
            // Arrange
            var attendeeDto = new AttendeeDTO { AttendeeId = 1, Name = "John Doe", Email = "john.doe@example.com", Phone = "1234567890" };

            // Act
            await _service.AddAttendee(attendeeDto);

            // Assert
            _repositoryMock.Verify(r => r.AddAttendee(attendeeDto), Times.Once);
        }

        [Fact]
        public async Task AddAttendee_ShouldThrowArgumentNullException_WhenAttendeeDtoIsNull()
        {
            // Arrange
            AttendeeDTO attendeeDto = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.AddAttendee(attendeeDto));
            _repositoryMock.Verify(r => r.AddAttendee(It.IsAny<AttendeeDTO>()), Times.Never);
        }

        [Fact]
        public async Task AddAttendee_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            var attendeeDto = new AttendeeDTO { AttendeeId = 1, Name = "John Doe", Email = "john.doe@example.com", Phone = "1234567890" };
            _repositoryMock.Setup(r => r.AddAttendee(attendeeDto)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.AddAttendee(attendeeDto));
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.AddAttendee(attendeeDto), Times.Once);
        }
        //metoda tjeter
        [Fact]
        public async Task UpdateAttendee_ShouldCallRepositoryUpdateAttendee_WhenAttendeeDtoIsValid()
        {
            // Arrange
            var attendeeDto = new AttendeeDTO { AttendeeId = 1, Name = "John Doe", Email = "john.doe@example.com", Phone = "1234567890" };
            var attendee = new Attendee { AttendeeId = 1, Name = "John Doe", Email = "john.doe@example.com", Phone = "1234567890" };

            _repositoryMock.Setup(r => r.GetAttendeeById(attendeeDto.AttendeeId)).ReturnsAsync(attendee);
            _mapperMock.Setup(m => m.Map<Attendee>(attendeeDto)).Returns(attendee);

            // Act
            await _service.UpdateAttendee(attendeeDto, attendeeDto.AttendeeId);

            // Assert
            _repositoryMock.Verify(r => r.GetAttendeeById(attendeeDto.AttendeeId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAttendee(attendee, attendeeDto.AttendeeId), Times.Once);
        }


        [Fact]
        public async Task UpdateAttendee_ShouldThrowArgumentNullException_WhenAttendeeDtoIsNull()
        {
            // Arrange
            AttendeeDTO attendeeDto = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateAttendee(attendeeDto, 1));
            _repositoryMock.Verify(r => r.UpdateAttendee(It.IsAny<Attendee>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAttendee_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            var attendeeDto = new AttendeeDTO { AttendeeId = 1, Name = "John Doe", Email = "john.doe@example.com", Phone = "1234567890" };
            var attendee = new Attendee { AttendeeId = 1, Name = "John Doe", Email = "john.doe@example.com", Phone = "1234567890" };

            _repositoryMock.Setup(r => r.GetAttendeeById(attendeeDto.AttendeeId)).ReturnsAsync(attendee);
            _mapperMock.Setup(m => m.Map<Attendee>(attendeeDto)).Returns(attendee);
            _repositoryMock.Setup(r => r.UpdateAttendee(attendee, attendeeDto.AttendeeId)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.UpdateAttendee(attendeeDto, attendeeDto.AttendeeId));
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.GetAttendeeById(attendeeDto.AttendeeId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAttendee(attendee, attendeeDto.AttendeeId), Times.Once);
        }

        [Fact]
        public async Task DeleteAttendee_ShouldCallRepositoryDeleteAttendee_WhenAttendeeIdIsValid()
        {
            // Arrange
            var attendeeId = 1;
            var attendee = new Attendee { AttendeeId = attendeeId, Name = "John Doe" };

            _repositoryMock.Setup(r => r.GetAttendeeById(attendeeId)).ReturnsAsync(attendee);

            // Act
            await _service.DeleteAttendee(attendeeId);

            // Assert
            _repositoryMock.Verify(r => r.GetAttendeeById(attendeeId), Times.Once);
            _repositoryMock.Verify(r => r.DeleteAttendee(attendeeId), Times.Once);
        }


        [Fact]
        public async Task DeleteAttendee_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            var attendeeId = 1;
            var attendee = new Attendee { AttendeeId = attendeeId, Name = "John Doe" };

            _repositoryMock.Setup(r => r.GetAttendeeById(attendeeId)).ReturnsAsync(attendee);
            _repositoryMock.Setup(r => r.DeleteAttendee(attendeeId)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.DeleteAttendee(attendeeId));
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.GetAttendeeById(attendeeId), Times.Once);
            _repositoryMock.Verify(r => r.DeleteAttendee(attendeeId), Times.Once);
        }

    }
}

