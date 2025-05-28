using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

const string hostName = "askrwin10";

var factory = new ConnectionFactory { HostName = hostName };
await using var connection = await factory.CreateConnectionAsync();
await using var channel = await connection.CreateChannelAsync();

const string exchangeName = "myExchange";

var queueName = args.FirstOrDefault("defaultQueue");

await channel.ExchangeDeclareAsync(
    exchange: exchangeName,
    durable: true,
    autoDelete: false,
    type: ExchangeType.Fanout);

await channel.QueueDeclareAsync(
    queue: queueName,
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null);

await channel.QueueBindAsync(queueName, exchangeName, string.Empty);

Console.WriteLine($"Waiting for messages in the {exchangeName}/{queueName}...");

var consumer = new AsyncEventingBasicConsumer (channel);
consumer.ReceivedAsync += async (sender, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Received: {message}");
    await ((AsyncEventingBasicConsumer)sender).Channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
};

await channel.BasicConsumeAsync(queueName, autoAck: false, consumer);
Console.ReadLine();