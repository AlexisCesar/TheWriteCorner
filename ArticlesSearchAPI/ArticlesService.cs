
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public interface IArticlesService
{
    Task<List<Article>> GetAsync();
    Task<Article?> GetAsync(string id);
}
public class ArticlesService : IArticlesService
{
    private readonly IMongoCollection<Article> _articlesCollection;

    public ArticlesService(IOptions<ArticlesSearchDatabaseSettings> articlesDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            articlesDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            articlesDatabaseSettings.Value.DatabaseName);

        _articlesCollection = mongoDatabase.GetCollection<Article>(
            articlesDatabaseSettings.Value.ArticlesCollectionName);
    }

    public async Task<List<Article>> GetAsync() =>
        await _articlesCollection.Find(_ => true).ToListAsync();

    public async Task<Article?> GetAsync(string id) =>
        await _articlesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
}