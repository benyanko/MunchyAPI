using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MunchyAPI.Models
{
    public class Item
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Item name is required")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Display(Name = "Price($)")]
        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public decimal Price { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> ListOfOptiosnId { get; set; }
         
        public List<Options> ListOfOptions { get; set; }

        
        [Required(ErrorMessage = "Item Category is required")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }
        

        public Item()
        {
            ListOfOptiosnId = new List<string>();
            ListOfOptions = new List<Options>();
        }
    }
}
