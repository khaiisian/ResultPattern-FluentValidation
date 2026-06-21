namespace ResultPattern_FluentValidation.Api.Models;

public class CreateItemModel
{
    public string Name { get; set; } = string.Empty;
    public int Qty { get; set; }
    public decimal Price { get; set; }
}
