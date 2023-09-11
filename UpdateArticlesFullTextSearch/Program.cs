using Microsoft.Extensions.Configuration;
using UpdateArticlesFullTextSearch.RabbitMq;
using UpdateArticlesFullTextSearch.Services;

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

        var service = new ArticlesService(configuration);

        Console.WriteLine("Initializing consumer...");
        
        rabbitMqConsumer.RabbitMqStartConsumer("createArticle", "articlesOperations", "create.*", async (article) => { await service.CreateAsync(article); });
        rabbitMqConsumer.RabbitMqStartConsumer("updateArticle", "articlesOperations", "update.*", async (article) => { await service.UpdateAsync(article.Id!, article); });
        rabbitMqConsumer.RabbitMqStartConsumer("deleteArticle", "articlesOperations", "delete", async (article) => { await service.RemoveAsync(article.Id!); });

        Console.WriteLine("Listening RabbitMQ queue");

        Console.ReadLine();
        Console.WriteLine("Ending application");
    }
}