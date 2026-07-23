using GymSystem.BLL.Service.Interfaces;
using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.DAL.Data;
using GymSystem.DAL.Data.DbContexts;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Service.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Plan> _planRepository;


        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _planRepository = _unitOfWork.GetRepository<Plan>();
        }



        // Get All Plans For Index

        public async Task<IEnumerable<PlanViewModel>> GetAllAsync()
        {
            var plans = await _planRepository.GetAllAsync();

            return plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                DurationDays = p.DurationDays,
                Price = p.Price,
                Description = p.Description,
                IsActive = p.IsActive

            });
        }





        // Get Plan Details

        public async Task<PlanViewModel?> GetDetailsAsync(int id)
        {
            var plan = await _planRepository.GetByIdAsync(id);


            if (plan == null)
                return null;


            return new PlanViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                Description = plan.Description,
                IsActive = plan.IsActive
            };
        }






        // Get Plan For Edit

        public async Task<PlanToUpdateViewModel?> GetByIdAsync(int id)
        {
            var plan = await _planRepository.GetByIdAsync(id);


            if (plan == null)
                return null;


            return new PlanToUpdateViewModel
            {
                Name = plan.Name,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                Description = plan.Description
            };
        }






        // Create Plan

        public async Task CreateAsync(CreatePlanViewModel model)
        {
            var plan = new Plan
            {
                Name = model.Name,
                DurationDays = model.DurationDays,
                Price = model.Price,
                Description = model.Description,
                IsActive = true
            };


             _planRepository.AddAsync(plan);

            await _unitOfWork.SaveChangesAsync();
        }



        public async Task UpdateAsync(int id, PlanToUpdateViewModel model)
        {
            var plan = await _planRepository.GetByIdAsync(id);


            if (plan == null)
                throw new Exception("Plan not found");



            plan.Name = model.Name;
            plan.Price = model.Price;
            plan.DurationDays = model.DurationDays;
            plan.Description = model.Description;



            _planRepository.UpdateAsync(plan);


            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ToggleStatusAsync(int id)
        {
            var plan = await _planRepository.GetByIdAsync(id);


            if (plan == null)
                return;



            plan.IsActive = !plan.IsActive;



            _planRepository.UpdateAsync(plan);


            await _unitOfWork.SaveChangesAsync();
        }

    }
}
