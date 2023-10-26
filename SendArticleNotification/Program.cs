using System.Reflection;
using log4net;
using log4net.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SendArticleNotification.Data;
using SendArticleNotification.Email;
using SendArticleNotification.Email.Services;
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

        var emailSettingsConfig = configuration.GetSection("EmailSettings");
        var emailSettings = new EmailSettings()
        {
            EmailId = emailSettingsConfig["EmailId"],
            Host = emailSettingsConfig["Host"],
            Name = emailSettingsConfig["Name"],
            Password = emailSettingsConfig["Password"],
            Port = int.TryParse(emailSettingsConfig["Port"], out int x) ? x : 0,
            UseSSL = bool.TryParse(emailSettingsConfig["UseSSL"], out bool y) ? y : false,
        };

        var emailService = new EmailService(emailSettings);
        var emailDbContext = new EmailDbContext();
        if (emailDbContext.Database.GetPendingMigrations().Any())
        {
            emailDbContext.Database.Migrate();
        }

        logger.Info("Initializing consumers...");

        rabbitMqConsumer.RabbitMqStartConsumer<Article>("sendNotification", "articlesOperations", "*.notify", async (article) => {

            await emailDbContext.Emails.ForEachAsync((email) =>
            {
                emailService.SendEmail(new EmailData()
                {
                    EmailToId = email.EmailAddress,
                    EmailToName = "Dear Reader",
                    EmailSubject = "TheWriteCorner - Something just happened!",
                    EmailBody = @$"
                    <html xmlns=""http://www.w3.org/1999/xhtml"">
                        <head>
                            <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
                        </head>
                        <div style=""background-color: ##f2ecda;"">
                        <h2>Hey, you should check this article!</h2>
                        <hr>
                        <h3>{article.Title}</h3>
                        <p>Published by: {article.Authors.First()} {((article.Authors.Count() > 1) ? "and others..." : "")}</p>
                        <p>To check this article, click here!<p>
                        <br>
                        <p>To stop receiving notifications, click here.<p>
                        <br>
                        <p>TheWriteCorner! 📝<p>
                        </div>
                    </html>",
                });
            });

        });

        rabbitMqConsumer.RabbitMqStartConsumer<string>("addEmailToNotificationList", "usersOperations", "email.registered", async (emailAddress) => {
            Console.WriteLine($"Time to add {emailAddress} to notification list.");
            await emailDbContext.Emails.AddAsync(new EmailToNotify() { EmailAddress = emailAddress });
            await emailDbContext.SaveChangesAsync();
        });

        Console.ReadLine();
        logger.Info("Ending application");
    }
}