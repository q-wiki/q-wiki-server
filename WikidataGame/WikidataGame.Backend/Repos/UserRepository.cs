using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Repos
{
    public class UserRepository : Repository<User, string>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context) { }

        public User CreateOrUpdateUser(string firebaseUserId, string pushToken, string username)
        {
            User user = SingleOrDefault(u => u.FirebaseUserId == firebaseUserId);
            if(user == null || user.Username != username)
            {
                if(SingleOrDefault(u => u.Username == username) != null)
                {
                    throw new UsernameTakenException();
                }
            }
            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    FirebaseUserId = firebaseUserId,
                    PushToken = pushToken,
                    Username = username
                };
                Add(user);
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
