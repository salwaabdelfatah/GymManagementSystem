//using GymManagement.Data.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymSystem.DAL.Repositories.Interfaces;
using GymSystem.DAL.Repositories.Classes;
using GymSystem.DAL.Data.Models;
namespace GymManagement.Controllers
{
    public class PlansController : Controller
    {
        private readonly IGenericRepository<Plan> planRepository;

        //Index Action
        //GET BaseURL/Plans/Index -> Listing All Plans

        public PlansController(IGenericRepository<Plan> PlanRepository)
        {
            planRepository = PlanRepository;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await planRepository.GetAllAsync(ct: ct);
            return View(plans);
        }
        //Details Action
        //GET BaseURL/Plans/Details/1 (Id)
        public async Task<IActionResult> Details(int id,CancellationToken ct)
        {
            var plan = await planRepository.GetByIdAsync(id,ct);
            if (plan is null)
                return RedirectToAction(nameof(Index));
            else
                return View(plan);
        }
    }
}
