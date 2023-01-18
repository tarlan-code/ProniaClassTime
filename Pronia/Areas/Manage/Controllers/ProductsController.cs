using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using Pronia.DAL;
using Pronia.Models;
using Pronia.Utilies.Enums;
using Pronia.Utilies.Extensions;
using Pronia.ViewModels;

namespace Pronia.Areas.Manage.Controllers
{
    [Area(nameof(Manage))]
    [Authorize(Roles ="Admin,Superadmin")]
    public class ProductsController : Controller
    {
        readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;
        public ProductsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int page=1)
        {
           PaginationVM<Product> paginet = new PaginationVM<Product>();
            paginet.MaxPageCount = (int)Math.Ceiling((decimal)_context.Products.Count() / 5);
            paginet.CurrentPage = page;
            if (page > paginet.MaxPageCount || page < 1) return BadRequest();
            paginet.Items = _context.Products.Skip((page - 1) * 5).Take(5).Include(p => p.ProductColors).ThenInclude(pc => pc.Color).Include(p => p.ProductSizes).ThenInclude(ps => ps.Size).Include(p => p.ProductCategories).ThenInclude(pc => pc.Category).Include(p => p.ProductImages);
            return View(paginet);
        } 

        
        public IActionResult Delete(int? id)
        {
            if (id is null || id <= 0) return BadRequest();
            Product product = _context.Products.Find(id);
            if (product == null) return NotFound();
            product.IsDeleted = true;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        
        public IActionResult Create()
        {
            ViewBag.Colors = new SelectList(_context.Colors,nameof(Color.Id),nameof(Color.Name));
            ViewBag.Sizes = new SelectList(_context.Sizes,nameof(Size.Id),nameof(Size.Name));
            ViewBag.Categories = new SelectList(_context.Categories,nameof(Category.Id),nameof(Category.Name));
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateProductVM cp)
        {
            var coverImg = cp?.CoverImage;
            var hoverImg = cp?.HoverImage;
            var otherImgs = cp?.OtherImages;


            string result = coverImg?.CheckValidate("image/", 300) ?? "";
            if (result.Length>0) ModelState.AddModelError("CoverImage", result);
            result = hoverImg?.CheckValidate("image/", 300) ?? "";
            if (result.Length > 0) ModelState.AddModelError("HoverImage", result);


            if (otherImgs != null)
            {
                foreach (IFormFile item in otherImgs)
                {
                    result = item?.CheckValidate("image/", 300) ?? "";
                    if (result.Length > 0) ModelState.AddModelError("OtherImages", result);
                }
            }

            if (cp.ColorIds is not null)
            {
                foreach (int ColorId in cp.ColorIds)
                {
                    if (!_context.Colors.Any(c => c.Id == ColorId))
                    {
                        ModelState.AddModelError("ColorIds", "Colors are not entered correctly");
                        break;
                    }
                }
            }

            if (cp.SizeIds is not null)
            {
                foreach (int SizeId in cp.SizeIds)
                {
                    if (!_context.Sizes.Any(s => s.Id == SizeId))
                    {
                        ModelState.AddModelError("SizeIds", "Categories are not entered correctly");
                        break;
                    }
                }
            }

            if (cp.CategoryIds is not null)
            {
                foreach (int CatId in cp.CategoryIds)
                {
                    if (!_context.Categories.Any(c => c.Id == CatId))
                    {
                        ModelState.AddModelError("CategryIds", "Categories are not entered correctly");
                        break;
                    }
                }
            }





            if (!ModelState.IsValid)
            {
                ViewBag.Colors = new SelectList(_context.Colors, nameof(Color.Id), nameof(Color.Name));
                ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
                ViewBag.Categories = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
                return View();
            }

            var sizes = _context.Sizes.Where(s => cp.SizeIds.Contains(s.Id));
            var colors = _context.Colors.Where(col => cp.ColorIds.Contains(col.Id));
            var categories = _context.Categories.Where(c => cp.CategoryIds.Contains(c.Id));
            Product newProduct = new Product
            {
                Name = cp.Name,
                Desc = cp.Desc,
                SKU = Guid.NewGuid().ToString(),
                CostPrice = cp.CostPrice,
                SellPrice = cp.SellPrice,
                Discount = cp.Discount,
                IsDeleted = false,
                Date = DateTime.Now,

            };
            

            List<ProductImage> images = new List<ProductImage>();

            images.Add(new ProductImage { Image = coverImg.SaveFile(Path.Combine(_env.WebRootPath, "assets", "images", "product")), IsCover = true, Product = newProduct });

            if(hoverImg is not null) images.Add(new ProductImage { Image = hoverImg.SaveFile(Path.Combine(_env.WebRootPath, "assets", "images", "product")), IsCover = false, Product = newProduct });


            if (otherImgs is not null)
            {
                foreach (IFormFile item in otherImgs)
                {
                    images.Add(new ProductImage { Image = item.SaveFile(Path.Combine(_env.WebRootPath, "assets", "images", "product")), IsCover = null, Product = newProduct });
                }
            }



            newProduct.ProductImages = images;


            _context.Products.Add(newProduct);  


            foreach (var item in sizes)
            {
                _context.ProductSizes.Add(new ProductSize {
                    Product = newProduct,
                    SizeId = item.Id
                });
            }
            foreach (var item in colors)
            {
                _context.ProductColors.Add(new ProductColor {
                    Product = newProduct,
                    ColorId = item.Id
                });
            }
            foreach (var item in categories)
            {
                _context.ProductCategories.Add(new ProductCategory {
                    Product = newProduct,
                    CategoryId = item.Id
                });
            }


            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }



        public IActionResult Update(int? Id)
        {
            Product product = _context.Products.Include(p => p.ProductColors).Include(p => p.ProductCategories).Include(p => p.ProductSizes).FirstOrDefault(p=> p.Id == Id);
            if (product == null) return NotFound();

            UpdateProductVM up = new UpdateProductVM()
            {
                 Id = product.Id,   
                 Name = product.Name,
                 Desc = product.Desc,
                 CostPrice = product.CostPrice,
                 SellPrice = product.SellPrice,
                 Discount = product.Discount,
                 ColorIds = new List<int>(),
                 SizeIds = new List<int>(),
                 CategoryIds = new List<int>()
            };

            foreach (var color in product.ProductColors)
            {
                up.ColorIds.Add(color.ColorId);
            }
            foreach (var size in product.ProductSizes)
            {
                up.SizeIds.Add(size.SizeId);
            }
            foreach (var cat in product.ProductCategories)
            {
                up.CategoryIds.Add(cat.CategoryId);
            }

            ViewBag.Colors = new SelectList(_context.Colors, nameof(Color.Id), nameof(Color.Name));
            ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
            ViewBag.Categories = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
            return View(up);
        }

        [HttpPost]
        public IActionResult Update(int? Id,UpdateProductVM up)
        {
            if (Id is null || Id <= 0) return BadRequest();
            

            if (up.ColorIds is not null)
            {
                foreach (int ColorId in up.ColorIds)
                {
                    if (!_context.Colors.Any(c => c.Id == ColorId))
                    {
                        ModelState.AddModelError("ColorIds", "Colors are not entered correctly");
                        break;
                    }
                }
            }

            if (up.SizeIds is not null)
            {
                foreach (int SizeId in up.SizeIds)
                {
                    if (!_context.Sizes.Any(s => s.Id == SizeId))
                    {
                        ModelState.AddModelError("SizeIds", "Categories are not entered correctly");
                        break;
                    }
                }
            }

            if (up.CategoryIds is not null)
            {
                foreach (int CatId in up.CategoryIds)
                {
                    if (!_context.Categories.Any(c => c.Id == CatId))
                    {
                        ModelState.AddModelError("CategryIds", "Categories are not entered correctly");
                        break;
                    }
                }
            }



            if (!ModelState.IsValid)
            {
                ViewBag.Colors = new SelectList(_context.Colors, nameof(Color.Id), nameof(Color.Name));
                ViewBag.Sizes = new SelectList(_context.Sizes, nameof(Size.Id), nameof(Size.Name));
                ViewBag.Categories = new SelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name));
                return View();
            }

