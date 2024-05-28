using Xunit;
using Moq;
using System.Threading.Tasks;
using EventManagementTests.Services.Interfaces;
using EventManagementTests.Services.Implementations;
using EventManagementTests.DTOs;
using System;
using EventManagementTests.Repositories.Interfaces;
using AutoMapper;
using EventManagementTests.Models;

namespace EventManagementTests
{
    public class OrganizerServiceTests
    {
        private readonly Mock<IOrganizerRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly OrganizerService _service;


        public OrganizerServiceTests()
        {
            _repositoryMock = new Mock<IOrganizerRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new OrganizerService(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateOrganizer_ShouldCallRepositoryAddOrganizer_WhenOrganizerDtoIsValid()
        {
            // Arrange
            var organizerDto = new OrganizerRequestDTO { Name = "Organizer Name", Phone = "1234567890" };

            // Act
            await _service.CreateOrganizer(organizerDto);

            // Assert
            _repositoryMock.Verify(r => r.AddOrganizer(organizerDto), Times.Once);
        }

        [Fact]
        public async Task CreateOrganizer_ShouldThrowArgumentNullException_WhenOrganizerDtoIsNull()
        {
            // Arrange
            OrganizerRequestDTO organizerDto = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateOrganizer(organizerDto));
            _repositoryMock.Verify(r => r.AddOrganizer(It.IsAny<OrganizerRequestDTO>()), Times.Never);
        }

        [Fact]
        public async Task CreateOrganizer_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            var organizerDto = new OrganizerRequestDTO { Name = "Organizer Name", Phone = "1234567890" };

            _repositoryMock.Setup(r => r.AddOrganizer(organizerDto)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.CreateOrganizer(organizerDto));
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.AddOrganizer(organizerDto), Times.Once);
        }
        //metoda tjeter
        [Fact]
        public async Task DeleteOrganizer_ShouldCallRepositoryDeleteOrganizer_WhenOrganizerExists()
        {
            // Arrange
            var organizerId = 1;
            var organizerEntity = new Organizer { OrganizerId = organizerId, Name = "Organizer Name", Phone = "1234567890" };

            _repositoryMock.Setup(r => r.GetOrganizerById(organizerId)).ReturnsAsync(organizerEntity);

            // Act
            await _service.DeleteOrganizer(organizerId);

            // Assert
            _repositoryMock.Verify(r => r.GetOrganizerById(organizerId), Times.Once);
            _repositoryMock.Verify(r => r.DeleteOrganizer(organizerId), Times.Once);
        }

