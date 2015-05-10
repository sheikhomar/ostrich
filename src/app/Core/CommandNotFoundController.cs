namespace ostrich.Core
{
    public class CommandNotFoundController : Controller
    {
        public const string EmptyCommandErrorMessage = "Please command and I will try to obey.";

        public CommandNotFoundController(IUserInterface ui, IBackendSystem system) : base(ui, system)
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