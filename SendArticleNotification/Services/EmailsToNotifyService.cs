using SendArticleNotification.Models;

namespace SendArticleNotification.Services
{
    public interface IEmailsToNotifyService
    {
        Task<List<string>> GetEmailsToNotify();
    }

    public class EmailsToNotifyService : IEmailsToNotifyService
    {
        public Task<List<string>> GetEmailsToNotify()
        {
            throw new NotImplementedException();
        }
    }
}
