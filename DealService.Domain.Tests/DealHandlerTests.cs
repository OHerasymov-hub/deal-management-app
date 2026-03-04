using DealService.Application.Command;
using DealService.Application.Common.Events;
using DealService.Application.Common.Interfaces;
using DealService.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using MockQueryable.Moq;

namespace DealService.Domain.Tests
{
    public class DealHandlerTests
    {
        [Fact]
        public async Task CreateHandle_ValidCommand_ShouldSaveToDbAndPublishEvent()
        {
            // Arrange
            var dbContextMock = new Mock<IApplicationDbContext>();
            var publisherMock = new Mock<IIntegrationEventPublisher>();
            var httpAccessorMock = new Mock<IHttpContextAccessor>();
            var dealsDbSetMock = new Mock<DbSet<Deal>>();

            dbContextMock.Setup(x => x.Deals).Returns(dealsDbSetMock.Object);

            //fake user
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "test-user-id") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext { User = principal };
            httpAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            var handler = new CreateDealHandler(
                dbContextMock.Object,
                httpAccessorMock.Object,
                publisherMock.Object);

            var command = new CreateDealCommand("New Deal", 1000);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            // check that a new id returned
            Assert.NotEqual(Guid.Empty, result);
            //check if  handler called SaveChangesAsync once
            dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            // check if event published
            publisherMock.Verify(x => x.PublishAsync(
                "deal-events",
                It.IsAny<DealCreatedIntegrationEvent>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateHandle_ValidCommand_ShouldUpdateDeal()
        {
            // Arrange
            var dbContextMock = new Mock<IApplicationDbContext>();
            var httpAccessorMock = new Mock<IHttpContextAccessor>();

            var userId = "test-user-id";
            var existingDeal = new Deal("Old Title", 500, userId);

            var mockDbSet = new List<Deal> { existingDeal }
                //.AsQueryable()
                .BuildMockDbSet(); // Метод из MockQueryable

            dbContextMock.Setup(x => x.Deals).Returns(mockDbSet.Object);
            //fake user
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "test-user-id") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = principal };
            httpAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
            var handler = new UpdateDealHandler(
                dbContextMock.Object,
                httpAccessorMock.Object);
            var command = new UpdateDealCommand(existingDeal.Id, "Updated Deal", 1500);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.True(result);
            Assert.Equal("Updated Deal", existingDeal.Title);
            Assert.Equal(1500, existingDeal.Amount);
        }

        [Fact]
        public async Task UpdateHandle_WrongUser_ShouldNotUpdateDeal()
        {
            // Arrange
            var dbContextMock = new Mock<IApplicationDbContext>();
            var httpAccessorMock = new Mock<IHttpContextAccessor>();
            var existingDeal = new Deal("Old Title", 500, "owner-user-id");
            var mockDbSet = new List<Deal> { existingDeal }
                //.AsQueryable()
                .BuildMockDbSet(); // Метод из MockQueryable
            dbContextMock.Setup(x => x.Deals).Returns(mockDbSet.Object);
            //fake user
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "other-user-id") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = principal };
            httpAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
            var handler = new UpdateDealHandler(
                dbContextMock.Object,
                httpAccessorMock.Object);
            var command = new UpdateDealCommand(existingDeal.Id, "Updated Deal", 1500);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.False(result);
            Assert.Equal("Old Title", existingDeal.Title);
            Assert.Equal(500, existingDeal.Amount);
        }

        [Fact]
        public async Task UpdateStatusHandler_ValidCommand_ShouldUpdateStatus()
        {
            // Arrange
            var dbContextMock = new Mock<IApplicationDbContext>();
            var httpAccessorMock = new Mock<IHttpContextAccessor>();
            var userId = "test-user-id";
            var existingDeal = new Deal("Deal Title", 500, userId);
            var mockDbSet = new List<Deal> { existingDeal }
                //.AsQueryable()
                .BuildMockDbSet(); // Метод из MockQueryable
            dbContextMock.Setup(x => x.Deals).Returns(mockDbSet.Object);
            //fake user
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "test-user-id") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = principal };
            httpAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
            var handler = new UpdateDealStatusHandler(
                dbContextMock.Object,
                httpAccessorMock.Object);
            var command = new UpdateDealStatus(existingDeal.Id, DealStatus.InProgress);
            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            // Assert
            Assert.True(result);
            Assert.Equal(DealStatus.InProgress, existingDeal.Status);
        }

    }
}
