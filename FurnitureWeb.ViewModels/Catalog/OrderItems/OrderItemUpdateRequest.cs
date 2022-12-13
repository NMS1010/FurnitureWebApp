using System.ComponentModel.DataAnnotations;

namespace FurnitureWeb.ViewModels.Catalog.OrderItems
{
    public class OrderItemUpdateRequest
    {
        [Required]
        public int OrderItemId { get; set; }
    }
}