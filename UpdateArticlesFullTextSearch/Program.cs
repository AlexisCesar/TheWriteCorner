using Microsoft.Extensions.Configuration;
using UpdateArticlesFullTextSearch.RabbitMq;

class Program
{
    static void Main()
    {
        var configuration = new ConfigurationBuilder()        
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        var rabbitMqConnection = new RabbitMqConnection(configuration);
        var rabbitMqConsumer = new RabbitMqConsumer(rabbitMqConnection);

        Console.WriteLine("Initializing consumer...");

        rabbitMqConsumer.RabbitMqStartConsumer("updateArticle", "articlesOperations");

        Console.WriteLine("Listening RabbitMQ queue");

        Console.ReadLine();
        Console.WriteLine("Ending application");
    }
}