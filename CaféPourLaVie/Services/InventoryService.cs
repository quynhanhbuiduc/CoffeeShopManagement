using CaféPourLaVie.Data;
using CaféPourLaVie.Services.Interfaces;
using CaféPourLaVie.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CaféPourLaVie.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext _context;

        public InventoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<InventoryViewModel>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Select(p => new InventoryViewModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    CategoryName = p.Category.CategoryName,
                    Quantity = p.Quantity,
                    Price = p.Price
                })
                .OrderBy(p => p.Quantity)
                .ToListAsync();
        }

        public async Task<List<InventoryViewModel>> SearchAsync(
            string? searchString)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(p =>
                    p.ProductName.Contains(searchString));
            }

            return await query
                .Select(p => new InventoryViewModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    CategoryName = p.Category.CategoryName,
                    Quantity = p.Quantity,
                    Price = p.Price
                })
                .OrderBy(p => p.Quantity)  // Order by Quantity in ascending order
                .ToListAsync();
        }

        public async Task<List<InventoryTransactionViewModel>> GetTransactionsAsync()
        {
            return await _context.InventoryTransactions
                .Include(t => t.Product)
                .OrderByDescending(t => t.TransactionDate)
                .Select(t => new InventoryTransactionViewModel
                {
                    TransactionDate = t.TransactionDate,

                    ProductName = t.Product.ProductName,

                    Type = t.Type.ToString(),

                    Quantity = t.Quantity,

                    Note = t.Note
                })
                .ToListAsync();
        }
    }
}