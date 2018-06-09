using System.Collections.Generic;

namespace Pactor.Infra.DAL.ORM.Queries
{
    public class PagedResult<T>
    {
        public int TotalItems { get; set; }
        public IEnumerable<T> PageOfResults { get; set; }
    }
}
