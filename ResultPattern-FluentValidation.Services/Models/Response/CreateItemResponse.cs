namespace ResultPattern_FluentValidation.Services.Models.Response;

public class CreateItemResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Qty { get; set; }
    public decimal Price { get; set; }
}
