using CaféPourLaVie.Data;
using CaféPourLaVie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaféPourLaVie.Controllers
{
    [Authorize(Roles = "Admin")]

    public class CategoryController : Controller
    {
        // Dependency injection of the ApplicationDbContext to access the database
        private readonly ApplicationDbContext _context;

        // Constructor that takes ApplicationDbContext as a parameter and assigns it to the private field
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();

            return View(categories);
        }

        //===== CREATE CATEGORY=====
        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            var inputName = category.CategoryName?.Trim().ToLower();

            if (await _context.Categories.AnyAsync(c => c.CategoryName.Trim().ToLower() == inputName))
            {
                ViewData["Error"] = "Không thể thêm danh mục. Tên danh mục này đã tồn tại.";
                return View(category);
            }

            if (ModelState.IsValid)
            {
                _context.Add(category); // INSERT INTO Category VALUES(...)
                await _context.SaveChangesAsync(); // Save changes to the database

                return RedirectToAction(nameof(Index));
            }

            // If the model state is not valid, return the view with the category object to display validation errors
            return View(category);
        }
    
        //===== EDIT CATEGORY=====
        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                _context.Update(category); // UPDATE Category SET ... WHERE CategoryId = id

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }


            return View(category);
        }

        //===== DELETE CATEGORY=====
        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category != null)
            {
                _context.Categories.Remove(category); // DELETE FROM Category WHERE CategoryId = id

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
