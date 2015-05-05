namespace ConsoleUI
{
    public class Command
    {
        public Command(string inputCommand)
        {
            if (!string.IsNullOrWhiteSpace(inputCommand))
            {
                string[] arr = inputCommand.Split(' ');
                if (arr.Length > 1)
                {
                    Name = arr[0];
                    int commandArg;

                    if (int.TryParse(arr[1], out commandArg))
                    {
                        Argument = commandArg;
                    }
                }
                else
                {
                    Name = inputCommand;
                }
            }
        }

        public string Name { get; private set; }

        public int? Argument { get; private set; }

        public bool IsAdminCommand
        {
            get { return !string.IsNullOrWhiteSpace(Name) && Name.StartsWith(":"); }
        }
    }
}