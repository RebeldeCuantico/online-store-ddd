using Catalog.Domain;
using FluentAssertions;

namespace CatalogTest
{
    public class NameTest
    {
        [Fact]
        public void We_Cant_Create_Name_Null()
        {
            Action action = () => { _ = new Name(null); };

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void We_Cant_Create_Name_Empty()
        {
            Action action = () => { _ = new Name(string.Empty); };

            action.Should().Throw<ArgumentException>();
        }
    }
}