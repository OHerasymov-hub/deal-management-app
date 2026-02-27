using Confluent.Kafka;
using System.Text.Json;

namespace NotificationService
{
    public record MessageDeal(Guid Id, string Title, decimal Amount, string Status, string CreatedAt, string UserId);

    public class Worker(ILogger<Worker> logger, IConfiguration configuration) : BackgroundService
    {
        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = configuration["Kafka:GroupId"], //unique for this service
                AutoOffsetReset = AutoOffsetReset.Earliest // read from the beginning
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe("deal-events");

            Console.WriteLine("---------Notification service started");

            while (!stoppingToken.IsCancellationRequested)
            {

                try
                {
                    logger.LogInformation("Waiting for new messages from Kafka...");
                    var result = consumer.Consume(stoppingToken);
                    Console.WriteLine($"\nNew Event: Message from Kafka:");
                    Console.WriteLine($"Message: {result.Message.Value}");
                    var value = JsonSerializer.Deserialize<MessageDeal>(result.Message.Value);
                    Console.WriteLine("-----------------------------------------");
                    Console.WriteLine($"Id: {value.Id}, UserId: {value.UserId}, Title: {value.Title}");
                    //Console.WriteLine($"Value: {result.Message}");
                    Console.WriteLine("-----------------------------------------");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception message: {ex.Message}");
                }
            }
        }
    }
}
