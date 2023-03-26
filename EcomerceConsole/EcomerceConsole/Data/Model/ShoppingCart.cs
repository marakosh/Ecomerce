using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomerceConsole.Data.Model
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastModifiedDate { get; set; }

        public decimal TotalPrice => Items.Sum(ci => ci.Product.Price * ci.Quantity);
    }
}
