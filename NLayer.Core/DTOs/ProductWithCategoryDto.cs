namespace NLayer.Core.DTOs
{
    public class ProductWithCategoryDto : ProductDto  // GetProductWithCategory için olusturduk
    {
        public CategoryDto Category { get; set; }
    }
}
