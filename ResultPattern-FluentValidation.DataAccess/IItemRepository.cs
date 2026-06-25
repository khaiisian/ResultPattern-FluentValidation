using ResultPattern_FluentValidation.Db.AppDbContextModels;

namespace ResultPattern_FluentValidation.DataAccess;

public interface IItemRepository
{
    Task<List<TblItem>> GetAll();
    Task<TblItem?> GetById(int id);
    Task<int> Add(TblItem item);
    Task<int> Update(TblItem item);
    Task<int> Delete(TblItem item);
}
