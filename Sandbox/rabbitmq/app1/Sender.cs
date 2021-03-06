using System;
using System.Text;
using RabbitMQ.Client;

namespace app1
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


        for (int i = 0; i < 6; i++)
        {
          string message = $"[i:{i}]" + new String('.', i);
          var body = Encoding.UTF8.GetBytes(message);
          channel.BasicPublish(exchange: "",
                             routingKey: "hello",
                             basicProperties: null,
                             body: body);

          Console.WriteLine("Send: " + message);
        }
      }

      Console.ReadLine();
    }
  }
}
