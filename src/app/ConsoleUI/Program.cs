using ostrich.Core;

namespace ostrich.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            IBackendSystem backendSystem = new DefaultBackendSystem();
            CommandLineInterface cli = new CommandLineInterface();
            StregsystemCommandParser parser = new StregsystemCommandParser(cli, backendSystem);
            cli.Start(parser);
        }
    }
}
