using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DealService.Application.Command
{
    public record UpdateDealCommand(Guid Id, string Title, decimal Amount) : IRequest<bool>;

}
