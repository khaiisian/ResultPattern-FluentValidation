namespace ResultPattern_FluentValidation.Services.Models.Request;

public class CreateItemRequest
{
    public string Name { get; set; } = string.Empty;
    public int Qty { get; set; }
    public decimal Price { get; set; }
}
