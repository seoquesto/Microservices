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
        channel.QueueDeclare(queue: "hello",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
          var body = ea.Body.ToArray();
          var message = Encoding.UTF8.GetString(body);
          var dots = message.Split(".").Length;
          Console.WriteLine("Processing Received {0}", message);
          Thread.Sleep(dots * 1000);
          Console.WriteLine("Received Received {0}", message);
          channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };
        channel.BasicConsume(queue: "hello",
                             autoAck: false,
                             consumer: consumer);

        Console.ReadLine();
      }
    }
  }
}
