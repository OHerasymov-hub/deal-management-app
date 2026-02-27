using DealService.Application.Common.Events;
using DealService.Application.Common.Interfaces;
using DealService.Domain.Entities;
//using DealService.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace DealService.Application.Command
{
    public class CreateDealHandler : IRequestHandler<CreateDealCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIntegrationEventPublisher _publisher;

        public CreateDealHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IIntegrationEventPublisher publisher)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _publisher = publisher;
        }

        public async Task<Guid> Handle(CreateDealCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var deal = new Deal(request.Title, request.Amount, userId);
            _context.Deals.Add(deal);
            await _context.SaveChangesAsync(cancellationToken);

            // Publish integration event
            var integrationEvent = new DealCreatedIntegrationEvent(deal.Id, deal.Title, deal.Amount, deal.CreatedAt.ToString(), userId);
            await _publisher.PublishAsync("deal-events", integrationEvent, cancellationToken);

            return deal.Id;
        }
    }
}
