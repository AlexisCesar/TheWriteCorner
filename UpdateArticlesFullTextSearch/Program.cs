using System.Reflection;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using UpdateArticlesFullTextSearch.Models;
using UpdateArticlesFullTextSearch.RabbitMq;
using UpdateArticlesFullTextSearch.Services;

class Program
{
    static void Main()
    {
        var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
        XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        ILog logger = LogManager.GetLogger(typeof(Program));

        var configuration = new ConfigurationBuilder()        
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        var rabbitMqConnection = new RabbitMqConnection(configuration);
        var rabbitMqConsumer = new RabbitMqConsumer(rabbitMqConnection);

        var service = new ArticlesService(configuration);

        logger.Info("Initializing consumers...");
        
        rabbitMqConsumer.RabbitMqStartConsumer<Article>("createArticle", "articlesOperations", "create.*", async (article) => { await service.CreateAsync(article); });
        rabbitMqConsumer.RabbitMqStartConsumer<Article>("updateArticle", "articlesOperations", "update.*", async (article) => { await service.UpdateAsync(article.Id!, article); });
        rabbitMqConsumer.RabbitMqStartConsumer<Article>("deleteArticle", "articlesOperations", "delete", async (article) => { await service.RemoveAsync(article.Id!); });

        Console.ReadLine();
        logger.Info("Ending application");
    }
}