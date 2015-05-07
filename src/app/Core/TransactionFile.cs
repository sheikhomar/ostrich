using System;
using System.IO;

namespace ostrich.Core
{
    public class TransactionFile
    {
        public TransactionFile(string filePath)
        {
            if (filePath == null) 
                throw new ArgumentNullException("filePath");

            FilePath = filePath;
        }

        public string FilePath { get; private set; }

        public void Write(Transaction transaction)
        {
            using (StreamWriter writer = new StreamWriter(
                File.Open(FilePath, FileMode.Append, FileAccess.Write, FileShare.Read)))
            {
                transaction.Log(writer);
            }
        }
    }
}