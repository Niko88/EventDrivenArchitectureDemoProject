using System.ComponentModel.DataAnnotations;

namespace ExampleStore.Models
{
    public class HomeViewModel
    {
        public StockDetails? ItemDetails { get; set; }

        [Required]
        public string CustomerCode { get; set; }
    }
}
