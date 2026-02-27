using DealService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DealService.Application.Command
{
    public record UpdateDealStatus(Guid Id, DealStatus Status) : IRequest<bool>; 
}
