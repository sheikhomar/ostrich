using ostrich.Core;

namespace ostrich.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            IBackendSystem backendSystem = new DefaultBackendSystem();
            CommandLineInterface cli = new CommandLineInterface();
            CommandParser parser = new CommandParser(cli, backendSystem);
            cli.Start(parser);
        }
    }
}
