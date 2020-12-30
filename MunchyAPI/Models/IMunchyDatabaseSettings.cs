using System;
namespace MunchyAPI.Models
{
    public interface IMunchyDatabaseSettings
    {
        string RestaurantsCollectionName { get; set; }
        string CategoriesCollectionName { get; set; }
        string ItemsCollectionName { get; set; }
        string OptionsCollectionName { get; set; }
        string OptionCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
