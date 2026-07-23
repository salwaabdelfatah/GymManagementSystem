using GymSystem.BLL.ViewModels.TrainerViewModel;
using GymSystem.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Service.Interfaces
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerViewModel>> GetAllAsync();
        Task<TrainerDetailsViewModel?> GetByIdAsync(int id);
        Task CreateAsync(CreateTrainerViewModel vm);
        Task UpdateAsync(UpdateTrainerViewModel vm);
        Task DeleteAsync(int id);
    }
}
