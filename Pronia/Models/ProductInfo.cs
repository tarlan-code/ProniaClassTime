namespace Pronia.Models
{
    public class ProductInfo:BaseEntity
    {
        public string Shipping { get; set; }
        public string AboutReturnRequest { get; set; }
        public string Garantee { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
