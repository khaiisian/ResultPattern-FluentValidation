using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResultPattern_FluentValidation.Api.Models;
using ResultPattern_FluentValidation.Api.Services;
using ResultPattern_FluentValidation.Db.AppDbContextModels;

namespace ResultPattern_FluentValidation.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly IItemService _itemService;

    public ItemController(IItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllItems()
    {
        var lst = await _itemService.GetAll();
        return Ok(lst);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var res = await _itemService.GetById(id);
        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> CreateItem(CreateItemModel request)
    {
        var res = await _itemService.Create(request);

        return Ok(res);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateItem(int id, UpdateItemModel request)
    {
        var res = await _itemService.Update(id, request);

        return Ok(res);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var res = await _itemService.Delete(id);
        return Ok(res);
    }
}
