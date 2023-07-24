using Common.Domain;
using GuardNet;

namespace Catalog.Domain
{
    public class Price : ValueObject
    {
        private Price() { }

        public Price(decimal price, string currencyCode)
        {
            Guard.NotLessThanOrEqualTo(price, 0, nameof(price), "The price cannot be negative or zero");
            Amount = price;
            Currency = Currency.GetByCode(currencyCode);
        }

        public decimal Amount { get; }
        public Currency Currency { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }

        public Price ChangeCurrency(string currencyCode)
        {
            var currency = Currency.GetByCode(currencyCode);
            var newAmount = this.Amount * GetConversionRate(Currency, currency);

            return new Price(newAmount, currencyCode);
        }

        //TODO: Review how to get conversionRateFromInternet or database
        protected decimal GetConversionRate(Currency from, Currency to)
        {
            if(from.Code == Currency.USDollar.Code && to.Code == Currency.Euro.Code)
            {
                return 1.1M;
            }
            if (from.Code == Currency.Euro.Code && to.Code == Currency.USDollar.Code)
            {
                return 0.9091M;
            }

            throw new Exception("Bad conversion");
        }
    }
}
