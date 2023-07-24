using Catalog.Domain;
using FluentAssertions;

namespace CatalogTest
{
    public class PriceTest
    {
        [Theory]
        [InlineData("USD", "EUR", 1, 1.1)]
        [InlineData("EUR", "USD", 1, 0.9091)]
        public void Change_Amount_Currency(string fromCode, string toCode, decimal fromAmount, decimal expected)
        {
            var fromPrice = new Price(fromAmount, fromCode);
            var newPrice = fromPrice.ChangeCurrency(toCode);

            newPrice.Amount.Should().Be(expected);
            newPrice.Currency.Code.Should().Be(toCode);
        }
    }
}
