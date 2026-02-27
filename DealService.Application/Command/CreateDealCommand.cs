using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace DealService.Application.Command
{
    public record CreateDealCommand(string Title, decimal Amount) : IRequest<Guid>;
}
