using System;

namespace ostrich.Core.Exceptions
{
    public class DuplicateUserException : Exception
    {
        public DuplicateUserException(User user, string message)
            : base(message)
        {
            User = user;
        }

        public User User { get; private set; }
    }
}