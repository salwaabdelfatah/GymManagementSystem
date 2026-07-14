using GymSystem.DAL.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Data.Models
{
    public class Trainer:GymUser
    {
        //hire date == created at
        public Speciality speciality { get; set; }
        #region Relationship
        public ICollection<Session> Sessions { get; set; } = default!;

        #endregion

    }
}
