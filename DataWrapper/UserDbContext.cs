using MongoDB.Driver;
using WorkingHoursCounterSystemCore.Models;

namespace WorkingHoursCounterSystemCore.DataWrapper
{
    public class UserDbContext
    {
        private readonly MongoClient _mongoClient;

        private readonly IMongoDatabase _mongoDatabase;

        private IMongoCollection<User> _usersCollection;

        private const string ConstUsersCollection = "Users";

        public UserDbContext(IConfiguration configuration)
        {
            this._mongoClient = new MongoClient(configuration.GetValue<string>("ConnectionStrings:MongoDbConnectionString"));

            this._mongoDatabase = _mongoClient.GetDatabase(configuration.GetValue<string>("ConnectionStrings:MongoDbName"));

            this._usersCollection = _mongoDatabase.GetCollection<User>(ConstUsersCollection);
        }


    }
}