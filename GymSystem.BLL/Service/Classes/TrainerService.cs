using GymSystem.BLL.Service.Interfaces;
using GymSystem.BLL.ViewModels.TrainerViewModel;
using GymSystem.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymSystem.DAL.Repositories.Interfaces;
using GymSystem.BLL.ViewModels.TrainerViewModel;

namespace GymSystem.BLL.Service.Classes
{
    public class TrainerService :ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllAsync()
        {
            var trainers = await _unitOfWork
                .GetRepository<Trainer>()
                .GetAllAsync();

            return trainers.Select(t => new TrainerViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone
            });
        }

        public async Task<TrainerDetailsViewModel?> GetByIdAsync(int id)
        {
            var t = await _unitOfWork
                .GetRepository<Trainer>()
                .GetByIdAsync(id);

            if (t == null) return null;

            return new TrainerDetailsViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                DateOfBirth = t.DateOfBirth,
                Gender = t.Gender,
                Speciality = t.speciality,
                City = t.address.City,
                Street = t.address.Street,
                BuildingNumber = t.address.BuildingNumber
            };
        }

        public async Task CreateAsync(CreateTrainerViewModel vm)
        {
            var trainer = new Trainer
            {
                Name = vm.Name,
                Email = vm.Email,
                Phone = vm.Phone,
                DateOfBirth = vm.DateOfBirth,
                Gender = vm.Gender,
                speciality = vm.Speciality,
                address = new Address
                {
                    City = vm.City,
                    Street = vm.Street,
                    BuildingNumber = vm.BuildingNumber
                }
            };

            _unitOfWork.GetRepository<Trainer>().AddAsync(trainer);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateTrainerViewModel vm)
        {
            var trainer = new Trainer
            {
                Id = vm.Id,
                Name = vm.Name,
                Email = vm.Email,
                Phone = vm.Phone,
                DateOfBirth = vm.DateOfBirth,
                Gender = vm.Gender,
                speciality = vm.Speciality,
                address = new Address
                {
                    City = vm.City,
                    Street = vm.Street,
                    BuildingNumber = vm.BuildingNumber
                }
            };

            _unitOfWork.GetRepository<Trainer>().UpdateAsync(trainer);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Trainer>();

            var trainer = await repo.GetByIdAsync(id);

            if (trainer == null) return;

            repo.DeleteAsync(trainer);
            await _unitOfWork.SaveChangesAsync();
        }


      
    }
}
