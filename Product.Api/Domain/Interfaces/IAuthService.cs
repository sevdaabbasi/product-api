namespace Product.Api.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<bool> ValidateUserAsync(string username, string password);
    }
} 