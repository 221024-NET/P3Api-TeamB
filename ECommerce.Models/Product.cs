using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
    [Table ("Products", Schema = "ecd")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        public string ProductImage { get; set; }

        public Product() { }

        public Product(int id, string name, int quantity, decimal price, string description, string image)
        {
            ProductId = id;
            ProductName = name;
            ProductPrice =price;
            ProductDescription =description;
            ProductImage =image;    
        }
    }
}
