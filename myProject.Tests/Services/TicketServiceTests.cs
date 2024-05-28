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
    public class TicketServiceTests
    {
        private readonly Mock<ITicketRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly TicketService _service;

        public TicketServiceTests()
        {
            _repositoryMock = new Mock<ITicketRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new TicketService(_repositoryMock.Object, _mapperMock.Object);
        }
        [Fact]
        public async Task AddTicket_ShouldCallRepositoryAddTicket_WhenTicketRequestDtoIsValid()
        {
            // Arrange
            var ticketRequestDto = new TicketRequestDTO
            {
                Price = 50.0M,
                TicketType = "VIP",
                QuantityAvailable = 100
            };

            // Act
            await _service.AddTicket(ticketRequestDto);

            // Assert
            _repositoryMock.Verify(r => r.AddTicket(ticketRequestDto), Times.Once);
        }

        [Fact]
        public async Task AddTicket_ShouldThrowArgumentNullException_WhenTicketRequestDtoIsNull()
        {
            // Arrange
            TicketRequestDTO ticketRequestDto = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.AddTicket(ticketRequestDto));
            _repositoryMock.Verify(r => r.AddTicket(It.IsAny<TicketRequestDTO>()), Times.Never);
        }

        [Fact]
        public async Task AddTicket_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            var ticketRequestDto = new TicketRequestDTO
            {
                Price = 50.0M,
                TicketType = "VIP",
                QuantityAvailable = 100
            };

            _repositoryMock.Setup(r => r.AddTicket(ticketRequestDto)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.AddTicket(ticketRequestDto));
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.AddTicket(ticketRequestDto), Times.Once);
        }
        //metoda tjeter
        [Fact]
        public async Task DeleteTicket_ShouldCallRepositoryDeleteTicket_WhenTicketExists()
        {
            // Arrange
            var ticketId = 1;
            var ticketEntity = new Ticket { TicketId = ticketId, Price = 50.0M, TicketType = "VIP", QuantityAvailable = 100 };

            _repositoryMock.Setup(r => r.GetTicketById(ticketId)).ReturnsAsync(ticketEntity);

            // Act
            await _service.DeleteTicket(ticketId);

            // Assert
            _repositoryMock.Verify(r => r.GetTicketById(ticketId), Times.Once);
            _repositoryMock.Verify(r => r.DeleteTicket(ticketId), Times.Once);
        }

        [Fact]
        public async Task DeleteTicket_ShouldThrowKeyNotFoundException_WhenTicketDoesNotExist()
        {
            // Arrange
            var ticketId = 1;

            _repositoryMock.Setup(r => r.GetTicketById(ticketId)).ReturnsAsync((Ticket)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteTicket(ticketId));
            _repositoryMock.Verify(r => r.GetTicketById(ticketId), Times.Once);
            _repositoryMock.Verify(r => r.DeleteTicket(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteTicket_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            var ticketId = 1;
            var ticketEntity = new Ticket { TicketId = ticketId, Price = 50.0M, TicketType = "VIP", QuantityAvailable = 100 };

            _repositoryMock.Setup(r => r.GetTicketById(ticketId)).ReturnsAsync(ticketEntity);
            _repositoryMock.Setup(r => r.DeleteTicket(ticketId)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.DeleteTicket(ticketId));
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.GetTicketById(ticketId), Times.Once);
            _repositoryMock.Verify(r => r.DeleteTicket(ticketId), Times.Once);
        }
        //metoda tjeter
        [Fact]
        public async Task GetTicketById_ShouldReturnTicket_WhenTicketExists()
        {
            // Arrange
            var ticketId = 1;
            var ticketEntity = new Ticket { TicketId = ticketId, Price = 50.0M, TicketType = "VIP", QuantityAvailable = 100 };
            var ticketDto = new TicketDTO { TicketId = ticketId, Price = 50.0M, TicketType = "VIP", QuantityAvailable = 100 };

            _repositoryMock.Setup(r => r.GetTicketById(ticketId)).ReturnsAsync(ticketEntity);
            _mapperMock.Setup(m => m.Map<TicketDTO>(ticketEntity)).Returns(ticketDto);

            // Act
            var result = await _service.GetTicketById(ticketId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ticketId, result.TicketId);
            Assert.Equal("VIP", result.TicketType);
            _repositoryMock.Verify(r => r.GetTicketById(ticketId), Times.Once);
            _mapperMock.Verify(m => m.Map<TicketDTO>(ticketEntity), Times.Once);
        }

        [Fact]
        public async Task GetTicketById_ShouldThrowKeyNotFoundException_WhenTicketDoesNotExist()
        {
            // Arrange
            var ticketId = 1;

            _repositoryMock.Setup(r => r.GetTicketById(ticketId)).ReturnsAsync((Ticket)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetTicketById(ticketId));
            _repositoryMock.Verify(r => r.GetTicketById(ticketId), Times.Once);
            _mapperMock.Verify(m => m.Map<TicketDTO>(It.IsAny<Ticket>()), Times.Never);
        }

        [Fact]
        public async Task GetTicketById_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            var ticketId = 1;

            _repositoryMock.Setup(r => r.GetTicketById(ticketId)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.GetTicketById(ticketId));
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.GetTicketById(ticketId), Times.Once);
            _mapperMock.Verify(m => m.Map<TicketDTO>(It.IsAny<Ticket>()), Times.Never);
        }
        //metoda tjeter
        [Fact]
        public async Task SellTicket_ShouldCallRepositoryUpdateTicket_WhenTicketExistsAndQuantityIsValid()
        {
            // Arrange
            var ticketId = 1;
            var quantity = 2;
            var ticketEntity = new Ticket { TicketId = ticketId, QuantityAvailable = 5, Price = 50.0M, TicketType = "VIP" };
            var updatedTicketDto = new TicketRequestDTO { QuantityAvailable = 3, Price = 50.0M, TicketType = "VIP" };

            _repositoryMock.Setup(r => r.GetTicketById(ticketId)).ReturnsAsync(ticketEntity);
            _mapperMock.Setup(m => m.Map<TicketRequestDTO>(ticketEntity)).Returns(updatedTicketDto);

            // Act
            await _service.SellTicket(ticketId, quantity);

            // Assert
            _repositoryMock.Verify(r => r.GetTicketById(ticketId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateTicket(updatedTicketDto, ticketId), Times.Once);
            _mapperMock.Verify(m => m.Map<TicketRequestDTO>(ticketEntity), Times.Once);
        }

        [Fact]
        public async Task SellTicket_ShouldThrowArgumentException_WhenQuantityIsLessThanOrEqualToZero()
        {
            // Arrange
            var ticketId = 1;
            var quantity = 0;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.SellTicket(ticketId, quantity));
            _repositoryMock.Verify(r => r.GetTicketById(It.IsAny<int>()), Times.Never);
            _repositoryMock.Verify(r => r.UpdateTicket(It.IsAny<TicketRequestDTO>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task SellTicket_ShouldThrowKeyNotFoundException_WhenTicketDoesNotExist()
        {
            // Arrange
            var ticketId = 1;
            var quantity = 2;

            _repositoryMock.Setup(r => r.GetTicketById(ticketId)).ReturnsAsync((Ticket)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.SellTicket(ticketId, quantity));
            _repositoryMock.Verify(r => r.GetTicketById(ticketId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateTicket(It.IsAny<TicketRequestDTO>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task SellTicket_ShouldThrowInvalidOperationException_WhenQuantityExceedsAvailableTickets()
        {
            // Arrange
            var ticketId = 1;
            var quantity = 6;
            var ticketEntity = new Ticket { TicketId = ticketId, QuantityAvailable = 5, Price = 50.0M, TicketType = "VIP" };

            _repositoryMock.Setup(r => r.GetTicketById(ticketId)).ReturnsAsync(ticketEntity);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.SellTicket(ticketId, quantity));
            _repositoryMock.Verify(r => r.GetTicketById(ticketId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateTicket(It.IsAny<TicketRequestDTO>(), It.IsAny<int>()), Times.Never);
        }
        [Fact]
        public async Task SellTicket_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            var ticketId = 1;
            var quantity = 2;
            var ticketEntity = new Ticket
            {
                TicketId = ticketId,
                QuantityAvailable = 5,
                Price = 50.0M,
                TicketType = "VIP"
            };
            var updatedTicketDto = new TicketRequestDTO
            {
                QuantityAvailable = 3,
                Price = 50.0M,
                TicketType = "VIP"
            };

            // Set up the repository mock to return the ticket entity when GetTicketById is called
            _repositoryMock.Setup(r => r.GetTicketById(ticketId)).ReturnsAsync(ticketEntity);

            // Set up the mapper mock to map the ticket entity to the DTO
            _mapperMock.Setup(m => m.Map<TicketRequestDTO>(ticketEntity)).Returns(updatedTicketDto);

            // Set up the repository mock to throw an exception when UpdateTicket is called
            _repositoryMock.Setup(r => r.UpdateTicket(updatedTicketDto, ticketId)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.SellTicket(ticketId, quantity));
            Assert.Equal("Database error", exception.Message);

            // Verify that the methods were called the expected number of times
            _repositoryMock.Verify(r => r.GetTicketById(ticketId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateTicket(updatedTicketDto, ticketId), Times.Once);
            _mapperMock.Verify(m => m.Map<TicketRequestDTO>(ticketEntity), Times.Once);
        }



        //metoda tjeter
        [Fact]
        public async Task RefundTicket_ShouldCallRepositoryUpdateTicket_WhenTicketExistsAndQuantityIsValid()
        {
            // Arrange
            var ticketId = 1;
            var quantity = 2;
            var ticketEntity = new Ticket { TicketId = ticketId, QuantityAvailable = 5, Price = 50.0M, TicketType = "VIP" };
            var updatedTicketDto = new TicketRequestDTO { QuantityAvailable = 7, Price = 50.0M, TicketType = "VIP" };

            _repositoryMock.Setup(r => r.GetTicketById(ticketId)).ReturnsAsync(ticketEntity);
            _mapperMock.Setup(m => m.Map<TicketRequestDTO>(ticketEntity)).Returns(updatedTicketDto);

            // Act
            await _service.RefundTicket(ticketId, quantity);

            // Assert
            _repositoryMock.Verify(r => r.GetTicketById(ticketId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateTicket(updatedTicketDto, ticketId), Times.Once);
            _mapperMock.Verify(m => m.Map<TicketRequestDTO>(ticketEntity), Times.Once);
        }

        [Fact]
        public async Task RefundTicket_ShouldThrowArgumentException_WhenQuantityIsLessThanOrEqualToZero()
        {
            // Arrange
            var ticketId = 1;
            var quantity = 0;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.RefundTicket(ticketId, quantity));
            _repositoryMock.Verify(r => r.GetTicketById(It.IsAny<int>()), Times.Never);
            _repositoryMock.Verify(r => r.UpdateTicket(It.IsAny<TicketRequestDTO>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task RefundTicket_ShouldThrowKeyNotFoundException_WhenTicketDoesNotExist()
        {
            // Arrange
            var ticketId = 1;
            var quantity = 2;

            _repositoryMock.Setup(r => r.GetTicketById(ticketId)).ReturnsAsync((Ticket)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.RefundTicket(ticketId, quantity));
            _repositoryMock.Verify(r => r.GetTicketById(ticketId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateTicket(It.IsAny<TicketRequestDTO>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task RefundTicket_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            var ticketId = 1;
            var quantity = 2;
            var ticketEntity = new Ticket
            {
                TicketId = ticketId,
                QuantityAvailable = 5,
                Price = 50.0M,
                TicketType = "VIP"
            };
            var updatedTicketDto = new TicketRequestDTO
            {
                QuantityAvailable = 7, // Quantity after refund
                Price = 50.0M,
                TicketType = "VIP"
            };

            // Mock the repository and mapper behavior
            _repositoryMock.Setup(r => r.GetTicketById(ticketId)).ReturnsAsync(ticketEntity);
            _mapperMock.Setup(m => m.Map<TicketRequestDTO>(ticketEntity)).Returns(updatedTicketDto);
            _repositoryMock.Setup(r => r.UpdateTicket(updatedTicketDto, ticketId)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.RefundTicket(ticketId, quantity));
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.GetTicketById(ticketId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateTicket(updatedTicketDto, ticketId), Times.Once);
            _mapperMock.Verify(m => m.Map<TicketRequestDTO>(ticketEntity), Times.Once);
        }

        //metoda tjeter
        [Fact]
        public async Task UpdateTicket_ShouldCallRepositoryUpdateTicket_WhenTicketExistsAndDtoIsValid()
        {
            // Arrange
            var ticketId = 1;
            var ticketDto = new TicketRequestDTO { Price = 100.0M, TicketType = "Regular", QuantityAvailable = 50 };
            var ticketEntity = new Ticket { TicketId = ticketId, QuantityAvailable = 30, Price = 80.0M, TicketType = "Regular" };

            _repositoryMock.Setup(r => r.GetTicketById(ticketId)).ReturnsAsync(ticketEntity);
            _mapperMock.Setup(m => m.Map<TicketRequestDTO>(ticketDto)).Returns(ticketDto);

            // Act
            await _service.UpdateTicket(ticketDto, ticketId);

            // Assert
            _repositoryMock.Verify(r => r.GetTicketById(ticketId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateTicket(ticketDto, ticketId), Times.Once);
            _mapperMock.Verify(m => m.Map<TicketRequestDTO>(ticketDto), Times.Once);
        }

        [Fact]
        public async Task UpdateTicket_ShouldThrowKeyNotFoundException_WhenTicketDoesNotExist()
        {
            // Arrange
            var ticketId = 1;
            var ticketDto = new TicketRequestDTO { Price = 100.0M, TicketType = "Regular", QuantityAvailable = 50 };

            _repositoryMock.Setup(r => r.GetTicketById(ticketId)).ReturnsAsync((Ticket)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateTicket(ticketDto, ticketId));
            _repositoryMock.Verify(r => r.GetTicketById(ticketId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateTicket(It.IsAny<TicketRequestDTO>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task UpdateTicket_ShouldPropagateException_WhenMapperThrowsException()
        {
            // Arrange
            var ticketId = 1;
            var ticketDto = new TicketRequestDTO { Price = 100.0M, TicketType = "Regular", QuantityAvailable = 50 };
            var ticketEntity = new Ticket { TicketId = ticketId, QuantityAvailable = 30, Price = 80.0M, TicketType = "Regular" };

            _repositoryMock.Setup(r => r.GetTicketById(ticketId)).ReturnsAsync(ticketEntity);
            _mapperMock.Setup(m => m.Map<TicketRequestDTO>(ticketDto)).Throws(new Exception("Mapping error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.UpdateTicket(ticketDto, ticketId));
            Assert.Equal("Mapping error", exception.Message);
            _repositoryMock.Verify(r => r.GetTicketById(ticketId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateTicket(It.IsAny<TicketRequestDTO>(), It.IsAny<int>()), Times.Never);
        }
    }
}
