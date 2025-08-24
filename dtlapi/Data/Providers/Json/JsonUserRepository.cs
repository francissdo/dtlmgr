using dtlapi.Models;
using Newtonsoft.Json;

namespace dtlapi.Data.Providers.Json
{
    public class JsonUserRepository : IUserRepository
    {
        private readonly JsonDataProvider _dataProvider;
        private readonly string _usersFilePath;

        public JsonUserRepository(JsonDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _usersFilePath = Path.Combine(_dataProvider.GetConnectionString(), "users.json");
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await LoadUsersAsync();
            return users;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var users = await LoadUsersAsync();
            return users.FirstOrDefault(u => u.Id == id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            var users = await LoadUsersAsync();
            return users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var users = await LoadUsersAsync();
            return users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<User> CreateAsync(User user)
        {
            var users = await LoadUsersAsync();
            user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
            user.CreatedAt = DateTime.UtcNow;
            
            users.Add(user);
            await SaveUsersAsync(users);
            
            return user;
        }

        public async Task<User?> UpdateAsync(int id, User user)
        {
            var users = await LoadUsersAsync();
            var existingUser = users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.PasswordHash = user.PasswordHash;
            
            await SaveUsersAsync(users);
            return existingUser;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var users = await LoadUsersAsync();
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return false;
            }

            users.Remove(user);
            await SaveUsersAsync(users);
            return true;
        }

        private async Task<List<User>> LoadUsersAsync()
        {
            if (!File.Exists(_usersFilePath))
            {
                // Create default users if file doesn't exist
                var defaultUsers = new List<User>
                {
                    new User
                    {
                        Id = 1,
                        Username = "admin",
                        Email = "admin@dtlmanager.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Id = 2,
                        Username = "user",
                        Email = "user@example.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                        CreatedAt = DateTime.UtcNow
                    }
                };
                
                await SaveUsersAsync(defaultUsers);
                return defaultUsers;
            }

            var json = await File.ReadAllTextAsync(_usersFilePath);
            return JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
        }

        private async Task SaveUsersAsync(List<User> users)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_usersFilePath)!);
            var json = JsonConvert.SerializeObject(users, Formatting.Indented);
            await File.WriteAllTextAsync(_usersFilePath, json);
        }
    }
}
