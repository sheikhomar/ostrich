using System;

namespace ConsoleUI
{
    public class User : IComparable<User>
    {
        private string userName;
        private string email;
        private int balance;

        public User(int userId, string firstName, string lastName)
        {
            if (firstName == null) 
                throw new ArgumentNullException("firstName");
            if (lastName == null) 
                throw new ArgumentNullException("lastName");

            UserID = userId;
            FirstName = firstName;
            LastName = lastName;
        }

        public int UserID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

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

        public int Balance
        {
            get { return balance; }
            set
            {
                if (!IsNewBalanceValid(value))
                    throw new ArgumentException("Balance is not valid.");
                
                balance = value;
            }
        }

        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        public bool Equals(User user)
        {
            if (user == null)
                return false;
            return user.UserID == UserID;
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
            return string.Format("{0} {1} ({2}) Balance={3}", FirstName, LastName, UserName, Balance);
        }

        private bool IsNewBalanceValid(int input)
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