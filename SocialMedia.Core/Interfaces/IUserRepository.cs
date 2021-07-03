using SocialMedia.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int userId);
        Task InsertUser(User user);
        Task<User> UpdateUser(User user);
        Task<bool> DeleteUser(int userId);
    }
}
