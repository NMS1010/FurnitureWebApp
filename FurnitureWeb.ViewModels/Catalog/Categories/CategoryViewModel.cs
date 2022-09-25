namespace FurnitureWeb.ViewModels.Catalog.Categories
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }

        public string ImagePath { get; set; }
    }
}