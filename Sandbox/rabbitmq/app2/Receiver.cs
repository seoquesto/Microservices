using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace app2
{
  class Program
  {
    static void Main(string[] args)
    {
      var factory = new ConnectionFactory() { HostName = "localhost" };
      using (var connection = factory.CreateConnection())
      using (var channel = connection.CreateModel())
      {
        channel.ExchangeDeclare("logs", ExchangeType.Direct);
        // var queueName = channel.QueueDeclare().QueueName;
        channel.QueueDeclare(queue: "adas", false, false, true);
        channel.QueueBind(queue: "adas",
                          exchange: "exchange",
                          routingKey: "info");
        channel.QueueBind(queue: "adas",
                          exchange: "exchange",
                          routingKey: "error");
        channel.QueueBind(queue: "adas",
                          exchange: "exchange",
                          routingKey: "warning");

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, ea) =>
        {
          var body = ea.Body.ToArray();
          var message = Encoding.UTF8.GetString(body);
          Thread.Sleep(1000);
          Console.WriteLine("Received Received {0}", message);
          channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };
        channel.BasicConsume(queue: "adas",
                             autoAck: false,
                             consumer: consumer);

        Console.ReadLine();
      }
    }
  }
}
