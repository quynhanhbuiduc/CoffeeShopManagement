using CaféPourLaVie.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaféPourLaVie.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InventoryController : Controller
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // GET: Inventory
        public async Task<IActionResult> Index(string? searchString)
        {
            var inventory = await _inventoryService
                .SearchAsync(searchString);

            ViewBag.SearchString = searchString;

            return View(inventory);
        }

        // GET: Inventory/Transactions
        public async Task<IActionResult> Transactions()
        {
            var transactions = await _inventoryService.GetTransactionsAsync();

            return View(transactions);
        }
    }
}