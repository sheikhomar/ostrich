using System;
using System.Collections.Generic;
using System.Linq;
using ostrich.Core.Exceptions;

namespace ostrich.Core
{
    public class UserRepository
    {
        private readonly IDictionary<string, User> users;

        public UserRepository()
        {
            users = new Dictionary<string, User>();
        }

        public void Add(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (users.ContainsKey(user.UserName))
                throw new DuplicateUserException(user, "User with same username already exists.");

            if (users.Values.Contains(user))
                throw new DuplicateUserException(user, "User with same ID already exists.");

            users.Add(user.UserName, user);
        }

        public IEnumerable<User> GetUsers()
        {
            return users.Values;
        }

        public User this[string userName]
        {
            get
            {
                if (!users.ContainsKey(userName))
                    throw new UserNotFoundException(userName);
                return users[userName];
            }
        }
    }
}