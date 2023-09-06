using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace UpdateArticlesFullTextSearch.RabbitMq
{
    public class RabbitMqConsumer
    {

        IRabbitMqConnection _rabbitMqConnection;
        public RabbitMqConsumer(IRabbitMqConnection rabbitMqConnection)
        {
            _rabbitMqConnection = rabbitMqConnection;
        }

        public void RabbitMqStartConsumer(string queueName, string exchange, string routingKey) 
        {
            var channel = _rabbitMqConnection.CreateChannel();
            
            channel.ExchangeDeclare(exchange: exchange,
                                    type: ExchangeType.Topic,
                                    durable: true,
                                    autoDelete: false);

            channel.QueueDeclare(queueName, true, false, false, null);

            channel.QueueBind(queue: queueName,
                                exchange: exchange,
                                routingKey: routingKey);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                Console.WriteLine($" [x] Received '{routingKey}':'{message}'");

                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: queueName,
                                    autoAck: false,
                                    consumer: consumer);            
        }
    }
}
