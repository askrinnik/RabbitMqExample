using System.Text;
using RabbitMQ.Client;

const string hostName = "askrwin10";

var factory = new ConnectionFactory { HostName = hostName };
await using var connection = await factory.CreateConnectionAsync();
await using var channel = await connection.CreateChannelAsync();

const string exchangeName = "myExchange";

await channel.ExchangeDeclareAsync(
    exchange: exchangeName,
    durable: true,
    autoDelete: false,
    type: ExchangeType.Fanout);

for (var i = 0; i < 10; i++)
{
    var message = $"{DateTime.UtcNow} {Guid.CreateVersion7()}";
    var body = Encoding.UTF8.GetBytes(message);
    await channel.BasicPublishAsync(
        exchange: exchangeName,
        routingKey: string.Empty, 
        mandatory: true,
        basicProperties: new BasicProperties { Persistent = true },
        body: body);
    
    Console.WriteLine($"Sent: {message}");
    await Task.Delay(2000);
}