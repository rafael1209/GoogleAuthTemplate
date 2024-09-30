using MongoDB.Driver;
using WorkingHoursCounterSystemCore.Models;

namespace WorkingHoursCounterSystemCore.DataWrapper
{
    public class DashboardDbContext
    {
        private readonly MongoClient _mongoClient;

        private readonly IMongoDatabase _mongoDatabase;

        private IMongoCollection<Shift> _shiftsCollection;

        private const string ConstShiftsCollection = "Shift";

        public DashboardDbContext(IConfiguration configuration)
        {
            this._mongoClient = new MongoClient(configuration.GetValue<string>("ConnectionStrings:MongoDbConnectionString"));

            this._mongoDatabase = _mongoClient.GetDatabase(configuration.GetValue<string>("ConnectionStrings:MongoDbName"));

            this._shiftsCollection = _mongoDatabase.GetCollection<Shift>(ConstShiftsCollection);
        }
    }
}
