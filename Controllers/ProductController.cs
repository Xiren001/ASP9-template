using Microsoft.AspNetCore.Mvc;
using TransparencyCo.Models;
using UserRoles.Data;

namespace TransparencyCo.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Admin()
        {
            return View(_context.Products.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return RedirectToAction("Admin", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile image, IFormFile certificateImage)
        {
            if (image != null)
            {
                var imagePath = Path.Combine("uploads", Guid.NewGuid().ToString() + Path.GetExtension(image.FileName));
                using var stream = new FileStream(Path.Combine(_env.WebRootPath, imagePath), FileMode.Create);
                await image.CopyToAsync(stream);
                product.ImagePath = "/" + imagePath;
            }

            if (certificateImage != null)
            {
                var certPath = Path.Combine("uploads", Guid.NewGuid().ToString() + Path.GetExtension(certificateImage.FileName));
                using var stream = new FileStream(Path.Combine(_env.WebRootPath, certPath), FileMode.Create);
                await certificateImage.CopyToAsync(stream);
                product.CertificateImagePath = "/" + certPath;
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Admin", "Home");

        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            var existing = await _context.Products.FindAsync(product.Id);
            if (existing == null) return NotFound();

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.CertificateName = product.CertificateName;

            await _context.SaveChangesAsync();
            return RedirectToAction("Admin", "Home");

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Admin", "Home");

        }
    }

}
