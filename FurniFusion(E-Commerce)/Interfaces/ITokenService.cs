using FurniFusion.Models;

namespace FurniFusion.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User appUser, IList<string> roles);
    }
}
