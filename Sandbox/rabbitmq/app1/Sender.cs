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
        channel.ExchangeDeclare("logs", ExchangeType.Direct);

        var body = Encoding.UTF8.GetBytes("Error message");
        channel.BasicPublish(exchange: "exchange",
                           routingKey: "error",
                           basicProperties: null,
                           body: body);

        body = Encoding.UTF8.GetBytes("Info message");
        channel.BasicPublish(exchange: "exchange",
                           routingKey: "info",
                           basicProperties: null,
                           body: body);

        body = Encoding.UTF8.GetBytes("Warning message");
        channel.BasicPublish(exchange: "exchange",
                           routingKey: "warning",
                           basicProperties: null,
                           body: body);

        body = Encoding.UTF8.GetBytes("Error message");
        channel.BasicPublish(exchange: "exchange",
                           routingKey: "error",
                           basicProperties: null,
                           body: body);

      }
      Console.ReadLine();
    }
  }
}
