using System;
namespace MunchyAPI.Models
{
    public class MunchyDatabaseSettings: IMunchyDatabaseSettings
    {
       public string RestaurantsCollectionName { get; set; }
       public string CategoriesCollectionName { get; set; }
       public string ItemsCollectionName { get; set; }
       public string OptionsCollectionName { get; set; }
       public string OptionCollectionName { get; set; }
       public string ConnectionString { get; set; }
       public string DatabaseName { get; set; }

        public MunchyDatabaseSettings()
        {
        }
    }
}
