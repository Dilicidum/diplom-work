using Microsoft.AspNetCore.Identity;

namespace API.Interfaces
{
    public interface IJWTManager
    {
        public Task<string> GenerateToken(IdentityUser user);
    }
}
