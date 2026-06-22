using Microsoft.EntityFrameworkCore;
using ResultPattern_FluentValidation.Api.Models;
using ResultPattern_FluentValidation.Api.Shared;
using ResultPattern_FluentValidation.Db.AppDbContextModels;

namespace ResultPattern_FluentValidation.Api.Services;

public class ItemService : IItemService
{
    private readonly AppDbContext _appDbContext;

    public ItemService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Result<List<TblItem>>> GetAll()
    {
        var items = await _appDbContext.TblItems.ToListAsync();
        return Result<List<TblItem>>.Success(items, "Items are retrieved.");
    }

    public async Task<Result<TblItem>> GetById(int id)
    {
        var item = await FindItem(id);
        if (item is null) return Result<TblItem>.NotFound();

        return Result<TblItem>.Success(item, "Item is retrieved.");
    }

    public async Task<Result<TblItem>> Create(CreateItemModel request)
    {
        var item = new TblItem
        {
            Name = request.Name,
            Qty = request.Qty,
            Price = request.Price,
        };

        await _appDbContext.TblItems.AddAsync(item);
        int result = await _appDbContext.SaveChangesAsync();

        return result > 0
            ? Result<TblItem>.Success(item, "Save successful.")
            : Result<TblItem>.Failure("Save failed.");
    }

    public async Task<Result<TblItem>> Update(int id, UpdateItemModel request)
    {
        var item = await FindItem(id);
        if (item is null) return Result<TblItem>.NotFound();

        if (!string.IsNullOrEmpty(request.Name)) item.Name = request.Name;

        if (request.Qty.HasValue) item.Qty = request.Qty.Value;

        if (request.Price.HasValue) item.Price = request.Price.Value;

        int result = await _appDbContext.SaveChangesAsync();

        return result > 0 ?
            Result<TblItem>.Success(item, "Update successful.")
            : Result<TblItem>.Failure("Update Failed.");
    }

    public async Task<Result<TblItem>> Delete(int id)
    {
        var item = await FindItem(id);
        if (item is null) return Result<TblItem>.NotFound();

        _appDbContext.TblItems.Remove(item);

        int result = await _appDbContext.SaveChangesAsync();

        return result > 0 ?
            Result<TblItem>.Success(item, "Delete successful.")
            : Result<TblItem>.Failure("Delete Failed.");
    }

    public async Task<TblItem?> FindItem(int id)
    {
        return await _appDbContext.TblItems.FirstOrDefaultAsync(x => x.Id == id);
    }
}
