using Pronia.Models;

namespace Pronia.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<MainSlider> MainSliders { get; set; }
        public List<ShippingArea> ShippingAreas { get; set; }
        public TestimonialArea TestimonialArea { get; set; }
        public IEnumerable<Testimonial> Testimonials { get; set; }
        public IEnumerable<Banner> Banners { get; set; }
        public IEnumerable<Brand> Brands { get; set; }

        public IEnumerable<Product> FeaturedProducts { get; set; }
        public IEnumerable<Product> LatestProducts { get; set; }

    }
}
