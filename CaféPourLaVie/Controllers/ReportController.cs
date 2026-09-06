using CaféPourLaVie.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaféPourLaVie.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // GET: Report
        public async Task<IActionResult> Index(
            DateTime? fromDate,
            DateTime? toDate)
        {
            // If fromDate is null, set it to the first day of the current month. If toDate is null, set it to today.
            var from = fromDate ?? new DateTime(DateTime.Today.Year,
                                                DateTime.Today.Month,
                                                1);

            var to = toDate ?? DateTime.Today;

            var model = await _reportService.GetReportAsync(from, to);

            return View(model);
        }
    }
}