//using GymManagement.Data.DbContexts;
using GymSystem.BLL.Service.Classes;
using GymSystem.BLL.Service.Interfaces;
using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repositories.Classes;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace GymManagement.Controllers
{
    public class PlansController : Controller
    {
        private readonly IPlanService _planService;

        public PlansController(IPlanService planService)
        {
            _planService = planService;
        }

        
        public async Task<IActionResult> Index()
        {
            var plans = await _planService.GetAllAsync();
            return View(plans);
        }

        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePlanViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _planService.CreateAsync(model);

            TempData["SuccessMessage"] = "Plan created successfully";

            return RedirectToAction(nameof(Index));
        }

        // 🟢 Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var plan = await _planService.GetByIdAsync(id);

            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan not found";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PlanToUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _planService.UpdateAsync(id, model);

            TempData["SuccessMessage"] = "Plan updated successfully";

            return RedirectToAction(nameof(Index));
        }

        // 🟢 Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            TempData["SuccessMessage"] = "Plan deleted successfully";

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            await _planService.ToggleStatusAsync(id);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int id)
        {
            var plan = await _planService.GetDetailsAsync(id);

            if (plan == null)
                return NotFound();

            return View(plan);
        }
    }
}
