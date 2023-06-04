using AutoFixture;
using Catalog.Domain;

namespace Catalog.Application
{
    public class GetAllCategoriesHandler
    {
        public static async Task<List<Category>> Handle(GetCategoriesQuery getCategoriesQuery)
        {
            //Do stuff
            var fixture = new Fixture();
            return fixture.Create<List<Category>>();
        }
    }
}
