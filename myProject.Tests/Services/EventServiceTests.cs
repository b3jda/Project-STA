using Xunit;
using Moq;
using System.Threading.Tasks;
using EventManagementTests.Services.Interfaces;
using EventManagementTests.Services.Implementations;
using EventManagementTests.DTOs;
using System;
using EventManagementTests.Repositories.Interfaces;
using EventSphereManagement.Services.Implementations;
using AutoMapper;
using EventManagementTests.Models;

namespace EventManagementTests
{
    public class EventServiceTests
    {
        private readonly Mock<IEventRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly EventService _service;

        public EventServiceTests()
        {
            _repositoryMock = new Mock<IEventRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new EventService(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task AddEvent_ShouldCallRepositoryAddEvent_WhenEventDtoIsValid()
        {
            // Arrange
            var eventDto = new EventRequestDTO
            {
                Name = "Tech Conference",
                Description = "Annual tech conference",
                StartDate = new DateTime(2023, 12, 01),
                EndDate = new DateTime(2023, 12, 05),
                Location = "New York",
                Capacity = 1000,
                OrganizerId = 10
            };

            // Act
            await _service.AddEvent(eventDto);

            // Assert
            _repositoryMock.Verify(r => r.AddEvent(eventDto), Times.Once);
        }

        [Fact]
        public async Task AddEvent_ShouldThrowArgumentNullException_WhenEventDtoIsNull()
        {
            // Arrange
            EventRequestDTO eventDto = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.AddEvent(eventDto));
            _repositoryMock.Verify(r => r.AddEvent(It.IsAny<EventRequestDTO>()), Times.Never);
        }

        [Fact]
        public async Task AddEvent_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            var eventDto = new EventRequestDTO
            {
                Name = "Tech Conference",
                Description = "Annual tech conference",
                StartDate = new DateTime(2023, 12, 01),
                EndDate = new DateTime(2023, 12, 05),
                Location = "New York",
                Capacity = 1000,
                OrganizerId = 10
            };

            _repositoryMock.Setup(r => r.AddEvent(eventDto)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.AddEvent(eventDto));
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.AddEvent(eventDto), Times.Once);
        }

        //metoda tjeter
        [Fact]
        public async Task DeleteEvent_ShouldCallRepositoryDeleteEvent_WhenEventExists()
        {
            // Arrange
            var eventId = 1;
            var eventEntity = new Event { EventId = eventId, Name = "Tech Conference" };

            _repositoryMock.Setup(r => r.GetEventById(eventId)).ReturnsAsync(eventEntity);

            // Act
            await _service.DeleteEvent(eventId);

            // Assert
            _repositoryMock.Verify(r => r.GetEventById(eventId), Times.Once);
            _repositoryMock.Verify(r => r.DeleteEvent(eventId), Times.Once);
        }

        [Fact]
        public async Task DeleteEvent_ShouldThrowKeyNotFoundException_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = 1;

            _repositoryMock.Setup(r => r.GetEventById(eventId)).ReturnsAsync((Event)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteEvent(eventId));
            _repositoryMock.Verify(r => r.GetEventById(eventId), Times.Once);
            _repositoryMock.Verify(r => r.DeleteEvent(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteEvent_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            var eventId = 1;
            var eventEntity = new Event { EventId = eventId, Name = "Tech Conference" };

            _repositoryMock.Setup(r => r.GetEventById(eventId)).ReturnsAsync(eventEntity);
            _repositoryMock.Setup(r => r.DeleteEvent(eventId)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.DeleteEvent(eventId));
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.GetEventById(eventId), Times.Once);
            _repositoryMock.Verify(r => r.DeleteEvent(eventId), Times.Once);
        }
        //metoda tjeter
        [Fact]
        public async Task GetAllEvents_ShouldReturnEvents_WhenEventsExist()
        {
            // Arrange
            var events = new List<Event>
            {
                new Event { EventId = 1, Name = "Event 1", Description = "Description 1", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Location = "Location 1", Capacity = 100, OrganizerId = 1 },
                new Event { EventId = 2, Name = "Event 2", Description = "Description 2", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Location = "Location 2", Capacity = 200, OrganizerId = 2 }
            };

            var eventDtos = new List<EventDTO>
            {
                new EventDTO { EventId = 1, Name = "Event 1", Description = "Description 1", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Location = "Location 1", Capacity = 100, OrganizerId = 1 },
                new EventDTO { EventId = 2, Name = "Event 2", Description = "Description 2", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Location = "Location 2", Capacity = 200, OrganizerId = 2 }
            };

            _repositoryMock.Setup(r => r.GetAllEvents()).ReturnsAsync(events);
            _mapperMock.Setup(m => m.Map<IEnumerable<EventDTO>>(events)).Returns(eventDtos);

            // Act
            var result = await _service.GetAllEvents();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _repositoryMock.Verify(r => r.GetAllEvents(), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<EventDTO>>(events), Times.Once);
        }

        [Fact]
        public async Task GetAllEvents_ShouldReturnEmptyList_WhenNoEventsExist()
        {
            // Arrange
            var events = new List<Event>();
            var eventDtos = new List<EventDTO>();

            _repositoryMock.Setup(r => r.GetAllEvents()).ReturnsAsync(events);
            _mapperMock.Setup(m => m.Map<IEnumerable<EventDTO>>(events)).Returns(eventDtos);

            // Act
            var result = await _service.GetAllEvents();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _repositoryMock.Verify(r => r.GetAllEvents(), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<EventDTO>>(events), Times.Once);
        }

