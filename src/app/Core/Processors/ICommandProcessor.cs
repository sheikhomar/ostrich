namespace ostrich.Core
{
    public interface ICommandProcessor
    {
        void Process(CommandArgumentCollection args);
    }
}