using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MunchyAPI.Models
{
    public class Options
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Options name is required")]
        public string Name { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> ListOfOptionId { get; set; }

        public List<Option> ListOfOption { get; set; }

        public int Limit { get; set; }

        public Options()
        {
            ListOfOptionId = new List<string>();
            ListOfOption = new List<Option>();
        }
    }
}
