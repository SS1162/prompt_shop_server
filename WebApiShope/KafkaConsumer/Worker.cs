using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KafkaConsumer;

public sealed class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var bootstrapServers = _configuration["Kafka:BootstrapServers"]
            ?? throw new InvalidOperationException("Kafka:BootstrapServers is missing.");
        var topic = _configuration["Kafka:Topic"]
            ?? throw new InvalidOperationException("Kafka:Topic is missing.");
        var groupId = _configuration["Kafka:GroupId"] ?? "webapishope-kafka-consumer";

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true,
        };

        return Task.Run(() => ConsumeLoop(consumerConfig, topic, stoppingToken), stoppingToken);
    }

    private void ConsumeLoop(ConsumerConfig consumerConfig, string topic, CancellationToken stoppingToken)
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        consumer.Subscribe(topic);

        _logger.LogInformation("Kafka consumer started. Topic: {Topic}, BootstrapServers: {BootstrapServers}", topic, consumerConfig.BootstrapServers);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);
                    _logger.LogInformation("Kafka message received from {Topic}: {Message}", result.Topic, result.Message.Value);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Kafka consume error");
                }
            }
        }
        finally
        {
            consumer.Close();
        }
    }
}
