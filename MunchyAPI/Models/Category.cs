using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MunchyAPI.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        public string Name { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> ListOfItemId { get; set; }

        [BsonIgnore]
        public List<Item> ListOfItem { get; set; }

        public Category()
        {
            ListOfItemId = new List<string>();
            ListOfItem = new List<Item>();
        }
    }
}