        [Fact]
        public async Task DeleteOrganizer_ShouldThrowKeyNotFoundException_WhenOrganizerDoesNotExist()
        {
            // Arrange
            var organizerId = 1;

            _repositoryMock.Setup(r => r.GetOrganizerById(organizerId)).ReturnsAsync((Organizer)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteOrganizer(organizerId));
            _repositoryMock.Verify(r => r.GetOrganizerById(organizerId), Times.Once);
            _repositoryMock.Verify(r => r.DeleteOrganizer(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteOrganizer_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            var organizerId = 1;
            var organizerEntity = new Organizer { OrganizerId = organizerId, Name = "Organizer Name", Phone = "1234567890" };

            _repositoryMock.Setup(r => r.GetOrganizerById(organizerId)).ReturnsAsync(organizerEntity);
            _repositoryMock.Setup(r => r.DeleteOrganizer(organizerId)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.DeleteOrganizer(organizerId));
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.GetOrganizerById(organizerId), Times.Once);
            _repositoryMock.Verify(r => r.DeleteOrganizer(organizerId), Times.Once);
        }
        //metoda tjeter
        [Fact]
        public async Task GetAllOrganizers_ShouldReturnOrganizers_WhenOrganizersExist()
        {
            // Arrange
            var organizers = new List<Organizer>
            {
                new Organizer { OrganizerId = 1, Name = "Organizer 1", Phone = "1234567890" },
                new Organizer { OrganizerId = 2, Name = "Organizer 2", Phone = "0987654321" }
            };

            var organizerDtos = new List<OrganizerDTO>
            {
                new OrganizerDTO { OrganizerId = 1, Name = "Organizer 1", Phone = "1234567890" },
                new OrganizerDTO { OrganizerId = 2, Name = "Organizer 2", Phone = "0987654321" }
            };

            _repositoryMock.Setup(r => r.GetAllOrganizers()).ReturnsAsync(organizers);
            _mapperMock.Setup(m => m.Map<IEnumerable<OrganizerDTO>>(organizers)).Returns(organizerDtos);

            // Act
            var result = await _service.GetAllOrganizers();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _repositoryMock.Verify(r => r.GetAllOrganizers(), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<OrganizerDTO>>(organizers), Times.Once);
        }

        [Fact]
        public async Task GetAllOrganizers_ShouldReturnEmptyList_WhenNoOrganizersExist()
        {
            // Arrange
            var organizers = new List<Organizer>();
            var organizerDtos = new List<OrganizerDTO>();

            _repositoryMock.Setup(r => r.GetAllOrganizers()).ReturnsAsync(organizers);
            _mapperMock.Setup(m => m.Map<IEnumerable<OrganizerDTO>>(organizers)).Returns(organizerDtos);

            // Act
            var result = await _service.GetAllOrganizers();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _repositoryMock.Verify(r => r.GetAllOrganizers(), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<OrganizerDTO>>(organizers), Times.Once);
        }

        [Fact]
        public async Task GetAllOrganizers_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAllOrganizers()).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.GetAllOrganizers());
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.GetAllOrganizers(), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<OrganizerDTO>>(It.IsAny<IEnumerable<Organizer>>()), Times.Never);
        }
        //metoda tjeter
        [Fact]
        public async Task GetOrganizerById_ShouldReturnOrganizer_WhenOrganizerExists()
        {
            // Arrange
            var organizerId = 1;
            var organizerEntity = new Organizer { OrganizerId = organizerId, Name = "Organizer Name", Phone = "1234567890" };
            var organizerDto = new OrganizerDTO { OrganizerId = organizerId, Name = "Organizer Name", Phone = "1234567890" };

            _repositoryMock.Setup(r => r.GetOrganizerById(organizerId)).ReturnsAsync(organizerEntity);
            _mapperMock.Setup(m => m.Map<OrganizerDTO>(organizerEntity)).Returns(organizerDto);

            // Act
            var result = await _service.GetOrganizerById(organizerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(organizerId, result.OrganizerId);
            Assert.Equal("Organizer Name", result.Name);
            _repositoryMock.Verify(r => r.GetOrganizerById(organizerId), Times.Once);
            _mapperMock.Verify(m => m.Map<OrganizerDTO>(organizerEntity), Times.Once);
        }

        [Fact]
        public async Task GetOrganizerById_ShouldThrowKeyNotFoundException_WhenOrganizerDoesNotExist()
        {
            // Arrange
            var organizerId = 1;

            _repositoryMock.Setup(r => r.GetOrganizerById(organizerId)).ReturnsAsync((Organizer)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetOrganizerById(organizerId));
            _repositoryMock.Verify(r => r.GetOrganizerById(organizerId), Times.Once);
            _mapperMock.Verify(m => m.Map<OrganizerDTO>(It.IsAny<Organizer>()), Times.Never);
        }

        [Fact]
        public async Task GetOrganizerById_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            var organizerId = 1;

            _repositoryMock.Setup(r => r.GetOrganizerById(organizerId)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.GetOrganizerById(organizerId));
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.GetOrganizerById(organizerId), Times.Once);
            _mapperMock.Verify(m => m.Map<OrganizerDTO>(It.IsAny<Organizer>()), Times.Never);
        }
        //metoda tjeter
        [Fact]
        public async Task UpdateOrganizer_ShouldCallRepositoryUpdateOrganizer_WhenOrganizerDtoIsValid()
        {
            // Arrange
            var organizerId = 1;
            var organizerDto = new OrganizerRequestDTO { Name = "Updated Organizer", Phone = "0987654321" };
            var existingOrganizer = new Organizer { OrganizerId = organizerId, Name = "Existing Organizer", Phone = "1234567890" };

            _repositoryMock.Setup(r => r.GetOrganizerById(organizerId)).ReturnsAsync(existingOrganizer);

            // Act
            await _service.UpdateOrganizer(organizerDto, organizerId);

            // Assert
            _repositoryMock.Verify(r => r.GetOrganizerById(organizerId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateOrganizer(organizerDto, organizerId), Times.Once);
        }

        [Fact]
        public async Task UpdateOrganizer_ShouldThrowArgumentNullException_WhenOrganizerDtoIsNull()
        {
            // Arrange
            OrganizerRequestDTO organizerDto = null;
            var organizerId = 1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateOrganizer(organizerDto, organizerId));
            _repositoryMock.Verify(r => r.GetOrganizerById(It.IsAny<int>()), Times.Never);
            _repositoryMock.Verify(r => r.UpdateOrganizer(It.IsAny<OrganizerRequestDTO>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task UpdateOrganizer_ShouldThrowKeyNotFoundException_WhenOrganizerDoesNotExist()
        {
            // Arrange
            var organizerId = 1;
            var organizerDto = new OrganizerRequestDTO { Name = "Updated Organizer", Phone = "0987654321" };

            _repositoryMock.Setup(r => r.GetOrganizerById(organizerId)).ReturnsAsync((Organizer)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateOrganizer(organizerDto, organizerId));
            _repositoryMock.Verify(r => r.GetOrganizerById(organizerId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateOrganizer(It.IsAny<OrganizerRequestDTO>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task UpdateOrganizer_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            var organizerId = 1;
            var organizerDto = new OrganizerRequestDTO { Name = "Updated Organizer", Phone = "0987654321" };
            var existingOrganizer = new Organizer { OrganizerId = organizerId, Name = "Existing Organizer", Phone = "1234567890" };

            _repositoryMock.Setup(r => r.GetOrganizerById(organizerId)).ReturnsAsync(existingOrganizer);
            _repositoryMock.Setup(r => r.UpdateOrganizer(organizerDto, organizerId)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.UpdateOrganizer(organizerDto, organizerId));
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.GetOrganizerById(organizerId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateOrganizer(organizerDto, organizerId), Times.Once);
        }
    }
}
