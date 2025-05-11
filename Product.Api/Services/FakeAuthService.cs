using Product.Api.Domain.Interfaces;

namespace Product.Api.Services
{
    public class FakeAuthService : IAuthService
    {
        private readonly Dictionary<string, string> _users = new()
        {
            { "admin", "admin123" },
            { "user", "user123" }
        };

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            return await Task.FromResult(_users.TryGetValue(username, out var storedPassword) && 
                                        storedPassword == password);
        }
    }
} 