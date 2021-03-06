using System;

namespace ostrich.Core
{
    public class CommandArgumentCollection
    {
        public CommandArgumentCollection(string commandString)
        {
            if (commandString != null)
                RawArguments = commandString.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            else
                RawArguments = new string[0];
        }

        public string[] RawArguments { get; private set; }

        public int Count
        {
            get { return RawArguments.Length; }
        }

        public string this[int index]
        {
            get
            {
                if (index < 0 || index >= RawArguments.Length)
                    throw new ArgumentOutOfRangeException("index");

                return RawArguments[index];
            }
        }
        
        public int? GetAsInt(int index)
        {
            int tempVal;
            if (int.TryParse(this[index], out tempVal))
                return tempVal;
            return null;
        }
    }
}