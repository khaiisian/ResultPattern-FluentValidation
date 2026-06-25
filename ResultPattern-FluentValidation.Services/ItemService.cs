using ResultPattern_FluentValidation.DataAccess;
using ResultPattern_FluentValidation.Db.AppDbContextModels;
using ResultPattern_FluentValidation.Services.Models.Request;
using ResultPattern_FluentValidation.Services.Models.Response;
using ResultPattern_FluentValidation.Services.Shared;

namespace ResultPattern_FluentValidation.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;

    public ItemService(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<Result<List<ItemResponse>>> GetAll()
    {
        var items = await _itemRepository.GetAll();
        var data = items.Select(MapToItemResponse).ToList();

        return Result<List<ItemResponse>>.Success(data, "Items are retrieved.");
    }

    public async Task<Result<ItemResponse>> GetById(int id)
    {
        var item = await _itemRepository.GetById(id);
        if (item is null) return Result<ItemResponse>.NotFound();

        return Result<ItemResponse>.Success(MapToItemResponse(item), "Item is retrieved.");
    }

    public async Task<Result<CreateItemResponse>> Create(CreateItemRequest request)
    {
        var item = new TblItem
        {
            Name = request.Name,
            Qty = request.Qty,
            Price = request.Price,
        };

        int rows = await _itemRepository.Add(item);
        if (rows == 0) return Result<CreateItemResponse>.Failure("Save failed.");

        var data = new CreateItemResponse
        {
            Id = item.Id,
            Name = item.Name,
            Qty = item.Qty,
            Price = item.Price,
        };

        return Result<CreateItemResponse>.Success(data, "Save successful.");
    }

    public async Task<Result<UpdateItemResponse>> Update(int id, UpdateItemRequest request)
    {
        var item = await _itemRepository.GetById(id);
        if (item is null) return Result<UpdateItemResponse>.NotFound();

        if (!string.IsNullOrEmpty(request.Name)) item.Name = request.Name;
        if (request.Qty.HasValue) item.Qty = request.Qty.Value;
        if (request.Price.HasValue) item.Price = request.Price.Value;

        await _itemRepository.Update(item);

        var data = new UpdateItemResponse
        {
            Id = item.Id,
            Name = item.Name,
            Qty = item.Qty,
            Price = item.Price,
        };

        return Result<UpdateItemResponse>.Success(data, "Update successful.");
    }

    public async Task<Result<bool>> Delete(int id)
    {
        var item = await _itemRepository.GetById(id);
        if (item is null) return Result<bool>.NotFound();

        int rows = await _itemRepository.Delete(item);
        if (rows == 0) return Result<bool>.Failure("Delete failed.");

        return Result<bool>.Success(true, "Delete successful.");
    }

    private static ItemResponse MapToItemResponse(TblItem item) => new()
    {
        Id = item.Id,
        Name = item.Name,
        Qty = item.Qty,
        Price = item.Price,
    };
}
