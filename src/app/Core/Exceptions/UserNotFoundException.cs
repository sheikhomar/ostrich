using System;

namespace ostrich.Core.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; private set; }

        public override string Message
        {
            get { return String.Format("User '{0}' was not found.", UserName); }
        }
    }
}