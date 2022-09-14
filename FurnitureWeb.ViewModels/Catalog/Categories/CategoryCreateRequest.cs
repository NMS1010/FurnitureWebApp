using System.ComponentModel.DataAnnotations;

namespace FurnitureWeb.ViewModels.Catalog.Categories
{
    public class CategoryCreateRequest
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public int? ParentCategoryId { get; set; }

        [MaxLength(255)]
        public string Content { get; set; }
    }
}