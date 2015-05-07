using System;
using System.Collections.Generic;

namespace ostrich.Core
{
    public class StregsystemCommandParser
    {
        private readonly IDictionary<CommandType, IController> controllers;

        public StregsystemCommandParser(IUserInterface ui, IStregsystem stregsystem)
        {
            if (ui == null) 
                throw new ArgumentNullException("ui");

            if (stregsystem == null) 
                throw new ArgumentNullException("stregsystem");

            controllers = new Dictionary<CommandType, IController>
            {
                {CommandType.Unknown, new CommandNotFoundController(ui, stregsystem)},
                {CommandType.Administration, new AdministrationController(ui, stregsystem)},
                {CommandType.UserDetails, new UserDetailsController(ui, stregsystem)},
                {CommandType.QuickBuy, new QuickBuyController(ui, stregsystem)},
            };
        }

        public void Parse(string command)
        {
            CommandArgumentCollection args = new CommandArgumentCollection(command);
            IController controller = controllers[CommandType.Unknown];

            if (AdministrationController.IsAdminCommand(command))
                controller = controllers[CommandType.Administration];
            else if (args.Count == 1)
                controller = controllers[CommandType.UserDetails];
            else if (args.Count == 2 || args.Count == 3)
                controller = controllers[CommandType.QuickBuy];

            controller.Process(args);
        }
    }
}