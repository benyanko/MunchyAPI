using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MunchyAPI.Models;
using System.Threading.Tasks;

namespace MunchyAPI.Services
{
    public class OptionService
    {
        private readonly IMongoCollection<Option> OptionCollection;

        public OptionService(IMunchyDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            OptionCollection = database.GetCollection<Option>(settings.OptionCollectionName);
        }

        public async Task<List<Option>> GetAllAsync()
        {
            return await OptionCollection.Find(option => true).ToListAsync();
        }

        public async Task<Option> GetByIdAsync(string id)
        {
            return await OptionCollection.Find<Option>(option => option.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Option> CreateAsync(Option option)
        {
            await OptionCollection.InsertOneAsync(option);
            return option;
        }

        public async Task UpdateAsync(string id, Option optionIn)
        {
            await OptionCollection.ReplaceOneAsync(option => option.Id == id, optionIn);
        }

        public async Task DeleteAsync(string id)
        {
            await OptionCollection.DeleteOneAsync(option => option.Id == id);
        }

        public async Task DeleteParentOptionsAsync(string parentId)
        {
            await OptionCollection.DeleteManyAsync(option => option.ParentId == parentId);
        }
    }
}
