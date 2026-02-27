using DealService.Application.Common.Interfaces;
//using DealService.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;

namespace DealService.Application.Command
{
    public class UpdateDealStatusHandler : IRequestHandler<UpdateDealStatus, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateDealStatusHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Handle(UpdateDealStatus request, CancellationToken cancellationToken)
        {
            var deal = await _context.Deals.FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);
            if (deal == null) return false;
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (currentUserId != deal.UserId) return false;

            deal.ChangeStatus(request.Status);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
