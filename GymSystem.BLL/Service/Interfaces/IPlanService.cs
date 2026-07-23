using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymSystem.BLL.ViewModels.PlanViewModels;

namespace GymSystem.BLL.Service.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetAllAsync();


        Task<PlanViewModel?> GetDetailsAsync(int id);
        Task<PlanToUpdateViewModel?> GetByIdAsync(int id);
        Task CreateAsync(CreatePlanViewModel model);
        Task UpdateAsync(int id, PlanToUpdateViewModel model);
        Task ToggleStatusAsync(int id);
    }
}
