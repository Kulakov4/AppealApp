using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppTestCase.Data;
using WebAppTestCase.Interfaces;
using WebAppTestCase.Models;

namespace WebAppTestCase.Services
{
    public class AppealService : CrudService<Appeal>, IAppealService
    {
        private readonly int truncateMessageLength = 0;
        public AppealService(ApplicationDbContext DbContext, IOptions<Configuration.AppealConfig> config) : base(DbContext)
        {
            truncateMessageLength = config.Value.TruncateMessageLength;
            if (truncateMessageLength == 0)
                truncateMessageLength = Int32.MaxValue;
        }

        public IQueryable<Appeal> GetPageItems(int page, int pageSize)
        {
            var skipCount = (page - 1) * pageSize;
            return GetAll().Select(item => new Appeal
            {
                Id = item.Id,
                CreatedAt = item.CreatedAt,
                Email = item.Email,
                Phone = item.Phone,
                Message = (item.Message.Length > truncateMessageLength) ? item.Message.Remove(truncateMessageLength) + "..." : item.Message
            }).OrderByDescending(f => f.CreatedAt).Skip(skipCount).Take(pageSize);
        }

        public async Task<int> GetSkipItemCount(DateTime appealTime)
        {
            return await GetAll().OrderByDescending(f => f.CreatedAt).Where(item => item.CreatedAt.CompareTo(appealTime) > 0).CountAsync();
        }
    }
}
