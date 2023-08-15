using RabbitMQ.Client;
using System.Text;

namespace ArticlesAPI.RabbitMq
{
    public interface IRabbitMqPublisher 
    {
        void PublishMessage(string exchange, string message);
    }

    public class RabbitMqPublisher : IRabbitMqPublisher
    {
        IRabbitMqConnection _rabbitMqConnection;
        public RabbitMqPublisher(IRabbitMqConnection rabbitMqConnection) 
        {
            _rabbitMqConnection = rabbitMqConnection;
        }

        public void PublishMessage(string exchange, string message) 
        {
            using (var channel = _rabbitMqConnection.CreateChannel())
            {
               channel.ExchangeDeclare(exchange: exchange,
                                        type: "fanout",
                                        durable: true,
                                        autoDelete: false);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: exchange,
                                     routingKey: string.Empty,
                                     basicProperties: null,
                                     body: body);
            }
        }

    }
}