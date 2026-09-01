using System.ComponentModel.DataAnnotations;

namespace FirstMvcWebapp.Dto
{
    public class ProductDto
    {
      
        public int Id { get; set; }
        public string ProductName { get; set; } = null!;
        public string Description { get; set; } = null!;

        public decimal Price { get; set; } = 0.00m;
        public string color { get; set; } = null!;
    }
}
