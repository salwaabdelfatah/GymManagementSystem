using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GymSystem.BLL.ViewModels.PlanViewModels
{
    public class CreatePlanViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int DurationDays { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string Description { get; set; }
    }
}
