using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppTestCase.Models;

namespace WebAppTestCase.Interfaces
{
    public interface IAppealService: ICrudService<Appeal>
    {
        public IQueryable<Appeal> GetPageItems(int page, int pageSize);
        public Task<int> GetSkipItemCount(DateTime appealTime);
    }
}
