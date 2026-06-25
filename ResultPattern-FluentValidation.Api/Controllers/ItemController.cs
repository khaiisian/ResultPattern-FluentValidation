using Microsoft.AspNetCore.Mvc;
using ResultPattern_FluentValidation.Services;
using ResultPattern_FluentValidation.Services.Models.Request;
using ResultPattern_FluentValidation.Services.Shared;

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
        var res = await _itemService.GetAll();
        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var res = await _itemService.GetById(id);
        return res.Type switch
        {
            ResultType.Success => Ok(res),
            ResultType.NotFound => NotFound(res),
            _ => StatusCode(500, res)
        };
    }

    [HttpPost]
    public async Task<IActionResult> CreateItem(CreateItemRequest request)
    {
        var res = await _itemService.Create(request);
        return res.Type switch
        {
            ResultType.Success => Ok(res),
            _ => StatusCode(500, res)
        };
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateItem(int id, UpdateItemRequest request)
    {
        var res = await _itemService.Update(id, request);
        return res.Type switch
        {
            ResultType.Success => Ok(res),
            ResultType.NotFound => NotFound(res),
            _ => StatusCode(500, res)
        };
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var res = await _itemService.Delete(id);
        return res.Type switch
        {
            ResultType.Success => Ok(res),
            ResultType.NotFound => NotFound(res),
            _ => StatusCode(500, res)
        };
    }
}
