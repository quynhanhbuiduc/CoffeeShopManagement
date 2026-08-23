using CaféPourLaVie.Data;
using CaféPourLaVie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CaféPourLaVie.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Product
        public async Task<IActionResult> Index(int? categoryId,
                                       string? keyword,
                                       int page = 1)
        {
            // 1. Prepare data for ViewBag (Categories, SelectedCategory, Keyword)
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.SelectedCategory = categoryId;
            ViewBag.Keyword = keyword;

            // 2. Query products from the database
            var products = _context.Products
                                   .Include(p => p.Category)
                                   .AsQueryable();

            // 3. Filter products by CategoryId if provided
            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            // 4. Search products by keyword in ProductName or Description (case-insensitive)
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();

                // Add case-insensitive search using ToLower
                products = products.Where(p =>
                    p.ProductName.Contains(keyword) ||
                    (p.Description != null && p.Description.Contains(keyword)));
            }

            // 5. Implement pagination
            int pageSize = 10;

            // Count total products after filtering and searching
            int totalProducts = await products.CountAsync();

            // Calculate total pages and current page for ViewBag
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.CurrentPage = page;

            // Check if the requested page is valid
            var result = await products
                .OrderBy(p => p.ProductId) // Order by ProductId to ensure consistent ordering
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return View(result);
        }

        //===== CREATE PRODUCT=====
        // GET: Product/Create
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.CategoryId = new SelectList(_context.Categories,
                                                "CategoryId",
                                                "CategoryName");

            return View();
        }
        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product,IFormFile imageFile)
        {
            if (imageFile != null)
            {
                string fileName = Guid.NewGuid().ToString()
                                  + Path.GetExtension(imageFile.FileName);


                string path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/images/products",
                    fileName
                );


                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }


                product.Image = "/images/products/" + fileName;
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //===== EDIT PRODUCT=====
        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            Product? product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            ViewBag.CategoryId = new SelectList(
                _context.Categories,
                "CategoryId",
                "CategoryName",
                product.CategoryId);

            return View(product);
        }
        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile? imageFile)
        {
            if (id != product.ProductId)
                return NotFound();

            // Upload new image 
            if (imageFile != null && imageFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString()
                                + Path.GetExtension(imageFile.FileName);

                string path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/images/products",
                    fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                product.Image = "/images/products/" + fileName;
            }
            else
            {
                // Giữ lại ảnh cũ (Tối ưu hóa: Chỉ lấy đúng cột Image từ Database)
                var oldImage = await _context.Products
                    .AsNoTracking()
                    .Where(p => p.ProductId == id)
                    .Select(p => p.Image)
                    .FirstOrDefaultAsync();

                if (oldImage != null)
                {
                    product.Image = oldImage;
                }
            }

            _context.Update(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //===== DELETE PRODUCT=====
        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products
                                        .Include(p => p.Category)
                                        .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
                return NotFound();

            return View(product);
        }
        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
