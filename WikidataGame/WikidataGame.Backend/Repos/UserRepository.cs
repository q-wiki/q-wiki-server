using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Repos
{
    public class UserRepository : Repository<User, Guid>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context) { }

        public async Task<User> CreateOrUpdateUserAsync(string firebaseUserId, string pushToken, string username)
        {
            User user = await SingleOrDefaultAsync(u => u.FirebaseUserId == firebaseUserId);
            if(user == null || user.Username != username)
            {
                if((await SingleOrDefaultAsync(u => u.Username == username)) != null)
                {
                    throw new UsernameTakenException();
                }
            }
            if (user == null)
            {
                user = new User
                {
                    FirebaseUserId = firebaseUserId,
                    PushToken = pushToken,
                    Username = username
                };
                await AddAsync(user);
            }
            else
            {
                user.PushToken = pushToken;
                user.Username = username;
                Update(user);
            }
            return user;
        }

        [Serializable]
        public class UsernameTakenException : Exception
        {
            public UsernameTakenException()
            {
            }

            public UsernameTakenException(string message) : base(message)
            {
            }

            public UsernameTakenException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected UsernameTakenException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }
}
