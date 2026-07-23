using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymSystem.DAL.Data.Models.Enums;
namespace GymSystem.BLL.ViewModels.TrainerViewModel
{
    public class UpdateTrainerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Speciality Speciality { get; set; }

        public string City { get; set; }
        public string Street { get; set; }
        public int BuildingNumber { get; set; }
    }
}
