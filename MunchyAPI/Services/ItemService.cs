using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MunchyAPI.Models;
using System.Threading.Tasks;

namespace MunchyAPI.Services
{
    public class ItemService
    {
        private readonly IMongoCollection<Item> ItemsCollection;

        public ItemService(IMunchyDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            ItemsCollection = database.GetCollection<Item>(settings.ItemsCollectionName);
        }

        public async Task<List<Item>> GetAllAsync()
        {
            return await ItemsCollection.Find(item => true).ToListAsync();
        }

        public async Task<Item> GetByIdAsync(string id)
        {
            return await ItemsCollection.Find<Item>(item => item.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Item> CreateAsync(Item item)
        {
            await ItemsCollection.InsertOneAsync(item);
            return item;
        }

        public async Task UpdateAsync(string id, Item itemIn)
        {
            await ItemsCollection.ReplaceOneAsync(item => item.Id == id, itemIn);
        }

        public async Task AddOptionsAsync(string id, string optionsId)
        {
            var filter = Builders<Item>.Filter.Where(item => item.Id == id);
            var update = Builders<Item>.Update.Push<string>(item => item.ListOfOptiosnId, optionsId);
            await ItemsCollection.FindOneAndUpdateAsync(filter, update);
        }

        public async Task DeleteOptionsAsync(string id, string optionsId)
        {
            var filter = Builders<Item>.Filter.Where(item => item.Id == id);
            var update = Builders<Item>.Update.Pull<string>(item => item.ListOfOptiosnId, optionsId);
            await ItemsCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteOptionsFromAllAsync(string optionsId)
        {
            var filter = Builders<Item>.Filter.Where(item => true);
            var update = Builders<Item>.Update.Pull<string>(item => item.ListOfOptiosnId, optionsId);
            await ItemsCollection.UpdateManyAsync(filter, update);
        }

        public async Task DeleteByCategoryIdAsync(string categoryId)
        {
            await ItemsCollection.DeleteManyAsync(item => item.CategoryId == categoryId);
        }

        public async Task DeleteAsync(string id)
        {
            await ItemsCollection.DeleteOneAsync(item => item.Id == id);
        }
    }
}
