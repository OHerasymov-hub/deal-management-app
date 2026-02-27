using DealService.Application.Common.Interfaces;
//using DealService.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace DealService.Application.Command
{
    public class UpdateDealHandler : IRequestHandler<UpdateDealCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateDealHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> Handle(UpdateDealCommand request, CancellationToken cancellationToken)
        {
            var deal = await _context.Deals.FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);
            if (deal == null) return false;
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (deal.UserId != currentUserId) return false; // Ensure user can only

            deal.UpdateDetails(request.Title, request.Amount);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
