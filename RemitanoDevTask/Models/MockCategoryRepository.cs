using System.Collections.Generic;

namespace RemitanoDevTask.Models 
{
    public class MockCategoryRepository: ICategoryRepository
    {
        public IEnumerable<Category> Categories
        {
            get
            {
                return new List<Category>
                {
                    new Category{CategoryId=1, CategoryName="Shared", Description="All movies that are shared"},
                    new Category{CategoryId=2, CategoryName="NotShared", Description="All movies that are not shared"}
                };
            }
            
        }
    }
}
