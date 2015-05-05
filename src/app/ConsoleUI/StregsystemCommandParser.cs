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
            Command cmd = new Command(stringToParse);
            if (cmd.IsAdminCommand)
            {
                if (":q".Equals(cmd.Name) || ":quit".Equals(cmd.Name))
                    ui.Close();
                else if (commands.ContainsKey(cmd.Name) && cmd.Argument.HasValue)
                    commands[cmd.Name](cmd.Argument.Value);
                else
                    ui.DisplayGeneralError("Invalid administration command.");
            }
            else
            {
                // TODO: Non administrative actions.
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