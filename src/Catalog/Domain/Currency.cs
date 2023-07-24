using Common.Domain;
using GuardNet;

namespace Catalog.Domain
{
    public class Currency : ValueObject
    {
        public string Code { get; }

        public string Symbol { get; }

        public static Currency USDollar = new("USD", "$");
        public static Currency Euro = new("EUR", "€");

        public Currency(string code, string symbol)
        {
            Guard.NotNullOrEmpty(code, nameof(code), "The code Can't be null or empty");
            Guard.NotNullOrEmpty(symbol, nameof(symbol), "The symbol Can't be null or empty");

            Code = code;    
            Symbol = symbol;
        }
        private Currency()
        {
        }

        public static Currency GetByCode(string currencyCode)
        {
            return currencyCode switch
            {
                "USD" => Currency.USDollar,
                "EUR" => Currency.Euro,
                _ => throw new Exception($"the value {currencyCode} is not a valid Currency")
            };
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
            yield return Symbol;
        }
    }
}
