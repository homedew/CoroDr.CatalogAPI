using System;
using System.Collections.Generic;

namespace CoroDr.CatalogAPI.Models;

public partial class CatalogList
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal? Price { get; set; }

    public string? Description { get; set; }

    public string? ImageData { get; set; }

    public int? Promotion { get; set; }

    public int? Rating { get; set; }

    public int? CatalogBrandId { get; set; }

    public virtual CatalogBrand? CatalogBrand { get; set; }
}
