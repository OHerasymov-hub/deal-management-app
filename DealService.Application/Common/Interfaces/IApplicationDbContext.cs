using DealService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DealService.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Deal> Deals { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
