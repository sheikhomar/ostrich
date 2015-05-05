using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace ConsoleUI
{
    

    class Program
    {
        static void Main(string[] args)
        {
            // Transaktioen skal skrives ud på fil.
            IStregsystem stregsystem = new Stregsystem();
            StregsystemCli cli = new StregsystemCli(stregsystem);
            StregsystemCommandParser parser = new StregsystemCommandParser(cli, stregsystem);
            cli.Start(parser);
        }
    }
}
