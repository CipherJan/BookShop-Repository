using System;

namespace BookShop.Core.Exceptions
{
    public class InsufficientFundsOnBalanceException : Exception
    {
        public int ShopId { get; private set; }
        public double BalanceInShop { get; private set; }
        public double WriteOffAmount { get; private set; }

        public InsufficientFundsOnBalanceException(string message, int shopId, double balanceInShop, double writeOffAmount):base(message)
        {
            ShopId = shopId;
            BalanceInShop = balanceInShop;
            WriteOffAmount = writeOffAmount;
        }
    }
}
