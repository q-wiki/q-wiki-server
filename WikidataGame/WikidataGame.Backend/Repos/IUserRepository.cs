using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Repos
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<User> CreateOrUpdateUserAsync(string firebaseUid, string pushToken, string username);
    }
}
