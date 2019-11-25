using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Repos
{
    public interface IUserRepository : IRepository<User, string>
    {
        Task<User> CreateOrUpdateUserAsync(string deviceId, string pushToken, string username);
    }
}
