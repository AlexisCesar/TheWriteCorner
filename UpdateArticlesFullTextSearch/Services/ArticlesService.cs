﻿using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using UpdateArticlesFullTextSearch.Models;

namespace UpdateArticlesFullTextSearch.Services
{
    public interface IArticlesService
    {
        Task CreateAsync(Article newArticle);
        Task UpdateAsync(string id, Article updatedArticle);
        Task RemoveAsync(string id);
    }

    public class ArticlesService : IArticlesService
    {
        private readonly IMongoCollection<Article> _articlesCollection;

        public ArticlesService(IConfiguration configuration)
        {
            var mongoConfig = configuration.GetSection("ArticlesSearchDatabase");
            var mongoClient = new MongoClient(
                mongoConfig["ConnectionString"]);

            var mongoDatabase = mongoClient.GetDatabase(
                mongoConfig["DatabaseName"]);

            _articlesCollection = mongoDatabase.GetCollection<Article>(
                mongoConfig["ArticlesCollectionName"]);
        }

        public async Task CreateAsync(Article article) =>
            await _articlesCollection.InsertOneAsync(article);

        public async Task UpdateAsync(string id, Article updatedArticle) =>
            await _articlesCollection.ReplaceOneAsync(x => x.Id == id, updatedArticle);

        public async Task RemoveAsync(string id) =>
            await _articlesCollection.DeleteOneAsync(x => x.Id == id);
    }
}
