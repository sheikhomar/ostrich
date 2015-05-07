﻿using System.Collections.Generic;
using ostrich.Core.Exceptions;

namespace ostrich.Core
{
    public interface IUserInterface
    {
        void DisplayUserNotFound(string userName);
        void DisplayProductNotFound(int productId);
        void DisplayUserInfo(User user, IEnumerable<BuyTransaction> latestTransactions);
        void DisplayTooManyArgumentsError();
        void DisplayAdminCommandNotFoundMessage(Command cmd);
        void DisplayUserBuysProduct(BuyTransaction transaction);
        void Close();
        void DisplayInsufficientCash(User user, Product product);
        void DisplayGeneralError(string errorString);
        void DisplayProductNotSaleable(Product product);
        void DisplayCashInserted(InsertCashTransaction transaction);
        void DisplaceProducts(IEnumerable<Product> products);
        void DisplayUsers(IEnumerable<User> users);
        
    }
}