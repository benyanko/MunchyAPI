using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MunchyAPI.Models
{
    public class Option
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Option name is required")]
        public string Name { get; set; }

        [Display(Name = "Price($)")]
        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public decimal Price { get; set; }

        public bool Default { get; set; }

        public int Limit { get; set; }

        [Required(ErrorMessage = "Option must be connected to options")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ParentId { get; set; }

        public Option()
        {
        }
    }
}
