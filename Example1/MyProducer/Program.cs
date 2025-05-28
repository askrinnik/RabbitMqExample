using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "askrwin10" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

const string queueName = "myQueue";

await channel.QueueDeclareAsync(
    queue: queueName,
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null);

for (var i = 0; i < 10; i++)
{
    var message = $"{DateTime.UtcNow} {Guid.CreateVersion7()}";
    var body = Encoding.UTF8.GetBytes(message);
    await channel.BasicPublishAsync(
        exchange: string.Empty,
        routingKey: queueName,
        mandatory: true,
        basicProperties: new BasicProperties { Persistent = true },
        body: body);
    
    Console.WriteLine($"Sent: {message}");
    await Task.Delay(2000);
}