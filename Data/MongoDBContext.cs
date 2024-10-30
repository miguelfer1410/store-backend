using MongoDB.Driver;
using MyShopAPI.Models;
using Microsoft.Extensions.Options;

namespace MyShopAPI.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
        public IMongoCollection<Favorite> Favorites => _database.GetCollection<Favorite>("Favorites");
        public IMongoDatabase Database => _database;

    }

    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
