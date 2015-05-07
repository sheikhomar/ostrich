using System;

namespace ostrich.Core
{
    public class Command
    {
        public Command(string inputCommand)
        {
            if (!string.IsNullOrWhiteSpace(inputCommand))
            {
                RawArguments = inputCommand.Split(' ');
                Name = RawArguments.Length > 1 ? RawArguments[0] : inputCommand;
            }
            else
            {
                RawArguments = new string[0];
                Name = String.Empty;
            }
        }

        public string Name { get; private set; }
        

        public string[] RawArguments { get; set; }

        public bool IsAdminCommand
        {
            get { return !string.IsNullOrWhiteSpace(Name) && Name.StartsWith(":"); }
        }

        public int? GetInt(int argumentIndex)
        {
            int? returnVal = null;

            if (argumentIndex > 0 && argumentIndex < RawArguments.Length)
            {
                int tempVal;
                if (int.TryParse(RawArguments[argumentIndex], out tempVal))
                    returnVal = tempVal;
            }
            
            return returnVal;
        }
    }
}