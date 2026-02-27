using DealService.Domain.Exceptions;

namespace DealService.Domain.Entities;

public enum DealStatus { New, InProgress, Closed }

public class Deal
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public decimal Amount { get; private set; }
    public DealStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string UserId { get; private set; }

    
    public Deal(string title, decimal amount, string userId)
    {
        Id = Guid.NewGuid();
        Title = title;
        Amount = amount;
        UserId = userId;
        Status = DealStatus.New;
        CreatedAt = DateTime.UtcNow;
    }

    public void ChangeStatus(DealStatus newStatus)
    {
        bool canChange = (Status, newStatus) switch
        {
            (DealStatus.New, DealStatus.InProgress) => true,
            (DealStatus.InProgress, DealStatus.Closed) => true,
            _ => false
        };
        if(!canChange)
            throw new InvalidStatusTransactionException(Status, newStatus);
        Status = newStatus;
    }

    public void UpdateDetails(string title, decimal amount)
    {
        if (Status == DealStatus.Closed)
            throw new InvalidOperationException("Cannot update a closed deal");
        Title = title;
        Amount = amount;
    }
}