using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace UpdateArticlesFullTextSearch.RabbitMq
{
    public interface IRabbitMqConnection
    {
        IModel CreateChannel();
        void Dispose();
    }

    public class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly IConnection _connection;

        public RabbitMqConnection(IConfiguration configuration)
        {            
            var rabbitMQConfig = configuration.GetSection("RabbitMQConfig");
            var hostName = rabbitMQConfig["HostName"];
            var port = int.Parse(rabbitMQConfig["Port"]);
            var userName = rabbitMQConfig["UserName"];
            var password = rabbitMQConfig["Password"];

            var factory = new ConnectionFactory
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password
            };

            _connection = factory.CreateConnection();
        }

        public IModel CreateChannel()
        {
            return _connection.CreateModel();
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
