using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ostrich.Core
{
    public class User : IComparable<User>
    {
        public const int LowBalanceLimit = 5000;

        private static readonly Regex UserNameValidCharsRegex = new Regex("^[a-z0-9_]+$", RegexOptions.Compiled);
        private static readonly Regex EmailCheckerRegex = new Regex(
            "^[a-z0-9_.-]+@[a-z0-9_][a-z0-9_.-]+[a-z0-9_]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private string email;

        public User(int userId, string firstName, string lastName, string userName)
        {
            if (firstName == null) 
                throw new ArgumentNullException("firstName");
            if (lastName == null) 
                throw new ArgumentNullException("lastName");
            if (userName == null) 
                throw new ArgumentNullException("userName");
            if (!IsUserNameValid(userName))
                throw new ArgumentException("Username is not valid.", "userName");

            UserID = userId;
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
        }

        public int UserID { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string UserName { get; private set; }

        public int Balance { get; set; }

        public string Email
        {
            get { return email; }
            set
            {
                if (!IsEmailValid(value))
                    throw new ArgumentException("Email is not valid.", "value");
                email = value;
            }
        }

        public bool HasLowBalance
        {
            get { return Balance < LowBalanceLimit; }
        }

        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        public string FormattedBalance
        {
            get { return string.Format("{0:N2} kr.", Balance/100D);; }
        }

        public bool Equals(User other)
        {
            return other != null && other.UserID == UserID;
        }

        public int CompareTo(User other)
        {
            return UserID.CompareTo(other.UserID);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as User);
        }

        public override int GetHashCode()
        {
            return UserID.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} {1} ({2})", FirstName, LastName, Email);
        }

        private bool IsUserNameValid(string input)
        {
            return input != null && UserNameValidCharsRegex.IsMatch(input);
        }

        private bool IsEmailValid(string input)
        {
            return EmailCheckerRegex.IsMatch(input);
        }
    }
}