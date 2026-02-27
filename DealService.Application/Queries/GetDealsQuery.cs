using DealService.Application.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DealService.Application.Queries
{
    

    public record GetDealsQuery() : IRequest<List<DealDto>>;
}
