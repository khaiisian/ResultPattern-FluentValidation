using Microsoft.EntityFrameworkCore;
using ResultPattern_FluentValidation.Db.AppDbContextModels;

namespace ResultPattern_FluentValidation.DataAccess;

public class ItemRepository : IItemRepository
{
    private readonly AppDbContext _appDbContext;

    public ItemRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<List<TblItem>> GetAll()
    {
        return await _appDbContext.TblItems.ToListAsync();
    }

    public async Task<TblItem?> GetById(int id)
    {
        return await _appDbContext.TblItems.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<int> Add(TblItem item)
    {
        await _appDbContext.TblItems.AddAsync(item);
        return await _appDbContext.SaveChangesAsync();
    }

    public async Task<int> Update(TblItem item)
    {
        _appDbContext.TblItems.Update(item);
        return await _appDbContext.SaveChangesAsync();
    }

    public async Task<int> Delete(TblItem item)
    {
        _appDbContext.TblItems.Remove(item);
        return await _appDbContext.SaveChangesAsync();
    }
}
