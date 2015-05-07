using System;

namespace ostrich.Core.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string userName)
            : base(String.Format("User '{0}' was not found.", userName))
        {
            if (string.IsNullOrWhiteSpace(userName)) 
                throw new ArgumentNullException("userName");

            UserName = userName;
        }

        public string UserName { get; private set; }
    }
}