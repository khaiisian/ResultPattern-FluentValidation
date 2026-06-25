using ResultPattern_FluentValidation.Services.Models.Request;
using ResultPattern_FluentValidation.Services.Models.Response;
using ResultPattern_FluentValidation.Services.Shared;

namespace ResultPattern_FluentValidation.Services;

public interface IItemService
{
    Task<Result<List<ItemResponse>>> GetAll();
    Task<Result<ItemResponse>> GetById(int id);
    Task<Result<CreateItemResponse>> Create(CreateItemRequest request);
    Task<Result<UpdateItemResponse>> Update(int id, UpdateItemRequest request);
    Task<Result<bool>> Delete(int id);
}
