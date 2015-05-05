using System;

namespace ConsoleUI
{
    public class User : IComparable<User>
    {
        private string firstName;
        private string lastName;
        private string userName;
        private string email;
        private double balance;

        public User(int userId, string firstName)
        {
            UserID = userId;
            FirstName = firstName;

        }

        // TODO: Must be unique
        public int UserID { get; set; }

        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("First name cannot be null.");
                firstName = value;
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Last name cannot be null.");
                lastName = value;
            }
        }

        public string UserName
        {
            get { return userName; }
            set
            {
                if (!IsUserNameValid(value))
                    throw new ArgumentException("User is not valid.");
                userName = value;
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                if (!IsEmailValid(value))
                    throw new ArgumentException("Email is not valid.");
                email = value;
            }
        }

        public double Balance
        {
            get { return balance; }
            set
            {
                if (!IsBalanceValid(value))
                    throw new ArgumentException("Balance is not valid.");
                
                balance = value;
            }
        }

        public int CompareTo(User other)
        {
            return UserID.CompareTo(other.UserID);
        }

        public override bool Equals(object obj)
        {
            User user = obj as User;
            if (user != null)
                return user.UserID == UserID;
            return false;
        }

        public override int GetHashCode()
        {
            return UserID.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} {1} ({2})", FirstName, LastName, Email);
        }

        private bool IsBalanceValid(double input)
        {
            return true;
        }

        public bool IsUserNameValid(string input)
        {
            // TODO: Fix this
            return true;
        }

        private bool IsEmailValid(string input)
        {
            return true;
        }
    }
}