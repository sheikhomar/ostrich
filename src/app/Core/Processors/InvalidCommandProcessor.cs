namespace ostrich.Core.Processors
{
    public class InvalidCommandProcessor : CommandProcessor
    {
        public const string EmptyCommandErrorMessage = "Please command and I will try to obey.";

        public InvalidCommandProcessor(IUserInterface ui, IBackendSystem system) : base(ui, system)
        {
        }

        protected override void ProcessInternal(CommandArgumentCollection args)
        {
            if (args.Count == 0)
                UI.DisplayGeneralError(EmptyCommandErrorMessage);
            else
                UI.DisplayTooManyArgumentsError();
        }
    }
}