            Product OldProduct = _context.Products.Include(p => p.ProductColors).Include(p => p.ProductSizes).Include(p => p.ProductCategories).FirstOrDefault(p => p.Id == Id);

            if (OldProduct is null) return NotFound();



            foreach (var color in OldProduct.ProductColors)
            {
                if (up.ColorIds.Contains(color.ColorId)) up.ColorIds.Remove(color.ColorId);
                else _context.ProductColors.Remove(color);
            }

            foreach (var colorId in up.ColorIds) _context.ProductColors.Add(new ProductColor { Product = OldProduct, ColorId = colorId });
            

            foreach (var size in OldProduct.ProductSizes)
            {
                if (up.SizeIds.Contains(size.SizeId)) up.SizeIds.Remove(size.SizeId);
                else _context.ProductSizes.Remove(size);
            }

            foreach (var sizeId in up.SizeIds) _context.ProductSizes.Add(new ProductSize { Product = OldProduct, SizeId = sizeId });
            

            foreach (var cat in OldProduct.ProductCategories)
            {
                if (up.CategoryIds.Contains(cat.CategoryId)) up.CategoryIds.Remove(cat.CategoryId);
                else _context.ProductCategories.Remove(cat);
            }

            foreach (var catId in up.CategoryIds) _context.ProductCategories.Add(new ProductCategory { Product = OldProduct, CategoryId = catId });



