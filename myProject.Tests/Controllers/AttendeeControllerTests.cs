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
    public class AttendeeControllerTests
    {
        private readonly Mock<IAttendeeService> _serviceMock;
        private readonly AttendeeController _controller;

        public AttendeeControllerTests()
        {
            _serviceMock = new Mock<IAttendeeService>();
            _controller = new AttendeeController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAllAttendees_ShouldReturnOkResult_WithListOfAttendees()
        {
            // Arrange
            var attendees = new List<AttendeeDTO>
            {
                new AttendeeDTO { AttendeeId = 1, Name = "John Doe" },
                new AttendeeDTO { AttendeeId = 2, Name = "Jane Smith" }
            };

            _serviceMock.Setup(s => s.GetAllAttendees()).ReturnsAsync(attendees);

            // Act
            var result = await _controller.GetAllAttendees();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<AttendeeDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            _serviceMock.Verify(s => s.GetAllAttendees(), Times.Once);
        }

        [Fact]
        public async Task GetAllAttendees_ShouldReturnOkResult_WithEmptyList_WhenNoAttendeesExist()
        {
            // Arrange
            var attendees = new List<AttendeeDTO>();

            _serviceMock.Setup(s => s.GetAllAttendees()).ReturnsAsync(attendees);

            // Act
            var result = await _controller.GetAllAttendees();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<AttendeeDTO>>(okResult.Value);
            Assert.Empty(returnValue);
            _serviceMock.Verify(s => s.GetAllAttendees(), Times.Once);
        }

        [Fact]
        public async Task GetAttendeeById_ShouldReturnOkResult_WithAttendee_WhenAttendeeExists()
        {
            // Arrange
            var attendeeId = 1;
            var attendeeDto = new AttendeeDTO { AttendeeId = attendeeId, Name = "John Doe" };

            _serviceMock.Setup(s => s.GetAttendeeById(attendeeId)).ReturnsAsync(attendeeDto);

            // Act
            var result = await _controller.GetAttendeeById(attendeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<AttendeeDTO>(okResult.Value);
            Assert.Equal(attendeeId, returnValue.AttendeeId);
            _serviceMock.Verify(s => s.GetAttendeeById(attendeeId), Times.Once);
        }

        [Fact]
        public async Task GetAttendeeById_ShouldReturnNotFound_WhenAttendeeDoesNotExist()
        {
            // Arrange
            var attendeeId = 1;

            _serviceMock.Setup(s => s.GetAttendeeById(attendeeId)).ReturnsAsync((AttendeeDTO)null);

            // Act
            var result = await _controller.GetAttendeeById(attendeeId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
            _serviceMock.Verify(s => s.GetAttendeeById(attendeeId), Times.Once);
        }

        [Fact]
        public async Task AddAttendee_ShouldReturnOkResult_WhenAttendeeIsAdded()
        {
            // Arrange
            var attendeeDto = new AttendeeDTO { AttendeeId = 1, Name = "John Doe" };

            // Act
            var result = await _controller.AddAttendee(attendeeDto);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.AddAttendee(attendeeDto), Times.Once);
        }

        [Fact]
        public async Task AddAttendee_ShouldThrowArgumentNullException_WhenAttendeeDtoIsNull()
        {
            // Arrange
            AttendeeDTO attendeeDto = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _controller.AddAttendee(attendeeDto));
            _serviceMock.Verify(s => s.AddAttendee(It.IsAny<AttendeeDTO>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAttendee_ShouldReturnOkResult_WhenAttendeeIsDeleted()
        {
            // Arrange
            var attendeeId = 1;

            // Act
            var result = await _controller.DeleteAttendee(attendeeId);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.DeleteAttendee(attendeeId), Times.Once);
        }

        [Fact]
        public async Task DeleteAttendee_ShouldReturnNotFound_WhenAttendeeDoesNotExist()
        {
            // Arrange
            var attendeeId = 1;

            _serviceMock.Setup(s => s.DeleteAttendee(attendeeId)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.DeleteAttendee(attendeeId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _serviceMock.Verify(s => s.DeleteAttendee(attendeeId), Times.Once);
        }


        [Fact]
        public async Task UpdateAttendee_ShouldReturnOkResult_WhenAttendeeIsUpdated()
        {
            // Arrange
            var attendeeId = 1;
            var attendeeDto = new AttendeeDTO { AttendeeId = attendeeId, Name = "John Doe" };

            // Act
            var result = await _controller.UpdateAttendee(attendeeDto, attendeeId);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.UpdateAttendee(attendeeDto, attendeeId), Times.Once);
        }

        [Fact]
        public async Task UpdateAttendee_ShouldThrowArgumentNullException_WhenAttendeeDtoIsNull()
        {
            // Arrange
            AttendeeDTO attendeeDto = null;
            var attendeeId = 1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _controller.UpdateAttendee(attendeeDto, attendeeId));
            _serviceMock.Verify(s => s.UpdateAttendee(It.IsAny<AttendeeDTO>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAttendee_ShouldReturnNotFound_WhenAttendeeDoesNotExist()
        {
            // Arrange
            var attendeeId = 1;
            var attendeeDto = new AttendeeDTO { AttendeeId = attendeeId, Name = "John Doe" };

            _serviceMock.Setup(s => s.UpdateAttendee(attendeeDto, attendeeId)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.UpdateAttendee(attendeeDto, attendeeId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _serviceMock.Verify(s => s.UpdateAttendee(attendeeDto, attendeeId), Times.Once);
        }

        [Fact]
        public async Task UpdateAttendee_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            var attendeeId = 1;
            var attendeeDto = new AttendeeDTO { AttendeeId = attendeeId, Name = "John Doe" };

            _serviceMock.Setup(s => s.UpdateAttendee(attendeeDto, attendeeId)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.UpdateAttendee(attendeeDto, attendeeId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result); 
            Assert.Equal(500, objectResult.StatusCode); 
            Assert.Equal("Database error", objectResult.Value); 

            _serviceMock.Verify(s => s.UpdateAttendee(attendeeDto, attendeeId), Times.Once);
        }

    }
}
