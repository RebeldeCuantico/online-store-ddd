using Catalog.Domain;
using Common.Domain;
using FluentAssertions;

namespace CatalogTest
{
    public class CategoryTest
    {
        private static Category BuildCategory(string name, string description)
        {
            var categoryName = new Name(name);
            var categoryDescription = new Description(description);
            var id = new EntityId(Guid.NewGuid());

            var category = new Category(categoryName, categoryDescription, id);

            return category;
        }

        [Theory]
        [InlineData("fooName", "fooDescription")]
        [InlineData("barName", "barDescription")]
        public void Creation_Should_works(string name, string description)
        {
            var category = BuildCategory(name, description);

            category.Name.Value.Should().Be(name);
            category.Description.Value.Should().Be(description);
        }

        [Theory]
        [InlineData("foo", "fooDescription", "bar")]
        public void Change_Name(string name, string description, string newName) 
        {
            var category = BuildCategory(name, description);
            category.Name.Value.Should().Be(name);

            category.ChangeName(newName);
            category.Name.Value.Should().Be(newName);
        }

        [Theory]
        [InlineData("foo", "fooDescription", "barDescription")]
        public void Change_Description(string name, string description, string newDescription)
        {
            var category = BuildCategory(name, description);
            category.Description.Value.Should().Be(description);

            category.ChangeDescription(newDescription);
            category.Description.Value.Should().Be(newDescription);
        }

        [Theory]
        [InlineData("foo", "bar", null)]
        [InlineData("foo", "bar", "")]
        public void Change_Name_With_Null_Or_Empty_Should_Throw_Exception(string name, string description, string newName)
        {
            var category = BuildCategory(name, description);
            Action action = () => { category.ChangeName(newName); };

            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("foo", "bar", null)]
        [InlineData("foo", "bar", "")]
        public void Change_Description_With_Null_Or_Empty_Should_Throw_Exception(string name, string description, string newDescription)
        {
            var category = BuildCategory(name, description);
            Action action = () => { category.ChangeDescription(newDescription); };

            action.Should().Throw<ArgumentException>();
        }
    }
}
