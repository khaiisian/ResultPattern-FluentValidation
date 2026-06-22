using ResultPattern_FluentValidation.Api.Models;
using ResultPattern_FluentValidation.Api.Shared;
using ResultPattern_FluentValidation.Db.AppDbContextModels;

namespace ResultPattern_FluentValidation.Api.Services
{
    public interface IItemService
    {
        Task<Result<List<TblItem>>> GetAll();
        Task<Result<TblItem>> GetById(int id);
        Task<Result<TblItem>> Create(CreateItemModel request);
        Task<Result<TblItem>> Update(int id, UpdateItemModel request);
        Task<Result<TblItem>> Delete(int id);
    }
}