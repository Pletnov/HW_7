using System;
using System.Collections.Generic;
using System.Text;

namespace HW_7.Result;

public class DummyProductResponse
{
    public List<DummyProduct> Products { get; set; } = new();
    public int Total { get; set; }
    public int Skip { get; set; }
    public int Limit { get; set; }
}

public class DummyProduct
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal Rating { get; set; }
    public int Stock { get; set; }
    public List<string> Tags { get; set; } = new();
    public string Brand { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal Weight { get; set; }
    public Dimensions Dimensions { get; set; } = new();
    public string WarrantyInformation { get; set; } = string.Empty;
    public string ShippingInformation { get; set; } = string.Empty;
    public string AvailabilityStatus { get; set; } = string.Empty;
    public List<ProductReview> Reviews { get; set; } = new();
    public string ReturnPolicy { get; set; } = string.Empty;
    public int MinimumOrderQuantity { get; set; }
    public ProductMeta Meta { get; set; } = new();
    public List<string> Images { get; set; } = new();
    public string Thumbnail { get; set; } = string.Empty;
}

public class Dimensions
{
    public decimal Width { get; set; }
    public decimal Height { get; set; }
    public decimal Depth { get; set; }
}

public class ProductReview
{
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string ReviewerName { get; set; } = string.Empty;
    public string ReviewerEmail { get; set; } = string.Empty;
}

public class ProductMeta
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Barcode { get; set; } = string.Empty;
    public string QrCode { get; set; } = string.Empty;
}
