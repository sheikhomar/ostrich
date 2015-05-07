using ostrich.Core;

namespace ostrich.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            IStregsystem stregsystem = new Stregsystem();
            CommandLineInterface cli = new CommandLineInterface();
            StregsystemCommandParser parser = new StregsystemCommandParser(cli, stregsystem);
            cli.Start(parser);
        }
    }
}
