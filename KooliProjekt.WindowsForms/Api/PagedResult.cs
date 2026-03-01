using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.WindowsForms
{
    [ExcludeFromCodeCoverage]
    public class PagedResult<T> : PagedResultBase
    {
        public IList<T> Results { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PagedResult()
        {
            Results = new List<T>();
        }
    }
}