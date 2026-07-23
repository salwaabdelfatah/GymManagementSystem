using GymSystem.BLL.Service.Interfaces;
using GymSystem.BLL.ViewModels.TrainerViewModel;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace GymManagement.PL.Controllers
{
    public class TrainerController : Controller
    {

        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        public async Task<IActionResult> Index()
        {
            var trainers = await _trainerService.GetAllAsync();
            return View(trainers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var trainer = await _trainerService.GetByIdAsync(id);

            if (trainer == null)
                return NotFound();

            return View(trainer);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTrainerViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            await _trainerService.CreateAsync(vm);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var trainer = await _trainerService.GetByIdAsync(id);

            if (trainer == null)
                return NotFound();

            
            var vm = new UpdateTrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                DateOfBirth = trainer.DateOfBirth,
                Gender = trainer.Gender,
                Speciality = trainer.Speciality,
                City = trainer.City,
                Street = trainer.Street,
                BuildingNumber = trainer.BuildingNumber
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateTrainerViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            await _trainerService.UpdateAsync(vm);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var trainer = await _trainerService.GetByIdAsync(id);

            if (trainer == null)
                return NotFound();

            return View(trainer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _trainerService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
