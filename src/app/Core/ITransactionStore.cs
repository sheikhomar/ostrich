using System.Collections.Generic;

namespace ostrich.Core
{
    public interface ITransactionStore
    {
        void Save(Transaction transaction);
        IList<Transaction> Load();
    }
}