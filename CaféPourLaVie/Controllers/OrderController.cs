using CaféPourLaVie.Data;
using CaféPourLaVie.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaféPourLaVie.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IOrderService _orderService;


        public OrderController(ApplicationDbContext context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }


        // GET: Order
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.Account)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders

                .Include(o => o.Account)

                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Product)

                .FirstOrDefaultAsync(o => o.OrderId == id);


            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }


        // POST: Order/Cancel/5
        [HttpPost] // This action is only accessible via POST requests
        [ValidateAntiForgeryToken] // This attribute helps prevent CSRF attacks
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                await _orderService.CancelOrderAsync(id);
                TempData["Success"] = "Đã hủy đơn hàng và hoàn lại số lượng kho thành công!";
            }
            catch (Exception ex)
            {
                // Capture the exception and display an error message to the user
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}