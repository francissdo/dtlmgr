using dtlapi.Models;

namespace dtlapi.Data.Providers.Xml
{
    public class XmlUserRepository : IUserRepository
    {
        private readonly XmlDataProvider _dataProvider;

        public XmlUserRepository(XmlDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await _dataProvider.LoadUsersAsync();
            return users;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var users = await _dataProvider.LoadUsersAsync();
            return users.FirstOrDefault(u => u.Id == id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            var users = await _dataProvider.LoadUsersAsync();
            return users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var users = await _dataProvider.LoadUsersAsync();
            return users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<User> CreateAsync(User user)
        {
            var users = await _dataProvider.LoadUsersAsync();
            user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
            user.CreatedAt = DateTime.UtcNow;
            
            users.Add(user);
            await _dataProvider.SaveUsersAsync(users);
            
            return user;
        }

        public async Task<User?> UpdateAsync(int id, User user)
        {
            var users = await _dataProvider.LoadUsersAsync();
            var existingUser = users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.PasswordHash = user.PasswordHash;
            
            await _dataProvider.SaveUsersAsync(users);
            return existingUser;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var users = await _dataProvider.LoadUsersAsync();
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return false;
            }

            users.Remove(user);
            await _dataProvider.SaveUsersAsync(users);
            return true;
        }

        private async Task InitializeDefaultUsersIfNeeded()
        {
            var users = await _dataProvider.LoadUsersAsync();
            if (users.Count == 0)
            {
                // Create default users if none exist
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
                
                await _dataProvider.SaveUsersAsync(defaultUsers);
            }
        }
    }
}
