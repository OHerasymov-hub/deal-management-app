using System;
using System.Collections.Generic;
using System.Text;

namespace DealService.Application.Common.Interfaces
{
    public interface IIntegrationEventPublisher
    {
        Task PublishAsync<T>(string topic, T @event, CancellationToken cancellationToken = default) where T: class;
    }
}
