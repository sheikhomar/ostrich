using System.Linq;
using ostrich.Core.Exceptions;

namespace ostrich.Core
{
    public class UserDetailsController : Controller
    {
        public UserDetailsController(IUserInterface ui, IBackendSystem system) : base(ui, system)
        {
        }

        protected override void ProcessInternal(CommandArgumentCollection args)
        {
            try
            {
                string userName = args[0];
                User user = System.GetUser(userName);
                var transactions = System.GetTransactionList(user);
                var latestTransactions = transactions
                    .Where(t => t is BuyTransaction)
                    .Cast<BuyTransaction>()
                    .OrderBy(t => t.Date)
                    .Take(10);

                UI.DisplayUserInfo(user, latestTransactions);
            }
            catch (UserNotFoundException exception)
            {
                UI.DisplayUserNotFound(exception.UserName);
            }
        }
    }
}