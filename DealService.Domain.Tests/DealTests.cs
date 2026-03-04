using DealService.Domain.Entities;
using DealService.Domain.Exceptions;
using System.Runtime.CompilerServices;

namespace DealService.Domain.Tests
{
    public class DealTests
    {
        [Fact]
        public void ChangeStatus_fromNewToInProgress_ShouldSucceed()
        {
            var deal = new Deal("Test deal", 100, "user-123");
            deal.ChangeStatus(DealStatus.InProgress);
            Assert.Equal(DealStatus.InProgress, deal.Status);
        }

        [Theory]
        [InlineData(DealStatus.New)]
        [InlineData(DealStatus.InProgress)]
        public void UpdateDetails_WhenDealIsActive_ShouldUpdateFields(DealStatus currentStatus)
        {
            // Arrange
            var deal = new Deal("Old Title", 100, "user-1");
            if (currentStatus == DealStatus.InProgress) deal.ChangeStatus(DealStatus.InProgress);

            // Act
            deal.UpdateDetails("New Title", 200);

            // Assert
            Assert.Equal("New Title", deal.Title);
            Assert.Equal(200, deal.Amount);
        }

        [Fact]
        public void UpdateDetails_WhenDealIsClosed_ShouldThrowException()
        {
            // Arrange
            var deal = new Deal("Test", 100, "user-1");
            deal.ChangeStatus(DealStatus.InProgress);
            deal.ChangeStatus(DealStatus.Closed);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                deal.UpdateDetails("Try Update", 999));
        }

        [Theory]
        [InlineData(DealStatus.New, DealStatus.Closed)]
        [InlineData(DealStatus.Closed, DealStatus.New)]
        [InlineData(DealStatus.Closed, DealStatus.InProgress)]
        [InlineData(DealStatus.InProgress, DealStatus.New)]
        [InlineData(DealStatus.New, DealStatus.New)]
        [InlineData(DealStatus.InProgress, DealStatus.InProgress)]
        [InlineData(DealStatus.Closed, DealStatus.Closed)]
        public void ChangeStatus_InvalidTransitions_ShouldThrowException(DealStatus initial, DealStatus target)
        {
            var deal = new Deal("Test deal", 100, "user-123");
            if(initial == DealStatus.InProgress)
            {
                deal.ChangeStatus(DealStatus.InProgress);
            }
            else if(initial == DealStatus.Closed)
            {
                deal.ChangeStatus(DealStatus.InProgress);
                deal.ChangeStatus(DealStatus.Closed);
            }
            Assert.Throws<InvalidStatusTransactionException>(() => deal.ChangeStatus(target));
        }
    }
}
