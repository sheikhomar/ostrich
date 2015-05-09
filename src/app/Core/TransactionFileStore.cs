using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ostrich.Core
{
    public class TransactionFileStore : ITransactionStore
    {
        private readonly IList<Transaction> transactions;

        public TransactionFileStore(string filePath)
        {
            if (filePath == null) 
                throw new ArgumentNullException("filePath");

            FilePath = filePath;

            transactions = new List<Transaction>();
        }

        public string FilePath { get; private set; }

        public void Save(Transaction transaction)
        {
            transactions.Add(transaction);

            using (StreamWriter writer = new StreamWriter(
                File.Open(FilePath, FileMode.Append, FileAccess.Write, FileShare.Read)))
            {
                writer.WriteLine(transaction.ToString());
            }
        }

        public IEnumerator<Transaction> GetEnumerator()
        {
            return transactions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void LoadFromFile()
        {
            // TODO: Implement if there is time.
        }
    }
}