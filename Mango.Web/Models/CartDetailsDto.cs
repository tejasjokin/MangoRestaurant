using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Web.Models
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        [ForeignKey("CartHeaderId")]
        public virtual CartHeaderDto CartHeader { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("Product")]
        public virtual ProductDto Product { get; set; }
        public int Count { get; set; }
    }
}
