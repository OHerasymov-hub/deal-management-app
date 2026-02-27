using System;
using System.Collections.Generic;
using System.Text;

namespace DealService.Application.Dto
{
    public record DealDto(Guid Id, string Title, decimal Amount, string Status, string CreatedAt, string UserId);
}
