using log4net;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace SendArticleNotification.RabbitMq
{
    public class RabbitMqConsumer
    {
        readonly IRabbitMqConnection _rabbitMqConnection;
        private ILog Logger => LogManager.GetLogger(nameof(RabbitMqConsumer));

        public RabbitMqConsumer(IRabbitMqConnection rabbitMqConnection)
        {
            _rabbitMqConnection = rabbitMqConnection;
        }

        public void RabbitMqStartConsumer<T>(string queueName, string exchange, string routingKey, Action<T> callback) 
        {
            var channel = _rabbitMqConnection.CreateChannel();
            
            Logger.Info($"Declaring exchange: {exchange}");
            channel.ExchangeDeclare(exchange: exchange,
                                    type: ExchangeType.Topic,
                                    durable: true,
                                    autoDelete: false);

            Logger.Info($"Declaring queue: {queueName}");
            channel.QueueDeclare(queueName, true, false, false, null);


            Logger.Info($"Binding queue '{queueName}' with exchange '{exchange}' using '{routingKey}' routing key");
            channel.QueueBind(queue: queueName,
                                exchange: exchange,
                                routingKey: routingKey);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                Logger.Info($" [x] Received message in queue '{queueName}' with routingkey '{routingKey}'.");

                var article = JsonSerializer.Deserialize<T>(message);
                if(article != null ) callback(article);

                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            Logger.Info("Started consuming from queue: " + queueName);
            channel.BasicConsume(queue: queueName,
                                    autoAck: false,
                                    consumer: consumer);            
        }
    }
}