using BlazorRestaurantApp.Data;
using DnsClient;
using MongoDB.Bson;
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

        #region AddingMenuItemsToDatabase
        public void AddMenuItemToDatabase(MenuItem menuItem)
        {
            var collection = _database.GetCollection<MenuItem>("MenuItemsCollection");
            collection.InsertOneAsync(menuItem);
        }
        #endregion

        #region FindMenuItems
        public async Task<MenuItem> FindMenuItemById(string id)
        {
            var collection = _database.GetCollection<MenuItem>("MenuItemsCollection");
            return (await collection.FindAsync(x => x.Id == ObjectId.Parse(id))).FirstOrDefault();
        }

        public async Task<List<MenuItem>> GetAllMenuItems()
        {
            var collection = _database.GetCollection<MenuItem>("MenuItemsCollection");
            return await (await collection.FindAsync(new BsonDocument())).ToListAsync();
        }
        #endregion

        #region FindUser

        public User? FindUserByEmail(string email)
        {
            var findedCustomer =  FindCustomerByEmail(email);
            var findedEmployee =  FindEmployeeByEmail(email);

            if(findedCustomer != null)
            {
                return findedCustomer;
            }
            else if(findedEmployee != null) 
            {
                return findedEmployee;
            }

            return null;
        }

        public  Customer FindCustomerByEmail (string email)
        {
            var filter = Builders<Customer>.Filter.Eq("Email", email);
            var collection = _database.GetCollection<Customer>("CustomersCollection");
            return collection.Find(filter).FirstOrDefault();
        }

        public Employee FindEmployeeByEmail(string email)
        {
            var filter = Builders<Employee>.Filter.Eq("Email", email);
            var collection = _database.GetCollection<Employee>("EmployeesCollection");
            return collection.Find(filter).FirstOrDefault();
        }
        #endregion

        #region UpdateData
        public void UpdateMenuItem(MenuItem item)
        {
            var collection = _database.GetCollection<MenuItem>("MenuItemsCollection");
            collection.ReplaceOne(x => x.Id == item.Id, item);
        }
        #endregion
    }
}
