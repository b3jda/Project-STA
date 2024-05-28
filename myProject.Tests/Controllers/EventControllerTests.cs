using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventManagementTests.Controllers;
using EventManagementTests.Services.Interfaces;
using EventManagementTests.DTOs;
using System;

namespace EventManagementTests
{
    public class EventControllerTests
    {
        private readonly Mock<IEventService> _serviceMock;
        private readonly EventController _controller;

        public EventControllerTests()
        {
            _serviceMock = new Mock<IEventService>();
            _controller = new EventController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAllEvents_ShouldReturnOkResult_WithListOfEvents()
        {
            // Arrange
            var events = new List<EventDTO>
            {
                new EventDTO { EventId = 1, Name = "Event 1" },
                new EventDTO { EventId = 2, Name = "Event 2" }
            };

            _serviceMock.Setup(s => s.GetAllEvents()).ReturnsAsync(events);

            // Act
            var result = await _controller.GetAllEvents();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<EventDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            _serviceMock.Verify(s => s.GetAllEvents(), Times.Once);
        }

        [Fact]
        public async Task GetAllEvents_ShouldReturnOkResult_WithEmptyList_WhenNoEventsExist()
        {
            // Arrange
            var events = new List<EventDTO>();

            _serviceMock.Setup(s => s.GetAllEvents()).ReturnsAsync(events);

            // Act
            var result = await _controller.GetAllEvents();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<EventDTO>>(okResult.Value);
            Assert.Empty(returnValue);
            _serviceMock.Verify(s => s.GetAllEvents(), Times.Once);
        }

        [Fact]
        public async Task GetEventById_ShouldReturnOkResult_WithEvent_WhenEventExists()
        {
            // Arrange
            var eventId = 1;
            var eventDto = new EventDTO { EventId = eventId, Name = "Event 1" };

            _serviceMock.Setup(s => s.GetEventById(eventId)).ReturnsAsync(eventDto);

            // Act
            var result = await _controller.GetEventById(eventId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<EventDTO>(okResult.Value);
            Assert.Equal(eventId, returnValue.EventId);
            _serviceMock.Verify(s => s.GetEventById(eventId), Times.Once);
        }

        [Fact]
        public async Task GetEventById_ShouldReturnNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = 1;

            _serviceMock.Setup(s => s.GetEventById(eventId)).ReturnsAsync((EventDTO)null);

            // Act
            var result = await _controller.GetEventById(eventId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
            _serviceMock.Verify(s => s.GetEventById(eventId), Times.Once);
        }

        [Fact]
        public async Task AddEvent_ShouldReturnOkResult_WhenEventIsAdded()
        {
            // Arrange
            var eventDto = new EventRequestDTO { Name = "Event 1" };

            // Act
            var result = await _controller.AddEvent(eventDto);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.AddEvent(eventDto), Times.Once);
        }

        [Fact]
        public async Task AddEvent_ShouldReturnBadRequest_WhenEventDtoIsNull()
        {
            // Arrange
            EventRequestDTO eventDto = null;

            // Act
            var result = await _controller.AddEvent(eventDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("EventRequestDTO object is null", badRequestResult.Value);
            _serviceMock.Verify(s => s.AddEvent(It.IsAny<EventRequestDTO>()), Times.Never);
        }

        [Fact]
        public async Task DeleteEvent_ShouldReturnOkResult_WhenEventIsDeleted()
        {
            // Arrange
            var eventId = 1;

            // Act
            var result = await _controller.DeleteEvent(eventId);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.DeleteEvent(eventId), Times.Once);
        }

        [Fact]
        public async Task DeleteEvent_ShouldReturnNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = 1;

            _serviceMock.Setup(s => s.DeleteEvent(eventId)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.DeleteEvent(eventId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _serviceMock.Verify(s => s.DeleteEvent(eventId), Times.Once);
        }

        [Fact]
        public async Task UpdateEvent_ShouldReturnOkResult_WhenEventIsUpdated()
        {
            // Arrange
            var eventId = 1;
            var eventDto = new EventDTO { EventId = eventId, Name = "Updated Event" };

            // Act
            var result = await _controller.UpdateEvent(eventDto, eventId);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.UpdateEvent(eventDto, eventId), Times.Once);
        }

        [Fact]
        public async Task UpdateEvent_ShouldReturnBadRequest_WhenEventDtoIsNull()
        {
            // Arrange
            EventDTO eventDto = null;
            var eventId = 1;

            // Act
            var result = await _controller.UpdateEvent(eventDto, eventId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("EventDTO object is null", badRequestResult.Value);
            _serviceMock.Verify(s => s.UpdateEvent(It.IsAny<EventDTO>(), It.IsAny<int>()), Times.Never);
        }
        [Fact]
        public async Task UpdateEvent_ShouldReturnNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = 1;
            var eventDto = new EventDTO { EventId = eventId, Name = "Updated Event" };

            _serviceMock.Setup(s => s.UpdateEvent(eventDto, eventId)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.UpdateEvent(eventDto, eventId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _serviceMock.Verify(s => s.UpdateEvent(eventDto, eventId), Times.Once);
        }

        [Fact]
        public async Task UpdateEvent_ShouldReturnInternalServerError_WhenRepositoryThrowsException()
        {
            // Arrange
            var eventId = 1;
            var eventDto = new EventDTO { EventId = eventId, Name = "Updated Event" };

            _serviceMock.Setup(s => s.UpdateEvent(eventDto, eventId)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.UpdateEvent(eventDto, eventId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Internal server error: Database error", statusCodeResult.Value);
            _serviceMock.Verify(s => s.UpdateEvent(eventDto, eventId), Times.Once);
        }
    }
}
