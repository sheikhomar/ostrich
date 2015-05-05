using System;
using System.Collections.Generic;
using System.IO.Pipes;

namespace ConsoleUI
{
    public class StregsystemCommandParser
    {
        private readonly IUserInterface ui;
        private readonly IStregsystem stregsystem;
        private readonly IDictionary<string, Action<int>> commands;

        public StregsystemCommandParser(IUserInterface ui, IStregsystem stregsystem)
        {
            if (ui == null) throw new ArgumentNullException("ui");
            if (stregsystem == null) throw new ArgumentNullException("stregsystem");

            this.ui = ui;
            this.stregsystem = stregsystem;

            commands = new Dictionary<string, Action<int>>
            {
                {":activate", ActivateProduct},
                {":deactivate", DeactivateProduct},
            };
        }

        public void Parse(string stringToParse)
        {
            if (!string.IsNullOrWhiteSpace(stringToParse))
            {
                string[] arr = stringToParse.Split(' ');
                if (arr.Length > 1)
                {
                    string command = arr[0];
                    int commandArg;

                    if (int.TryParse(arr[1], out commandArg) && commands.ContainsKey(command))
                    {
                        commands[command](commandArg);
                    }
                }
            }
        }

        private void ActivateProduct(int productId)
        {
            Product product = stregsystem.GetProduct(productId);
            if (product.Active)
                ui.DisplayGeneralError(String.Format("Product {0} is already active.", product.ProductID));
            else
                product.Active = true;
        }

        private void DeactivateProduct(int productId)
        {
            Product product = stregsystem.GetProduct(productId);
            if (!product.Active)
                ui.DisplayGeneralError(String.Format("Product '{0}' is not active.", product.ProductID));
            else
                product.Active = false;
        }
    }
}