        [Fact]
        public async Task GetAllEvents_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAllEvents()).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.GetAllEvents());
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.GetAllEvents(), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<EventDTO>>(It.IsAny<IEnumerable<Event>>()), Times.Never);
        }

        //metoda tjeter

        [Fact]
        public async Task GetEventById_ShouldReturnEvent_WhenEventExists()
        {
            // Arrange
            var eventId = 1;
            var eventEntity = new Event { EventId = eventId, Name = "Tech Conference", Description = "Annual tech conference", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Location = "New York", Capacity = 1000, OrganizerId = 10 };
            var eventDto = new EventDTO { EventId = eventId, Name = "Tech Conference", Description = "Annual tech conference", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Location = "New York", Capacity = 1000, OrganizerId = 10 };

            _repositoryMock.Setup(r => r.GetEventById(eventId)).ReturnsAsync(eventEntity);
            _mapperMock.Setup(m => m.Map<EventDTO>(eventEntity)).Returns(eventDto);

            // Act
            var result = await _service.GetEventById(eventId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eventId, result.EventId);
            Assert.Equal("Tech Conference", result.Name);
            _repositoryMock.Verify(r => r.GetEventById(eventId), Times.Once);
            _mapperMock.Verify(m => m.Map<EventDTO>(eventEntity), Times.Once);
        }

        [Fact]
        public async Task GetEventById_ShouldThrowKeyNotFoundException_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = 1;

            _repositoryMock.Setup(r => r.GetEventById(eventId)).ReturnsAsync((Event)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetEventById(eventId));
            _repositoryMock.Verify(r => r.GetEventById(eventId), Times.Once);
            _mapperMock.Verify(m => m.Map<EventDTO>(It.IsAny<Event>()), Times.Never);
        }

        [Fact]
        public async Task GetEventById_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            var eventId = 1;

            _repositoryMock.Setup(r => r.GetEventById(eventId)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.GetEventById(eventId));
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.GetEventById(eventId), Times.Once);
            _mapperMock.Verify(m => m.Map<EventDTO>(It.IsAny<Event>()), Times.Never);
        }
        //metoda tjeter
        [Fact]
        public async Task UpdateEvent_ShouldCallRepositoryUpdateEvent_WhenEventDtoIsValid()
        {
            // Arrange
            var eventId = 1;
            var eventDto = new EventDTO { EventId = eventId, Name = "Updated Event", Description = "Updated Description", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Location = "Updated Location", Capacity = 100, OrganizerId = 1 };
            var existingEvent = new Event { EventId = eventId, Name = "Existing Event", Description = "Existing Description", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Location = "Existing Location", Capacity = 100, OrganizerId = 1 };
            var updatedEventRequestDto = new EventRequestDTO { Name = "Updated Event", Description = "Updated Description", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Location = "Updated Location", Capacity = 100, OrganizerId = 1 };

            _repositoryMock.Setup(r => r.GetEventById(eventId)).ReturnsAsync(existingEvent);
            _mapperMock.Setup(m => m.Map<EventRequestDTO>(existingEvent)).Returns(updatedEventRequestDto);

            // Act
            await _service.UpdateEvent(eventDto, eventId);

            // Assert
            _repositoryMock.Verify(r => r.GetEventById(eventId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateEvent(updatedEventRequestDto, eventId), Times.Once);
            _mapperMock.Verify(m => m.Map<EventRequestDTO>(existingEvent), Times.Once);
        }

        [Fact]
        public async Task UpdateEvent_ShouldThrowArgumentNullException_WhenEventDtoIsNull()
        {
            // Arrange
            EventDTO eventDto = null;
            var eventId = 1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateEvent(eventDto, eventId));
            _repositoryMock.Verify(r => r.GetEventById(It.IsAny<int>()), Times.Never);
            _repositoryMock.Verify(r => r.UpdateEvent(It.IsAny<EventRequestDTO>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task UpdateEvent_ShouldThrowKeyNotFoundException_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = 1;
            var eventDto = new EventDTO { EventId = eventId, Name = "Updated Event", Description = "Updated Description", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Location = "Updated Location", Capacity = 100, OrganizerId = 1 };

            _repositoryMock.Setup(r => r.GetEventById(eventId)).ReturnsAsync((Event)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateEvent(eventDto, eventId));
            _repositoryMock.Verify(r => r.GetEventById(eventId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateEvent(It.IsAny<EventRequestDTO>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task UpdateEvent_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            var eventId = 1;
            var eventDto = new EventDTO { EventId = eventId, Name = "Updated Event", Description = "Updated Description", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Location = "Updated Location", Capacity = 100, OrganizerId = 1 };
            var existingEvent = new Event { EventId = eventId, Name = "Existing Event", Description = "Existing Description", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Location = "Existing Location", Capacity = 100, OrganizerId = 1 };
            var updatedEventRequestDto = new EventRequestDTO { Name = "Updated Event", Description = "Updated Description", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), Location = "Updated Location", Capacity = 100, OrganizerId = 1 };

            _repositoryMock.Setup(r => r.GetEventById(eventId)).ReturnsAsync(existingEvent);
            _mapperMock.Setup(m => m.Map<EventRequestDTO>(existingEvent)).Returns(updatedEventRequestDto);
            _repositoryMock.Setup(r => r.UpdateEvent(updatedEventRequestDto, eventId)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.UpdateEvent(eventDto, eventId));
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.GetEventById(eventId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateEvent(updatedEventRequestDto, eventId), Times.Once);
            _mapperMock.Verify(m => m.Map<EventRequestDTO>(existingEvent), Times.Once);
        }
    }
}
