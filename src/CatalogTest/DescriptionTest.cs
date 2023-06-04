using Catalog.Domain;
using FluentAssertions;

namespace CatalogTest
{
    public class DescriptionTest
    {
        [Fact]
        public void We_Cant_Create_Description_Null()
        {
            Action action = () => { _ = new Description(null); };

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void We_Cant_Create_Description_Empty()
        {
            Action action = () => { _ = new Description(string.Empty); };

            action.Should().Throw<ArgumentException>();
        }
    }
}
