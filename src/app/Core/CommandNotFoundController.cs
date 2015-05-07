namespace ostrich.Core
{
    public class CommandNotFoundController : Controller
    {
        public CommandNotFoundController(IUserInterface ui, IBackendSystem system) : base(ui, system)
        {
        }

        protected override void ProcessInternal(CommandArgumentCollection args)
        {
            if (args.Count == 0)
            {
                UI.DisplayGeneralError("Please command and I will try to obey.");
            }
            else
            {
                UI.DisplayTooManyArgumentsError();
            }
        }
    }
}