using GymSystem.BLL.ViewModels.MemberViewModels;
using GymSystem.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Service.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModel>> GetALlMemberAsync(CancellationToken ct = default);
        Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default);
        Task<MemberViewModel?> GetMemberDetailsById (int MembeId, CancellationToken ct = default);
        Task<HealthRecordViewModel?> GetHealthRecordDetails (int MemberId, CancellationToken ct = default);

    }
}
