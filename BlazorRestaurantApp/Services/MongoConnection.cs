using BlazorRestaurantApp.Data;
using DnsClient;
using MongoDB.Driver;

namespace BlazorRestaurantApp.Services
{
    public class MongoConnection
    {
        IMongoDatabase _database;

        public MongoConnection()
        {
            var client = new MongoClient("mongodb://localhost");
            _database = client.GetDatabase("RestaurantApp");
        }


        #region AddingUserToDatabase
        public void AddUserToDataBase(Customer user)
        {
            var collection = _database.GetCollection<Customer>("CustomersCollection");
            collection.InsertOneAsync(user);
        }

        public void AddUserToDataBase(Employee user)
        {
            var collection = _database.GetCollection<Employee>("EmployeesCollection");
            collection.InsertOneAsync(user);
        }
        #endregion

        #region FindUser

        public async Task<Customer> FindCustomerByEmail (string email)
        {
            var filter = Builders<Customer>.Filter.Eq("Email", email);
            var collection = _database.GetCollection<Customer>("CustomersCollection");
            return (await collection.FindAsync(filter)).FirstOrDefault();
        }

        public async Task<Employee> FindEmployeeByEmail(string email)
        {
            var filter = Builders<Employee>.Filter.Eq("Email", email);
            var collection = _database.GetCollection<Employee>("EmployeesCollection");
            return (await collection.FindAsync(filter)).FirstOrDefault();
        }
        #endregion
    }
}
