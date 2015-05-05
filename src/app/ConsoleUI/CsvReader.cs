using System;
using System.IO;

namespace ConsoleUI
{
    public class CsvReader
    {
        private readonly TextReader reader;
        private string[] fields;

        public CsvReader(TextReader reader)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            this.reader = reader;
        }

        public bool Read()
        {
            string currentLine = reader.ReadLine();
            if (currentLine != null)
                fields = currentLine.Split(';');
            else
                fields = null;
            return currentLine != null;
        }

        public string GetString(int fieldIndex)
        {
            EnsureFieldsAreNotNull(fieldIndex);

            return fields[fieldIndex];
        }

        public int GetInt(int fieldIndex)
        {
            EnsureFieldsAreNotNull(fieldIndex);

            int value;
            if (!int.TryParse(fields[fieldIndex], out value))
                throw new ArgumentException("Field cannot be converted to Int32.", "fieldIndex");

            return value;
        }

        private void EnsureFieldsAreNotNull(int fieldIndex)
        {
            if (fields == null)
                throw new InvalidOperationException("Call Read() to read the next line into the buffer.");

            if (fieldIndex < 0 || fieldIndex >= fields.Length)
                throw new ArgumentOutOfRangeException("fieldIndex");
        }
    }
}