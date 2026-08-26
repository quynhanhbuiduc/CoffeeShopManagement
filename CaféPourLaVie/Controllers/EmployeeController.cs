using CaféPourLaVie.Services.Interfaces;
using CaféPourLaVie.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaféPourLaVie.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetAllAsync();

            return View(employees);
        }



        // GET: Employee/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _employeeService.CreateAsync(model);

            if (!result.Success)
            {
                ViewData["Error"] = result.Message;
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _employeeService
                .GetEditViewModelByIdAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _employeeService.UpdateAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: Employee/ToggleStatus/5
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var result = await _employeeService.ToggleStatusAsync(id);

            TempData["Message"] = result.Message;

            return RedirectToAction(nameof(Index));
        }


        // GET: Employee/ResetPassword/5
        public async Task<IActionResult> ResetPassword(int id)
        {
            var result = await _employeeService.ResetPasswordAsync(id);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
            }
            else
            {
                TempData["Success"] = result.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
