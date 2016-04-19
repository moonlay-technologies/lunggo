using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.ProductBase.Model
{
    public class Currency
    {
        public string Symbol { get; private set; }
        public decimal Rate { get; private set; }

        public Currency(string symbol)
        {
            if (!ValidateSymbol(symbol))
                return;
            Symbol = symbol.ToUpper();
            Rate = GetLatestRate();
        }

        public decimal GetLatestRate()
        {
            throw new NotImplementedException();
        }

        public static decimal GetLatestRate(string symbol)
        {
            return !ValidateSymbol(symbol) ? 0M : new Currency(symbol).Rate;
        }

        public static void SetRate(string symbol)
        {
            if (!ValidateSymbol(symbol))
                return;
            symbol = symbol.ToUpper();
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return Symbol;
        }

        public static implicit operator string(Currency currency)
        {
            return currency.ToString();
        }

        static bool ValidateSymbol(string symbol)
        {
            return symbol != null && symbol.Length == 3;
        }
    }
}
