using isc.time.report.be.domain.Entity.Sync;
using isc.time.report.be.infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.infrastructure.Repositories.Sync
{
    public class OutboxPositionRepository
    {
        private readonly DBContext _dbContext;
        public OutboxPositionRepository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<OutboxPosition>> GetPendingOutboxPositionsAsync()
        {
            return await _dbContext.OutboxPositions
               .Where(x => x.ProcessedAt == null
                        && x.NextAttemptAt <= DateTime.UtcNow)
               .OrderBy(x => x.CreatedAt)
               .Take(100)
               .ToListAsync();
        }

        public async Task MarkAsProcessedAsync(Guid outboxId)
        {
            await _dbContext.OutboxPositions
                .Where(x => x.OutboxId == outboxId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.ProcessedAt, DateTime.UtcNow)
                    .SetProperty(x => x.ErrorMessage, (string?)null));
        }

        public async Task MarkAsFailedAsync(Guid outboxId, string errorMessage)
        {
            var record = await _dbContext.OutboxPositions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.OutboxId == outboxId);
            if (record is null) return;

            record.Attempts++;
            var minutesDelay = Math.Pow(2, record.Attempts) * 5;

            await _dbContext.OutboxPositions
                .Where(x => x.OutboxId == outboxId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.Attempts, record.Attempts)
                    .SetProperty(x => x.ErrorMessage, errorMessage)
                    .SetProperty(x => x.NextAttemptAt, DateTime.UtcNow.AddMinutes(minutesDelay)));
        }

    }
}
