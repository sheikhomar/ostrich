using System;
using System.Collections.Generic;
using ostrich.Core.Processors;

namespace ostrich.Core
{
    public class CommandParser
    {
        private readonly IDictionary<CommandType, ICommandProcessor> processors;

        public CommandParser(IUserInterface ui, IBackendSystem system)
        {
            if (ui == null) 
                throw new ArgumentNullException("ui");

            if (system == null) 
                throw new ArgumentNullException("system");

            processors = new Dictionary<CommandType, ICommandProcessor>
            {
                {CommandType.Unknown, new InvalidCommandProcessor(ui, system)},
                {CommandType.Administration, new AdministrationCommandProcessor(ui, system)},
                {CommandType.UserDetails, new UserDetailsCommandProcessor(ui, system)},
                {CommandType.QuickBuy, new QuickBuyCommandProcessor(ui, system)},
            };
        }

        public ParsingResult Parse(string command)
        {
            CommandArgumentCollection args = new CommandArgumentCollection(command);
            ICommandProcessor commandProcessor = processors[CommandType.Unknown];

            if (AdministrationCommandProcessor.CanProcess(command))
                commandProcessor = processors[CommandType.Administration];
            else if (args.Count == 1)
                commandProcessor = processors[CommandType.UserDetails];
            else if (args.Count == 2 || args.Count == 3)
                commandProcessor = processors[CommandType.QuickBuy];

            return new ParsingResult(commandProcessor, args);
        }
    }
}