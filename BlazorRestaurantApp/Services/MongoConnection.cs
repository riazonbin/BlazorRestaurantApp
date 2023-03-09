using BlazorRestaurantApp.Data;
using DnsClient;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using static MongoDB.Driver.WriteConcern;

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
            if (result is null)
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

            var sort = Builders<MenuItem>.Sort.Ascending("DishType");
            var options = new FindOptions<MenuItem>() { Sort = sort };

            return await (await collection.FindAsync(new BsonDocument(), options)).ToListAsync();
        }
        #endregion

        #region CartRegion

        public async Task<Cart> GetUserCart(string userId)
        {

            var collection = _database.GetCollection<Cart>("CartsCollection");
            var cart = await collection.FindAsync(x => x.UserId == userId).Result.FirstOrDefaultAsync();
            return cart;
        }

        public async Task CreateUserCart(string userId)
        {

            var collection = _database.GetCollection<Cart>("CartsCollection");
            var cart = new Cart()
            {
                Items = new List<CartItem>(),
                UserId = userId
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

        public async Task ClearUserCart(Cart userCart)
        {
            var collection = _database.GetCollection<Cart>("CartsCollection");

            userCart.Items = new List<CartItem>();

            await collection.ReplaceOneAsync(x => x.Id == userCart.Id, userCart);
        }

        public void LowerCountOfItemInUserCart(Cart userCart, MenuItem menuItem)
        {
            var collection = _database.GetCollection<Cart>("CartsCollection");

            var cartItem = userCart.Items.FirstOrDefault(x => x.MenuItem.Id == menuItem.Id);

            if (cartItem is not null)
            {
                if (cartItem.Quantity > 1)
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


        #region TableRegion
        public async Task<bool> IsTableAvailable(int tableNumber, string userId)
        {
            var collection = _database.GetCollection<Table>("TablesCollection");
            var table = collection.Find(x => x.TableNumber == tableNumber).FirstOrDefault();

            var reservationsOnTable = await GetReservationsByTableId(table.Id);

            var query = reservationsOnTable
                .Where(x => x.StartTimeOfReservation <= DateTime.Now
                && x.EndTimeOfReservation >= DateTime.Now
                && x.ReservedCustomerId != userId
                || x.StartTimeOfReservation.Subtract(DateTime.Now).TotalMinutes <= StaticData.ReservationTime
                && x.ReservedCustomerId != userId).ToList();

            if (query.Count == 0)
            {
                return true;
            }

            return false;
        }

        public async Task<Table?> GetAvailableTable(string userId, int numberOfSeats, DateTime timeOfReservation)
        {
            var collection = _database.GetCollection<Table>("TablesCollection");

            var tablesList = await collection.FindAsync(x => x.NumberOfSeats == numberOfSeats).Result.ToListAsync();

            foreach(var table in tablesList)
            {
                var reservations = await GetReservationsByTableId(table.Id);

                var results = reservations
                    .Where(x => x.StartTimeOfReservation <= timeOfReservation
                && x.EndTimeOfReservation >= timeOfReservation
                || x.StartTimeOfReservation.Subtract(timeOfReservation).TotalMinutes <= StaticData.ReservationTime).ToList();

                if(results.Count == 0)
                {
                    return table;
                }
            }
            return null;

        }

        public async Task GenerateTables()
        {
            var collection = _database.GetCollection<Table>("TablesCollection");

            for(int i = 0; i < 40; ++i)
            {
                Random random = new Random();
                var numberOfSeats = random.Next(2, 9);
                while(numberOfSeats % 2 != 0)
                {
                    numberOfSeats = random.Next(2, 9);
                }

                var table = new Table()
                {
                    TableNumber= i + 1,
                    NumberOfSeats = numberOfSeats
                };
                await collection.InsertOneAsync(table);
            }
        }

        public async Task<bool> IsCustomerHasSoonReservationsOnTable(string customerId, string tableId) 
        {
            var reservationsOnTable = await GetReservationsByTableId(tableId);

            var query = reservationsOnTable
                            .Where(x => x.StartTimeOfReservation <= DateTime.Now
                           && x.EndTimeOfReservation >= DateTime.Now
                           && x.ReservedCustomerId == customerId).ToList();

            if (query.Count >= 1)
            {
                return true;
            }

            return false;
        }

        public async Task<Table> GetTableByTableNumber(int tableNumber)
        {
            var collection = _database.GetCollection<Table>("TablesCollection");
            return await collection.FindAsync(x => x.TableNumber == tableNumber).Result.FirstOrDefaultAsync();
        }

        public async Task<Table> GetTableByTableId(string tableId)
        {
            var collection = _database.GetCollection<Table>("TablesCollection");
            return await collection.FindAsync(x => x.Id == tableId).Result.FirstOrDefaultAsync();
        }

        public async Task<List<Reservation>> GetReservationsByTableId(string tableId)
        {
            var collection = _database.GetCollection<Reservation>("ReservationsCollection");
            return await collection.FindAsync(x => x.TableId == tableId).Result.ToListAsync();
        }

        public async Task DeleteOldReservations()
        {
            var collection = _database.GetCollection<Reservation>("ReservationsCollection");
            await collection.DeleteManyAsync(x => DateTime.Now > x.EndTimeOfReservation);
        }

        public async Task DeleteReservationById(string reservationId)
        {
            var collection = _database.GetCollection<Reservation>("ReservationsCollection");
            await collection.DeleteOneAsync(x => x.Id == reservationId);
        }

        public async Task<List<Reservation>> GetReservationsByTableNumber(int tableNumber)
        {
            var table = await GetTableByTableNumber(tableNumber);
            var collection = _database.GetCollection<Reservation>("ReservationsCollection");
            return await collection.FindAsync(x => x.Id == table.Id).Result.ToListAsync();
        }

        public async Task<List<Reservation>> GetReservationsByCustomerId(string customerId)
        {
            var collection = _database.GetCollection<Reservation>("ReservationsCollection");
            return await collection.FindAsync(x => x.ReservedCustomerId == customerId).Result.ToListAsync();
        }

        public async Task<List<Reservation>> GetAllReservations()
        {
            var collection = _database.GetCollection<Reservation>("ReservationsCollection");
            return await collection.FindAsync(new BsonDocument()).Result.ToListAsync();
        }

        public async Task ReserveTable(int tableNumber, string userId, DateTime? timeOfReservation)
        {
            var collection = _database.GetCollection<Reservation>("ReservationsCollection");
            var table = await GetTableByTableNumber(tableNumber);

            Reservation reservation;
            if (timeOfReservation is null)
            {
                reservation = new Reservation(DateTime.Now)
                {
                    ReservedCustomerId = userId,
                    TableId = table.Id
                };
            }
            else
            {
                reservation = new Reservation(timeOfReservation.Value)
                {
                    ReservedCustomerId = userId,
                    TableId = table.Id
                };
            }
            await collection.InsertOneAsync(reservation);
        }
        #endregion

        #region OrderRegion

        public async Task AddOrder(Cart userCart, int tableNumber)
        {
            var table = await GetTableByTableNumber(tableNumber);

            if (!await IsCustomerHasSoonReservationsOnTable(userCart.UserId, table.Id))
            {
                await ReserveTable(tableNumber, userCart.UserId, DateTime.Now);
            }

            Order order = new Order()
            {
                CartItems = userCart.Items,
                CustomerId = userCart.UserId,
                OrderStartTime = DateTime.Now,
                TableId = table.Id,
                Status = Enums.OrderStatuses.InProgress
            };

            var collection = _database.GetCollection<Order>("OrdersCollection");

            await collection.InsertOneAsync(order);

            await ClearUserCart(userCart);
        }

        public async Task<List<Order>> GetCustomersOrders(string customerId)
        {
            var collection = _database.GetCollection<Order>("OrdersCollection");
            return await collection.FindAsync(x => x.CustomerId == customerId).Result.ToListAsync();
        }

        public async Task<List<Order>> GetAllOrders()
        {
            var collection = _database.GetCollection<Order>("OrdersCollection");
            return await collection.FindAsync(new BsonDocument()).Result.ToListAsync();
        }

        public async Task UpdateOrdersStatuses()
        {
            var collection = _database.GetCollection<Order>("OrdersCollection");

            var ordersList = await collection.FindAsync(new BsonDocument()).Result.ToListAsync();


            foreach (var order in ordersList)
            {
                Random random = new Random();

                if(order.Status == Enums.OrderStatuses.IsDelivered)
                {
                    continue;
                }
                else if(order.Status == Enums.OrderStatuses.InProgress)
                {
                    order.Status= Enums.OrderStatuses.IsCooking;
                }
                else if(order.Status == Enums.OrderStatuses.IsCooking) 
                {
                    if(random.Next(1, 3) == 1)
                    {
                        order.Status = Enums.OrderStatuses.IsCooked;
                    }
                }
                else if (order.Status == Enums.OrderStatuses.IsCooked)
                {
                    if (random.Next(1, 2) == 1)
                    {
                        order.Status = Enums.OrderStatuses.IsDelivering;
                    }
                }
                else if (order.Status == Enums.OrderStatuses.IsDelivering)
                {
                    if (random.Next(1, 2) == 1)
                    {
                        order.Status = Enums.OrderStatuses.IsDelivered;
                        order.OrderEndTime= DateTime.Now;
                    }
                }
                await collection.ReplaceOneAsync(x => x.Id == order.Id, order);
            }


        }

        #endregion

        #region FindUser

        public User? FindUserByEmail(string email)
        {
            var findedCustomer = FindCustomerByEmail(email);
            var findedEmployee = FindEmployeeByEmail(email);

            if (findedCustomer != null)
            {
                return findedCustomer;
            }
            else if (findedEmployee != null)
            {
                return findedEmployee;
            }

            return null;
        }

        public Customer FindCustomerByEmail(string email)
        {
            var filter = Builders<Customer>.Filter.Eq("Email", email);
            var collection = _database.GetCollection<Customer>("CustomersCollection");
            return collection.Find(filter).FirstOrDefault();
        }

        public async Task<Customer> FindCustomerById(string customerId)
        {
            var collection = _database.GetCollection<Customer>("CustomersCollection");
            return await collection.FindAsync(x => x.Id == customerId).Result.FirstOrDefaultAsync();
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

        public async Task UpdateCustomer(Customer customer)
        {
            var collection = _database.GetCollection<Customer>("CustomersCollection");
            await collection.ReplaceOneAsync(x => x.Id == customer.Id, customer);
        }

        #endregion
    }
}