            OldProduct.Name = up.Name;
            OldProduct.Discount = up.Discount;
            OldProduct.CostPrice = up.CostPrice;
            OldProduct.SellPrice = up.SellPrice;
            OldProduct.Desc = up.Desc;
            OldProduct.IsDeleted = false;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult UpdateImage(int? Id)
        {
            if (Id is null || Id <= 0) return BadRequest();
            var prod = _context.Products.Include(p => p.ProductImages).FirstOrDefault(p=>p.Id==Id);
            if(prod is null) return NotFound();

            UpdateProductImgVM upi = new UpdateProductImgVM
            {
                ProductImages = prod.ProductImages
            };


            return View(upi);
        }

        [HttpPost]

        public IActionResult UpdateImage(int? Id,UpdateProductImgVM upi)
        {
            if(Id is null || Id <= 0) return BadRequest();
            Product prod = _context.Products.Include(p=>p.ProductImages).FirstOrDefault(p=> p.Id == Id);
            if(prod is null) return NotFound();

            IFormFile? coverImg = upi.CoverImage;
            IFormFile? hoverImg = upi.HoverImage;
            IEnumerable<IFormFile>? otherImgs = upi.OtherImages;
            string result = "";
            if (coverImg is not null)
            {
                 result= coverImg?.CheckValidate("image/", 300) ?? "";
                if (result.Length > 0) ModelState.AddModelError("CoverImage", result);
            }

            if(hoverImg is not null)
            {
                result = hoverImg?.CheckValidate("image/", 300) ?? "";
                if (result.Length > 0) ModelState.AddModelError("HoverImage", result);
            }

            if (otherImgs is not null)
            {
                foreach (var image in otherImgs)
                {
                    result = image?.CheckValidate("image/", 300) ?? "";
                    if (result.Length > 0)
                    {
                        ModelState.AddModelError("OtherImages", result);
                        break;
                    }

                }
            }

            if (!ModelState.IsValid) return View();

            List<ProductImage> images = new List<ProductImage>();
            var oldImgs = _context.ProductImages.Where(pi => pi.ProductId == Id);

            if(coverImg is not null)
            {
                coverImg.SaveWithName(Path.Combine(_env.WebRootPath, "assets", "images", "product"),oldImgs.FirstOrDefault(o=>o.IsCover == true).Image);
            }

            if(hoverImg is not null)
            {
                if (oldImgs.FirstOrDefault(o => o.IsCover == false) is not null)
                {
                    hoverImg.SaveWithName(Path.Combine(_env.WebRootPath, "assets", "images", "product"), oldImgs.FirstOrDefault(o => o.IsCover == false).Image);
                }

                else
                {
                    images.Add(new ProductImage { Image = hoverImg.SaveFile(Path.Combine(_env.WebRootPath, "assets", "images", "product")), IsCover = false, Product = prod });
                }
            }


            if(otherImgs is not null)
            {
                foreach (var image in otherImgs)
                {
                    images.Add(new ProductImage { Image = image.SaveFile(Path.Combine(_env.WebRootPath, "assets", "images", "product")), IsCover = null, Product = prod });
                }
            }

            prod.ProductImages.AddRange(images);

            _context.SaveChanges();
            return RedirectToAction(nameof(Update), new { Id = Id });
        }

        public IActionResult DeleteProductImage(int? Id)
        {

            if (Id is null || Id < 0) return BadRequest();
            var pi = _context.ProductImages.Find(Id);
            if(pi is null) return NotFound();
            _context.ProductImages.Remove(pi);
            pi.Image.DeleteFile(_env.WebRootPath, Path.Combine("assets", "images", "product"));
            _context.SaveChanges();
            return Ok();
        }
    }
}
