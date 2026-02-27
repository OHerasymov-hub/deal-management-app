using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using DealService.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DealService.Infrastructure.Messaging
{
    public class KafkaIntegrationEventPublisher : IIntegrationEventPublisher
    {
        private readonly IProducer<Null, string> _producer;

        public KafkaIntegrationEventPublisher(IConfiguration configuration)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                SocketTimeoutMs = 5000,
                MessageTimeoutMs = 5000,
                Acks = Acks.All
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }
        public async Task PublishAsync<T>(string topic, T @event, CancellationToken cancellationToken = default) where T : class
        {
            var message = new Message<Null, string>
            {
                Value = JsonSerializer.Serialize(@event)
            };

            await _producer.ProduceAsync(topic, message, cancellationToken);
        }
    }
}
