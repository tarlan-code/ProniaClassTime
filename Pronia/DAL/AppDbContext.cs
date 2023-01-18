using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pronia.Models;

namespace Pronia.DAL
{
    public class AppDbContext:IdentityDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<MainSlider> MainSliders { get; set; }
        public DbSet<ShippingArea> ShippingAreas { get; set; }
        public DbSet<TestimonialArea> TestimonialAreas { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Color> Colors { get; set; }        
        public DbSet<Size> Sizes { get; set; }        
        public DbSet<Product> Products { get; set; }     
        public DbSet<ProductCategory> ProductCategories { get; set; }      
        public DbSet<ProductColor> ProductColors { get; set; }       
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductInfo> ProductInfos { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }


    }
}
