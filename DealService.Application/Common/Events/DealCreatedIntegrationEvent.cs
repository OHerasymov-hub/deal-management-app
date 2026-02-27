using System;
using System.Collections.Generic;
using System.Text;

namespace DealService.Application.Common.Events
{
    public record DealCreatedIntegrationEvent(Guid Id, string Title, decimal Amount, string CreatedAt, string UserId);
}
