namespace Pronia.ViewModels
{
    public class BasketVM
    {
        public ICollection<PlantBasketItemVM> Plants { get; set; }
        public double TotalPrice { get; set; }
    }
}
