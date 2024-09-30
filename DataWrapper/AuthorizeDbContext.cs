using MongoDB.Driver;
using WorkingHoursCounterSystemCore.Models;

namespace WorkingHoursCounterSystemCore.DataWrapper
{
    public class AuthorizeDbContext
    {
        private readonly MongoClient _mongoClient;

        private readonly IMongoDatabase _mongoDatabase;

        private IMongoCollection<User> _usersCollection;

        private const string ConstUsersCollection = "Users";

        public AuthorizeDbContext(IConfiguration configuration)
        {
            this._mongoClient = new MongoClient(configuration.GetValue<string>("ConnectionStrings:MongoDbConnectionString"));

            this._mongoDatabase = _mongoClient.GetDatabase(configuration.GetValue<string>("ConnectionStrings:MongoDbName"));

            this._usersCollection = _mongoDatabase.GetCollection<User>(ConstUsersCollection);
        }

        public User GetUserByToken(string authToken)
        {
            var filter = Builders<User>.Filter.Eq(u => u.AuthToken, authToken);
            var user = _mongoDatabase.GetCollection<User>(ConstUsersCollection).Find(filter).FirstOrDefault();

            return user;
        }

        public bool IsUserExistByEmail(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);

            return _mongoDatabase.GetCollection<User>(ConstUsersCollection).Find(filter).Any();
        }

        public User GetUserByEmail(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);

            return _usersCollection.Find(filter).FirstOrDefault();
        }

        public User AddUser(User user, string authToken)
        {
            user.CreatedAtUtc = DateTime.UtcNow;

            user.AuthToken = authToken;

            _usersCollection.InsertOne(user);

            return user;
        }
    }
}
