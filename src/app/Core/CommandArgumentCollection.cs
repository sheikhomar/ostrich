using System;

namespace ostrich.Core
{
    public class CommandArgumentCollection
    {
        public CommandArgumentCollection(string commandString)
        {
            if (commandString != null)
                RawArguments = commandString.Trim().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            else
                RawArguments = new string[0];
        }

        public CommandArgumentCollection(string[] rawArguments)
        {
            if (rawArguments == null) 
                throw new ArgumentNullException("rawArguments");

            RawArguments = rawArguments;
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
                if (index >= 0 && index < RawArguments.Length)
                    return RawArguments[index];

                return null;
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