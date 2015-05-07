namespace ostrich.Core
{
    public interface IController
    {
        void Process(CommandArgumentCollection args);
    }
}