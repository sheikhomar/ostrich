using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ostrich.Core
{
    public class TransactionFileStore : ITransactionStore
    {

        public TransactionFileStore(string filePath)
        {
            if (filePath == null) 
                throw new ArgumentNullException("filePath");

            FilePath = filePath;
        }

        public string FilePath { get; private set; }

        public void Save(Transaction transaction)
        {
            using (StreamWriter writer = new StreamWriter(
                File.Open(FilePath, FileMode.Append, FileAccess.Write, FileShare.Read)))
            {
                writer.WriteLine(transaction.ToString());
            }
        }

        public IList<Transaction> Load()
        {
            // TODO: Implement if there is time.
            return new List<Transaction>();
        }
    }
}