﻿using ArticlesAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ArticlesAPI.Services
{
    public interface IArticlesService
    {
        Task<List<Article>> GetAsync();
        Task<Article?> GetAsync(string id);
        Task CreateAsync(Article newArticle);
        Task UpdateAsync(string id, Article updatedArticle);
        Task RemoveAsync(string id);
    }

    public class ArticlesService: IArticlesService
    {
        private readonly IMongoCollection<Article> _articlesCollection;

        public ArticlesService(
            IOptions<ArticlesManagementDatabaseSettings> articlesDatabaseSettings)
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

        public async Task CreateAsync(Article article) =>
            await _articlesCollection.InsertOneAsync(article);

        public async Task UpdateAsync(string id, Article updatedArticle) =>
            await _articlesCollection.ReplaceOneAsync(x => x.Id == id, updatedArticle);

        public async Task RemoveAsync(string id) =>
            await _articlesCollection.DeleteOneAsync(x => x.Id == id);
    }
}
