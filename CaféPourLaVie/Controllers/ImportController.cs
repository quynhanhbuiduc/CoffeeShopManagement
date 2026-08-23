using CaféPourLaVie.Data;
using CaféPourLaVie.Models;
using CaféPourLaVie.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaféPourLaVie.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class ImportController : Controller
    {
        private readonly IImportService _importService;

        private readonly ApplicationDbContext _context;


        public ImportController(
            IImportService importService,
            ApplicationDbContext context)
        {
            _importService = importService;
            _context = context;
        }


        // =========================
        // INDEX
        // =========================
        public async Task<IActionResult> Index()
        {
            var imports =
                await _importService.GetAllAsync();

            return View(imports);
        }


        // =========================
        // CREATE GET
        // =========================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Products =
                await _context.Products
                    .Where(p => p.Status)
                    .OrderBy(p => p.ProductName)
                    .ToListAsync();

            return View();
        }


        // =========================
        // CREATE POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            List<ImportDetail> details)
        {
            try
            {
                if (details == null ||
                    details.Count == 0)
                {
                    TempData["Error"] = "Phiếu nhập phải có ít nhất một sản phẩm.";

                    return RedirectToAction(nameof(Create));
                }


                var accountIdClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId");


                if (accountIdClaim == null ||
                    !int.TryParse(
                        accountIdClaim.Value,
                        out int accountId))
                {
                    return Unauthorized();
                }


                int importReceiptId =
                    await _importService.CreateAsync(
                        accountId,
                        details);


                return RedirectToAction(
                    nameof(Details),
                    new
                    {
                        id = importReceiptId
                    });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction(nameof(Create));
            }
        }


        // =========================
        // DETAILS
        // =========================
        public async Task<IActionResult> Details(int id)
        {
            var import = await _importService.GetByIdAsync(id);


            if (import == null)
            {
                return NotFound();
            }


            return View(import);
        }


        // =========================
        // APPROVE
        // ADMIN ONLY
        // =========================
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                await _importService.ApproveAsync(id);

                TempData["Success"] = "Đã duyệt phiếu nhập và cập nhật tồn kho.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }


            return RedirectToAction(nameof(Details), new { id });
        }


        // =========================
        // REJECT
        // ADMIN ONLY
        // =========================
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            try
            {
                await _importService.RejectAsync(id);

                TempData["Success"] = "Đã từ chối phiếu nhập.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }


            return RedirectToAction(nameof(Details), new { id });
        }
    }
}