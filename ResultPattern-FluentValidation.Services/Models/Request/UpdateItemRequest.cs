namespace ResultPattern_FluentValidation.Services.Models.Request;

public class UpdateItemRequest
{
    public string? Name { get; set; }
    public int? Qty { get; set; }
    public decimal? Price { get; set; }
}
