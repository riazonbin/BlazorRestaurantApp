using BlazorRestaurantApp.Data;
using DnsClient;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BlazorRestaurantApp.Services
{
    public class MongoConnection
    {
        IMongoDatabase _database;
        string _defaultConnectionString = "mongodb://localhost";

        public MongoConnection()
        {
            var client = new MongoClient(_defaultConnectionString);
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

        public async void AddDefaultManagerToDataBase()
        {
            Employee manager = new Employee()
            {
                Email = "RZrestaurant@gmail.com",
                Firstname = "Default",
                Lastname = "Restaurant",
                Patronymic = "Manager",
                Password = "qwerty1234",
                Role = Enums.UserRoles.Employee
            };
            var collection = _database.GetCollection<Employee>("EmployeesCollection");
            await collection.InsertOneAsync(manager);
        }

        public bool IsDefaultManagerExists()
        {
            var collection = _database.GetCollection<Employee>("EmployeesCollection");
            var result = collection.Find(x => x.Email == "RZrestaurant@gmail.com").FirstOrDefault();
            if(result is null)
            {
                return false;
            }
            return true;
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

        #region CartRegopn

        public async Task<Cart> GetUserCart(string userId)
        {
            ObjectId objectUserId = ObjectId.Parse(userId);

            var collection = _database.GetCollection<Cart>("CartsCollection");
            var cart = await collection.FindAsync(x => x.UserId == objectUserId).Result.FirstOrDefaultAsync();
            return cart;
        }

        public async Task CreateUserCart(string userId)
        {
            ObjectId objectUserId = ObjectId.Parse(userId);

            var collection = _database.GetCollection<Cart>("CartsCollection");
            var cart = new Cart()
            {
                Items= new List<CartItem>(),
                UserId = objectUserId
            };
            await collection.InsertOneAsync(cart);
        }

        public void AddItemToUserCart(Cart userCart, MenuItem menuItem)
        {
            var collection = _database.GetCollection<Cart>("CartsCollection");

            var cartItem = userCart.Items.Where(x => x.MenuItem.Id == menuItem.Id).FirstOrDefault();

            if (cartItem is not null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cartItem = new CartItem()
                {
                    MenuItem = menuItem,
                    Quantity = 1
                };
                userCart.Items.Add(cartItem);
            }

            collection.ReplaceOne(x => x.Id == userCart.Id, userCart);
        }

        public void DeleteItemFromUserCart(Cart userCart, MenuItem menuItem)
        {
            var collection = _database.GetCollection<Cart>("CartsCollection");

            var cartItem = userCart.Items.FirstOrDefault(x => x.MenuItem.Id == menuItem.Id);

            if (cartItem is not null)
            {
                userCart.Items.Remove(cartItem);
            }

            collection.ReplaceOne(x => x.Id == userCart.Id, userCart);
        }

        public void LowerCountOfItemInUserCart(Cart userCart, MenuItem menuItem)
        {
            var collection = _database.GetCollection<Cart>("CartsCollection");

            var cartItem = userCart.Items.FirstOrDefault(x => x.MenuItem.Id == menuItem.Id);

            if (cartItem is not null)
            {
                if(cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                }
                else
                {
                    userCart.Items.Remove(cartItem);
                }

            }

            collection.ReplaceOne(x => x.Id == userCart.Id, userCart);
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

        public async Task DeleteMenuItemAsync(MenuItem item)
        {
            var collection = _database.GetCollection<MenuItem>("MenuItemsCollection");
            await collection.DeleteOneAsync(x => x.Id == item.Id);
        }
        #endregion
    }
}
