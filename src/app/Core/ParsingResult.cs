using System;

namespace ostrich.Core
{
    public class ParsingResult
    {
        public ParsingResult(ICommandProcessor processor, CommandArgumentCollection arguments)
        {
            if (processor == null) 
                throw new ArgumentNullException("processor");
            if (arguments == null) 
                throw new ArgumentNullException("arguments");

            Processor = processor;
            Arguments = arguments;
        }

        public ICommandProcessor Processor { get; private set; }
        public CommandArgumentCollection Arguments { get; private set; }
    }
}