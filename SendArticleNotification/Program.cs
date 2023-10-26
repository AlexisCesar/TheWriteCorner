using System.Reflection;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using SendArticleNotification.Models;
using SendArticleNotification.RabbitMq;

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

        //var service = new ArticlesService(configuration);

        logger.Info("Initializing consumers...");

        rabbitMqConsumer.RabbitMqStartConsumer<Article>("sendNotification", "articlesOperations", "*.notify", /*async*/ (article) => { Console.WriteLine("Notify called."); /*await service.CreateAsync(article);*/ });

        Console.ReadLine();
        logger.Info("Ending application");
    }
}