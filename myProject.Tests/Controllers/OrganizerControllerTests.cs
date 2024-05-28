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
    public class OrganizerControllerTests
    {
        private readonly Mock<IOrganizerService> _serviceMock;
        private readonly OrganizerController _controller;

        public OrganizerControllerTests()
        {
            _serviceMock = new Mock<IOrganizerService>();
            _controller = new OrganizerController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAllOrganizers_ShouldReturnOkResult_WithListOfOrganizers()
        {
            // Arrange
            var organizers = new List<OrganizerDTO>
            {
                new OrganizerDTO { OrganizerId = 1, Name = "Organizer 1" },
                new OrganizerDTO { OrganizerId = 2, Name = "Organizer 2" }
            };

            _serviceMock.Setup(s => s.GetAllOrganizers()).ReturnsAsync(organizers);

            // Act
            var result = await _controller.GetAllOrganizers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OrganizerDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            _serviceMock.Verify(s => s.GetAllOrganizers(), Times.Once);
        }

        [Fact]
        public async Task GetAllOrganizers_ShouldReturnOkResult_WithEmptyList_WhenNoOrganizersExist()
        {
            // Arrange
            var organizers = new List<OrganizerDTO>();

            _serviceMock.Setup(s => s.GetAllOrganizers()).ReturnsAsync(organizers);

            // Act
            var result = await _controller.GetAllOrganizers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<OrganizerDTO>>(okResult.Value);
            Assert.Empty(returnValue);
            _serviceMock.Verify(s => s.GetAllOrganizers(), Times.Once);
        }

        [Fact]
        public async Task GetOrganizerById_ShouldReturnOkResult_WithOrganizer_WhenOrganizerExists()
        {
            // Arrange
            var organizerId = 1;
            var organizerDto = new OrganizerDTO { OrganizerId = organizerId, Name = "Organizer 1" };

            _serviceMock.Setup(s => s.GetOrganizerById(organizerId)).ReturnsAsync(organizerDto);

            // Act
            var result = await _controller.GetOrganizerById(organizerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<OrganizerDTO>(okResult.Value);
            Assert.Equal(organizerId, returnValue.OrganizerId);
            _serviceMock.Verify(s => s.GetOrganizerById(organizerId), Times.Once);
        }

        [Fact]
        public async Task GetOrganizerById_ShouldReturnNotFound_WhenOrganizerDoesNotExist()
        {
            // Arrange
            var organizerId = 1;

            _serviceMock.Setup(s => s.GetOrganizerById(organizerId)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.GetOrganizerById(organizerId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var value = notFoundResult.Value.GetType().GetProperty("message").GetValue(notFoundResult.Value, null);
            Assert.Equal("Organizer not found", value);
            _serviceMock.Verify(s => s.GetOrganizerById(organizerId), Times.Once);
        }


        [Fact]
        public async Task CreateOrganizer_ShouldReturnOkResult_WhenOrganizerIsCreated()
        {
            // Arrange
            var organizerDto = new OrganizerRequestDTO { Name = "Organizer 1", Phone = "1234567890" };

            // Act
            var result = await _controller.CreateOrganizer(organizerDto);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.CreateOrganizer(organizerDto), Times.Once);
        }

        [Fact]
        public async Task CreateOrganizer_ShouldThrowArgumentNullException_WhenOrganizerDtoIsNull()
        {
            // Arrange
            OrganizerRequestDTO organizerDto = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _controller.CreateOrganizer(organizerDto));
            _serviceMock.Verify(s => s.CreateOrganizer(It.IsAny<OrganizerRequestDTO>()), Times.Never);
        }


        [Fact]
        public async Task DeleteOrganizer_ShouldReturnOkResult_WhenOrganizerIsDeleted()
        {
            // Arrange
            var organizerId = 1;

            // Act
            var result = await _controller.DeleteOrganizer(organizerId);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.DeleteOrganizer(organizerId), Times.Once);
        }

        [Fact]
        public async Task DeleteOrganizer_ShouldReturnNotFound_WhenOrganizerDoesNotExist()
        {
            // Arrange
            var organizerId = 1;

            _serviceMock.Setup(s => s.DeleteOrganizer(organizerId)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.DeleteOrganizer(organizerId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var value = notFoundResult.Value.GetType().GetProperty("message").GetValue(notFoundResult.Value, null);
            Assert.Equal("Organizer not found", value);
            _serviceMock.Verify(s => s.DeleteOrganizer(organizerId), Times.Once);
        }


        [Fact]
        public async Task UpdateOrganizer_ShouldReturnOkResult_WhenOrganizerIsUpdated()
        {
            // Arrange
            var organizerId = 1;
            var organizerDto = new OrganizerRequestDTO { Name = "Updated Organizer", Phone = "1234567890" };

            // Act
            var result = await _controller.UpdateOrganizer(organizerDto, organizerId);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.UpdateOrganizer(organizerDto, organizerId), Times.Once);
        }

        [Fact]
        public async Task UpdateOrganizer_ShouldThrowArgumentNullException_WhenOrganizerDtoIsNull()
        {
            // Arrange
            OrganizerRequestDTO organizerDto = null;
            var organizerId = 1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _controller.UpdateOrganizer(organizerDto, organizerId));
            _serviceMock.Verify(s => s.UpdateOrganizer(It.IsAny<OrganizerRequestDTO>(), It.IsAny<int>()), Times.Never);
        }


        [Fact]
        public async Task UpdateOrganizer_ShouldReturnNotFound_WhenOrganizerDoesNotExist()
        {
            // Arrange
            var organizerId = 1;
            var organizerDto = new OrganizerRequestDTO { Name = "Updated Organizer", Phone = "1234567890" };

            _serviceMock.Setup(s => s.UpdateOrganizer(organizerDto, organizerId)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.UpdateOrganizer(organizerDto, organizerId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var value = notFoundResult.Value.GetType().GetProperty("message").GetValue(notFoundResult.Value, null);
            Assert.Equal("Organizer not found", value);
            _serviceMock.Verify(s => s.UpdateOrganizer(organizerDto, organizerId), Times.Once);
        }
    }
}
