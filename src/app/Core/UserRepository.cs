using System;
using System.Collections.Generic;

namespace ostrich.Core
{
    public class UserRepository
    {
        private readonly IDictionary<int, User> users;

        public UserRepository()
        {
            users = new Dictionary<int, User>();
        }

        public void Add(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            // TODO: Make custom exception here.
            if (users.ContainsKey(user.UserID))
                throw new Exception("User already exists.");

            users.Add(user.UserID, user);
        }

        public IEnumerable<User> GetUsers()
        {
            return users.Values;
        }
    }
}