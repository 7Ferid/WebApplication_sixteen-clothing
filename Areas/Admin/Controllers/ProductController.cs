using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication_sixteen_clothing.Contexts;
using WebApplication_sixteen_clothing.Models;
using WebApplication_sixteen_clothing.ViewModels.ProductViewModels;

namespace WebApplication_sixteen_clothing.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController(AppDbContext _context,IWebHostEnvironment _environment) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Select(x=>new ProductGetVM
            {
               Id=x.Id,
               Name=x.Name,
               Description=x.Description,
               ImagePath=x.ImagePath,
               CategoryName=x.Category.Name,
               Price=x.Price,
               Rating=x.Rating
            }).ToListAsync();


            return View(products);
        }

        public async Task<IActionResult> Create()
        {
          await _SendCategoriesWithViewBag();
            return View();
        }

       

        [HttpPost]
        public async Task<IActionResult> CreateAsync(ProductCreateVM vm)
        {
            await _SendCategoriesWithViewBag();
            if (ModelState.IsValid)
                return View(vm);

            var isExistcategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);
            if (!isExistcategory)
            {
                ModelState.AddModelError("CategoryId", "This category is not found");
                return View(vm);
            }
            if (vm.Image.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Image", "Image's maximum size must be 2 mb");
                return View(vm);
            }
            if (!vm.Image.ContentType.Contains("image"))
            {
                ModelState.AddModelError("Image", "You can upload file in only image format ");
                return View(vm);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + vm.Image.FileName;
            string folderPath = Path.Combine(_environment.WebRootPath, "assets", "images");
            string path= Path.Combine(folderPath, uniqueFileName);

            using FileStream stream = new FileStream(path, FileMode.Create);
            await vm.Image.CopyToAsync(stream);

            Product product = new()
            {
                Name = vm.Name,
                Description = vm.Description,
                Rating = vm.Rating,
                Price = vm.Price,
                CategoryId = vm.CategoryId,
                ImagePath=uniqueFileName
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

            
        }
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product is null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }


  
        
        
        private async Task<IActionResult> _SendCategoriesWithViewBag()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;
            return View();
        }
    }
}
