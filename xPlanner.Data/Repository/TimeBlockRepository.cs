using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core;
using System.Threading.Tasks;
using xPlanner.Domain.Entities;

namespace xPlanner.Data.Repository;

internal class TimeBlockRepository : IRepository<TimeBlock>
{
    private readonly AppDbContext dbContext;

    public TimeBlockRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<TimeBlock> Add(TimeBlock timeBlock)
    {
        await dbContext.TimeBlocks.AddAsync(timeBlock);
        await dbContext.SaveChangesAsync();

        return timeBlock;
    }

    public async Task<TimeBlock> Delete(int id)
    {
        var timeBlock = await GetById(id);

        dbContext.TimeBlocks.Remove(timeBlock);
        await dbContext.SaveChangesAsync();

        return timeBlock;
    }

    public async Task<List<TimeBlock>> GetAll()
    {
        return await dbContext.TimeBlocks
            .OrderBy(timeBlock => timeBlock.Id)
            .ToListAsync();
    }

    public async Task<TimeBlock> GetById(int id)
    {
        return await dbContext.TimeBlocks
            .FirstOrDefaultAsync(timeBlock => timeBlock.Id == id) ??
             throw new ObjectNotFoundException();
    }

    public async Task<TimeBlock> Update(TimeBlock timeBlock)
    {
        var existingTimeBlock = await GetById(timeBlock.Id);

        existingTimeBlock = timeBlock;

        await dbContext.SaveChangesAsync();

        return existingTimeBlock;
    }
}
