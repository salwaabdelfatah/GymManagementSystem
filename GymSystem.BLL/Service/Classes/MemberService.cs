using GymSystem.BLL.Service.Interfaces;
using GymSystem.BLL.ViewModels.MemberViewModels;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Data.Models.Enums;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace GymSystem.BLL.Service.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _membersRepository;
        private readonly IGenericRepository<Membership> _membershipRepository;
        private readonly IGenericRepository<Plan> _planRepository;
        private readonly IGenericRepository<HealthRecord> _healthrecordRepository;

        public MemberService(IGenericRepository<Member> memberRepository,IGenericRepository<Membership> membershipRepository
            ,IGenericRepository<Plan> PlanRepository,IGenericRepository<HealthRecord> HealthrecordRepository)
        {
            this._membersRepository = memberRepository;
            this._membershipRepository = membershipRepository;
            _planRepository = PlanRepository;
            _healthrecordRepository = HealthrecordRepository;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var emailexist = await _membersRepository.AnyAsync(x => x.Email == model.Email, ct);
            var phoneexist = await _membersRepository.AnyAsync(x => x.Phone == model.Phone, ct);
            if (emailexist || phoneexist) return false;
            var member = new Member()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                address = new Address()
                {
                    City = model.City,
                    Street = model.Street,
                    BuildingNumber = model.BuildingNumber
                },
                HealthRecord = new HealthRecord()
                {
                    Weight = model.HealthRecordViewModel.Weight,
                    Height = model.HealthRecordViewModel.Height,
                    Note = model.HealthRecordViewModel.Note,
                    BloodType = model.HealthRecordViewModel.BloodType
                }
            };
            var result = await _membersRepository.AddAsync(member);
            return result > 0;
        }

        public async Task<IEnumerable<MemberViewModel>> GetALlMemberAsync(CancellationToken ct = default)
        {
            var members = await _membersRepository.GetAllAsync(ct: ct);
            if (!members.Any()) return [];
            List<MemberViewModel> memberViewModels = new List<MemberViewModel>();
            foreach (var member in members)
            {
                var memberviewmodel = new MemberViewModel()
                {
                    Name = member.Name,
                    Email = member.Email,
                    Phone = member.Phone,
                    Photo = member.Photo,
                    Id = member.Id
                    ,
                    Gender = member.Gender.ToString()
                };
                memberViewModels.Add(memberviewmodel);
            }
            return memberViewModels;
        }

        public async Task<HealthRecordViewModel?> GetHealthRecordDetails(int MemberId, CancellationToken ct = default)
        {
            var record=await _healthrecordRepository.FirstOrDefaultAsync(x=> x.MemberId == MemberId);
            if (record == null) return null;
            else
                return new HealthRecordViewModel()
                {
                    Height= record.Height,
                    Weight=record.Weight
                    ,BloodType=record.BloodType,
                    Note=record.Note
                };
            
        }

        public async Task<MemberViewModel?> GetMemberDetailsById(int MemberId, CancellationToken ct = default)
        {
            var Member = await _membersRepository.GetByIdAsync(MemberId, ct);
            if (Member == null) return null;
            var model = new MemberViewModel()
            {
                Name = Member.Name,
                Phone = Member.Phone,
                DateOfBirth = Member.DateOfBirth.ToShortDateString(),
                Gender = Member.Gender.ToString(),
                Address = $"{Member.address.Street} {Member.address.BuildingNumber} {Member.address.City}"

            };
            var activemembership = await _membershipRepository.FirstOrDefaultAsync(x => x.MemberId == MemberId && x.EndDate >DateTime.Now);
            if (activemembership is not null) 
            {
                var activeplan = await _planRepository.GetByIdAsync(activemembership.PlanId);
                model.PlanName = activeplan.Name;
                model.MembershipStartDate = activemembership.CreatedAt.ToString();
                model.MembershipEndDate = activemembership.EndDate.ToString();
            }
            return model;
        }
    }
}
