using System;

namespace ostrich.Core
{
    public abstract class Controller : IController
    {
        protected Controller(IUserInterface ui, IBackendSystem system)
        {
            if (ui == null) 
                throw new ArgumentNullException("ui");
            if (system == null) 
                throw new ArgumentNullException("system");

            UI = ui;
            System = system;
        }

        public void Process(CommandArgumentCollection args)
        {
            if (args == null) 
                throw new ArgumentNullException("args");

            ProcessInternal(args);
        }

        protected IUserInterface UI { get; private set; }

        protected IBackendSystem System { get; private set; }

        protected abstract void ProcessInternal(CommandArgumentCollection args);
    }
}