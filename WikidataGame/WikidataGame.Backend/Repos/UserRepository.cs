using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Repos
{
    public class UserRepository : Repository<User, string>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context) { }

        public User CreateOrUpdateUser(string deviceId, string pushUrl)
        {
            var user = Get(deviceId);
            if (user == null)
            {
                user = new Models.User
                {
                    DeviceId = deviceId,
                    PushChannelUrl = pushUrl
                };
                Add(user);
            }
            else
            {
                user.PushChannelUrl = pushUrl;
                Update(user);
            }
            return user;
        }
    }
}
