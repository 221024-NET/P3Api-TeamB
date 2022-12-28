namespace ECommerce.Models
{
    public class ProductDTO
    {
        public int id { get; set; }
        public int quantity { get; set; }

        public ProductDTO() { }

        public ProductDTO(int id, int quantity)
        {
            this.id = id;
            this.quantity = quantity;
        }
    }
}
