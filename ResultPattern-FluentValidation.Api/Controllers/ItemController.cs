using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResultPattern_FluentValidation.Api.Models;
using ResultPattern_FluentValidation.Db.AppDbContextModels;

namespace ResultPattern_FluentValidation.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public ItemController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllItems()
    {
        var lst = await _appDbContext.TblItems.ToListAsync();
        return Ok(lst);
    }

    [HttpPost]
    public async Task<IActionResult> CreateItem(CreateItemModel reqeust)
    {
        var item = new TblItem
        {
            Name = reqeust.Name,
            Qty = reqeust.Qty,
            Price = reqeust.Price,
        };

        await _appDbContext.TblItems.AddAsync(item);
        int result = await _appDbContext.SaveChangesAsync();

        var msg = result > 0 ? "Save successful." : "Save failed.";
        return Ok(msg);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateItem(int id, UpdateItemModel request)
    {
        var item = await _appDbContext.TblItems.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null) return NotFound("No data found.");

        if(!string.IsNullOrEmpty(request.Name)) item.Name = request.Name;

        if(request.Qty.HasValue) item.Qty = request.Qty.Value;

        if(request.Price.HasValue) item.Price = request.Price.Value;

        int result = await _appDbContext.SaveChangesAsync();

        var msg = result > 0 ? "Update successful." : "Update Failed.";

        return Ok(msg);
    }
}
