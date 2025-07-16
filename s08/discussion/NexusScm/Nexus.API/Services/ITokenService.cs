using Nexus.Core;

namespace Nexus.API.Services
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user);

    }
}
