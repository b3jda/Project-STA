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
    public class TicketControllerTests
    {
        private readonly Mock<ITicketService> _serviceMock;
        private readonly TicketController _controller;

        public TicketControllerTests()
        {
            _serviceMock = new Mock<ITicketService>();
            _controller = new TicketController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetTicketById_ShouldReturnOkResult_WithTicket_WhenTicketExists()
        {
            // Arrange
            var ticketId = 1;
            var ticketDto = new TicketDTO { TicketId = ticketId, Price = 100, TicketType = "VIP", QuantityAvailable = 50 };

            _serviceMock.Setup(s => s.GetTicketById(ticketId)).ReturnsAsync(ticketDto);

            // Act
            var result = await _controller.GetTicketById(ticketId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<TicketDTO>(okResult.Value);
            Assert.Equal(ticketId, returnValue.TicketId);
            _serviceMock.Verify(s => s.GetTicketById(ticketId), Times.Once);
        }

        [Fact]
        public async Task GetTicketById_ShouldReturnNotFound_WhenTicketDoesNotExist()
        {
            // Arrange
            var ticketId = 1;

            _serviceMock.Setup(s => s.GetTicketById(ticketId)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.GetTicketById(ticketId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var value = notFoundResult.Value.GetType().GetProperty("message").GetValue(notFoundResult.Value, null);
            Assert.Equal("Ticket not found", value);
            _serviceMock.Verify(s => s.GetTicketById(ticketId), Times.Once);
        }

        [Fact]
        public async Task GetTicketById_ShouldReturnInternalServerError_WhenUnhandledExceptionOccurs()
        {
            // Arrange
            var ticketId = 1;

            _serviceMock.Setup(s => s.GetTicketById(ticketId)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetTicketById(ticketId);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);
            Assert.Equal("Unexpected error", internalServerErrorResult.Value);
            _serviceMock.Verify(s => s.GetTicketById(ticketId), Times.Once);
        }

        [Fact]
        public async Task AddTicket_ShouldReturnOkResult_WhenTicketIsAdded()
        {
            // Arrange
            var ticketDto = new TicketRequestDTO { Price = 100, TicketType = "VIP", QuantityAvailable = 50 };

            // Act
            var result = await _controller.AddTicket(ticketDto);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.AddTicket(ticketDto), Times.Once);
        }

        [Fact]
        public async Task AddTicket_ShouldReturnBadRequest_WhenTicketDtoIsNull()
        {
            // Arrange
            TicketRequestDTO ticketDto = null;

            // Act
            var result = await _controller.AddTicket(ticketDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var value = badRequestResult.Value.GetType().GetProperty("message").GetValue(badRequestResult.Value, null);
            Assert.Equal("TicketRequestDTO cannot be null", value);
            _serviceMock.Verify(s => s.AddTicket(It.IsAny<TicketRequestDTO>()), Times.Never);
        }

        [Fact]
        public async Task AddTicket_ShouldReturnInternalServerError_WhenUnhandledExceptionOccurs()
        {
            // Arrange
            var ticketDto = new TicketRequestDTO { Price = 100, TicketType = "VIP", QuantityAvailable = 50 };

            _serviceMock.Setup(s => s.AddTicket(ticketDto)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.AddTicket(ticketDto);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);
            Assert.Equal("Unexpected error", internalServerErrorResult.Value);
            _serviceMock.Verify(s => s.AddTicket(ticketDto), Times.Once);
        }

        [Fact]
        public async Task DeleteTicket_ShouldReturnOkResult_WhenTicketIsDeleted()
        {
            // Arrange
            var ticketId = 1;

            // Act
            var result = await _controller.DeleteTicket(ticketId);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.DeleteTicket(ticketId), Times.Once);
        }

        [Fact]
        public async Task DeleteTicket_ShouldReturnNotFound_WhenTicketDoesNotExist()
        {
            // Arrange
            var ticketId = 1;

            _serviceMock.Setup(s => s.DeleteTicket(ticketId)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.DeleteTicket(ticketId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var value = notFoundResult.Value.GetType().GetProperty("message").GetValue(notFoundResult.Value, null);
            Assert.Equal("Ticket not found", value);
            _serviceMock.Verify(s => s.DeleteTicket(ticketId), Times.Once);
        }

        [Fact]
        public async Task DeleteTicket_ShouldReturnInternalServerError_WhenUnhandledExceptionOccurs()
        {
            // Arrange
            var ticketId = 1;

            _serviceMock.Setup(s => s.DeleteTicket(ticketId)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.DeleteTicket(ticketId);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);
            Assert.Equal("Unexpected error", internalServerErrorResult.Value);
            _serviceMock.Verify(s => s.DeleteTicket(ticketId), Times.Once);
        }

        [Fact]
        public async Task SellTicket_ShouldReturnOkResult_WhenTicketIsSold()
        {
            // Arrange
            var ticketId = 1;
            var quantity = 5;

            // Act
            var result = await _controller.SellTicket(ticketId, quantity);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.SellTicket(ticketId, quantity), Times.Once);
        }

        [Fact]
        public async Task SellTicket_ShouldReturnNotFound_WhenTicketDoesNotExist()
        {
            // Arrange
            var ticketId = 1;
            var quantity = 5;

            _serviceMock.Setup(s => s.SellTicket(ticketId, quantity)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.SellTicket(ticketId, quantity);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var value = notFoundResult.Value.GetType().GetProperty("message").GetValue(notFoundResult.Value, null);
            Assert.Equal("Ticket not found", value);
            _serviceMock.Verify(s => s.SellTicket(ticketId, quantity), Times.Once);
        }

        [Fact]
        public async Task SellTicket_ShouldReturnBadRequest_WhenQuantityIsInvalid()
        {
            // Arrange
            var ticketId = 1;
            var quantity = -1;

            _serviceMock.Setup(s => s.SellTicket(ticketId, quantity)).ThrowsAsync(new ArgumentException("Quantity must be greater than zero"));

            // Act
            var result = await _controller.SellTicket(ticketId, quantity);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var value = badRequestResult.Value.GetType().GetProperty("message").GetValue(badRequestResult.Value, null);
            Assert.Equal("Quantity must be greater than zero", value);
            _serviceMock.Verify(s => s.SellTicket(ticketId, quantity), Times.Once);
        }

        

        [Fact]
        public async Task RefundTicket_ShouldReturnOkResult_WhenTicketIsRefunded()
        {
            // Arrange
            var ticketId = 1;
            var quantity = 5;

            // Act
            var result = await _controller.RefundTicket(ticketId, quantity);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.RefundTicket(ticketId, quantity), Times.Once);
        }

        [Fact]
        public async Task RefundTicket_ShouldReturnNotFound_WhenTicketDoesNotExist()
        {
            // Arrange
            var ticketId = 1;
            var quantity = 5;

            _serviceMock.Setup(s => s.RefundTicket(ticketId, quantity)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.RefundTicket(ticketId, quantity);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var value = notFoundResult.Value.GetType().GetProperty("message").GetValue(notFoundResult.Value, null);
            Assert.Equal("Ticket not found", value);
            _serviceMock.Verify(s => s.RefundTicket(ticketId, quantity), Times.Once);
        }

        [Fact]
        public async Task RefundTicket_ShouldReturnBadRequest_WhenQuantityIsInvalid()
        {
            // Arrange
            var ticketId = 1;
            var quantity = -1;

            _serviceMock.Setup(s => s.RefundTicket(ticketId, quantity)).ThrowsAsync(new ArgumentException("Quantity must be greater than zero"));

            // Act
            var result = await _controller.RefundTicket(ticketId, quantity);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var value = badRequestResult.Value.GetType().GetProperty("message").GetValue(badRequestResult.Value, null);
            Assert.Equal("Quantity must be greater than zero", value);
            _serviceMock.Verify(s => s.RefundTicket(ticketId, quantity), Times.Once);
        }

        [Fact]
        public async Task RefundTicket_ShouldReturnInternalServerError_WhenUnhandledExceptionOccurs()
        {
            // Arrange
            var ticketId = 1;
            var quantity = 5;

            _serviceMock.Setup(s => s.RefundTicket(ticketId, quantity)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.RefundTicket(ticketId, quantity);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);
            Assert.Equal("Unexpected error", internalServerErrorResult.Value);
            _serviceMock.Verify(s => s.RefundTicket(ticketId, quantity), Times.Once);
        }

        [Fact]
        public async Task UpdateTicket_ShouldReturnOkResult_WhenTicketIsUpdated()
        {
            // Arrange
            var ticketId = 1;
            var ticketDto = new TicketRequestDTO { Price = 100, TicketType = "VIP", QuantityAvailable = 50 };

            // Act
            var result = await _controller.UpdateTicket(ticketId, ticketDto);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.UpdateTicket(ticketDto, ticketId), Times.Once);
        }

        [Fact]
        public async Task UpdateTicket_ShouldReturnBadRequest_WhenTicketDtoIsNull()
        {
            // Arrange
            TicketRequestDTO ticketDto = null;
            var ticketId = 1;

            // Act
            var result = await _controller.UpdateTicket(ticketId, ticketDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var value = badRequestResult.Value.GetType().GetProperty("message").GetValue(badRequestResult.Value, null);
            Assert.Equal("TicketRequestDTO cannot be null", value);
            _serviceMock.Verify(s => s.UpdateTicket(It.IsAny<TicketRequestDTO>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task UpdateTicket_ShouldReturnNotFound_WhenTicketDoesNotExist()
        {
            // Arrange
            var ticketId = 1;
            var ticketDto = new TicketRequestDTO { Price = 100, TicketType = "VIP", QuantityAvailable = 50 };

            _serviceMock.Setup(s => s.UpdateTicket(ticketDto, ticketId)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.UpdateTicket(ticketId, ticketDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var value = notFoundResult.Value.GetType().GetProperty("message").GetValue(notFoundResult.Value, null);
            Assert.Equal("Ticket not found", value);
            _serviceMock.Verify(s => s.UpdateTicket(ticketDto, ticketId), Times.Once);
        }

        [Fact]
        public async Task UpdateTicket_ShouldReturnInternalServerError_WhenUnhandledExceptionOccurs()
        {
            // Arrange
            var ticketId = 1;
            var ticketDto = new TicketRequestDTO { Price = 100, TicketType = "VIP", QuantityAvailable = 50 };

            _serviceMock.Setup(s => s.UpdateTicket(ticketDto, ticketId)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.UpdateTicket(ticketId, ticketDto);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);
            Assert.Equal("Unexpected error", internalServerErrorResult.Value);
            _serviceMock.Verify(s => s.UpdateTicket(ticketDto, ticketId), Times.Once);
        }
    }
}
