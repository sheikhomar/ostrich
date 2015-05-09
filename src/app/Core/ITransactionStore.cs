using System.Collections.Generic;

namespace ostrich.Core
{
    public interface ITransactionStore : IEnumerable<Transaction>
    {
        void Save(Transaction transaction);
    }
}