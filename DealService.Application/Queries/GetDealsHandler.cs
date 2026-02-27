using DealService.Application.Common.Interfaces;
using DealService.Application.Dto;
//using DealService.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace DealService.Application.Queries
{
    public class GetDealsHandler : IRequestHandler<GetDealsQuery, List<DealDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetDealsHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<DealDto>> Handle(GetDealsQuery request, CancellationToken cancellationToken)
        {
            //Task.Delay(1000, cancellationToken);
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _context.Deals.Where(d => d.UserId == userId)
                .Select(d => new DealDto(d.Id, d.Title, d.Amount, d.Status.ToString(), d.CreatedAt.ToString(), userId ?? "")).ToListAsync<DealDto>(cancellationToken);
        }
    }
}
