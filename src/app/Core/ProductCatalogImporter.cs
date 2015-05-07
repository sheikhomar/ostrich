using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ostrich.Core
{
    public class ProductCatalogImporter
    {
        private readonly TextReader reader;
        private readonly Regex unwantedCharsRegex;

        public ProductCatalogImporter(TextReader reader)
        {
            if (reader == null) throw new ArgumentNullException("reader");

            this.reader = reader;
            unwantedCharsRegex = new Regex("<[^>]+>|\"", RegexOptions.Compiled);
        }

        public ProductCatalog Import()
        {
            var catalog = new ProductCatalog();
            var csv = new CsvReader(reader);

            csv.Read(); // Skip header
            while (csv.Read())
                catalog.Add(ParseRow(csv));

            return catalog;
        }

        private Product ParseRow(CsvReader csv)
        {
            int id = csv.GetInt(0);
            string name = StripUnwantedCharacters(csv.GetString(1));
            int price = csv.GetInt(2);
            bool active = csv.GetInt(3) == 1;

            return new Product(id, name, price) { Active = active };
        }

        private string StripUnwantedCharacters(string input)
        {
            return unwantedCharsRegex.Replace(input, "");
        }
    }
}