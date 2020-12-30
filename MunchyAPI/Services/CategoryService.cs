using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MunchyAPI.Models;
using System.Threading.Tasks;

namespace MunchyAPI.Services
{
    public class CategoryService
    {
        private readonly IMongoCollection<Category> CategoriesCollection;

        public CategoryService(IMunchyDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            CategoriesCollection = database.GetCollection<Category>(settings.CategoriesCollectionName);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await CategoriesCollection.Find(category => true).ToListAsync();
        }

        public async Task<Category> GetByIdAsync(string id)
        {
            return await CategoriesCollection.Find<Category>(category => category.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await CategoriesCollection.InsertOneAsync(category);
            return category;
        }

        public async Task UpdateAsync(string id, Category categoryIn)
        {
            await CategoriesCollection.ReplaceOneAsync(category => category.Id == id, categoryIn);
        }

        public async Task AddItemAsync(string id, string itemId)
        {
            var filter = Builders<Category>.Filter.Where(category => category.Id == id);
            var update = Builders<Category>.Update.Push<string>(category => category.ListOfItemId, itemId);
            await CategoriesCollection.FindOneAndUpdateAsync(filter, update);
        }

        public async Task DeleteItemAsync(string id, string itemId)
        {
            var filter = Builders<Category>.Filter.Where(category => category.Id == id);
            var update = Builders<Category>.Update.Pull<string>(category => category.ListOfItemId, itemId);
            await CategoriesCollection.FindOneAndUpdateAsync(filter, update);
        }


        public async Task DeleteAsync(string id)
        {
            await CategoriesCollection.DeleteOneAsync(category => category.Id == id);
        }
    }
}
