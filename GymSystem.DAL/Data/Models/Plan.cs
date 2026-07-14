using System.Buffers.Text;

namespace GymSystem.DAL.Data.Models
{
    public class Plan : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int DurationDays { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        #region Relation
        public ICollection<Membership> PlanMember { get; set; } = default!;
        #endregion

    }
}
