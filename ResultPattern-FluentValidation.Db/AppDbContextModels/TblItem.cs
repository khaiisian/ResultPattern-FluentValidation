using System;
using System.Collections.Generic;

namespace ResultPattern_FluentValidation.Db.AppDbContextModels;

public partial class TblItem
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Qty { get; set; }

    public decimal Price { get; set; }
}
