using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MunchyAPI.Models;
using System.Threading.Tasks;

namespace MunchyAPI.Services
{
    public class OptionsService
    {
        private readonly IMongoCollection<Options> OptionsCollection;

        public OptionsService(IMunchyDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            OptionsCollection = database.GetCollection<Options>(settings.OptionsCollectionName);
        }

        public async Task<List<Options>> GetAllAsync()
        {
            return await OptionsCollection.Find(options => true).ToListAsync();
        }

        public async Task<Options> GetByIdAsync(string id)
        {
            return await OptionsCollection.Find<Options>(options => options.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Options> CreateAsync(Options options)
        {
            await OptionsCollection.InsertOneAsync(options);
            return options;
        }

        public async Task UpdateAsync(string id, Options optionsIn)
        {
            await OptionsCollection.ReplaceOneAsync(options => options.Id == id, optionsIn);
        }

        public async Task AddOptionAsync(string id, string optionId)
        {
            var filter = Builders<Options>.Filter.Where(options => options.Id == id);
            var update = Builders<Options>.Update.Push<string>(options => options.ListOfOptionId, optionId);
            await OptionsCollection.FindOneAndUpdateAsync(filter, update);
        }

        public async Task DeleteOptionAsync(string id, string optionId)
        {
            var filter = Builders<Options>.Filter.Where(options => options.Id == id);
            var update = Builders<Options>.Update.Pull<string>(options => options.ListOfOptionId, optionId);
            await OptionsCollection.FindOneAndUpdateAsync(filter, update);
        }


        public async Task DeleteAsync(string id)
        {
            await OptionsCollection.DeleteOneAsync(options => options.Id == id);
        }
    }
}
