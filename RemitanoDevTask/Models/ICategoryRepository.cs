using System.Collections.Generic;

namespace RemitanoDevTask.Models
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> Categories { get; }
    }
}
