using AutoMapper;
using GymSystem.BLL.Service.Interfaces;
using GymSystem.BLL.ViewModels.MemberViewModels;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Data.Models.Enums;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace GymSystem.BLL.Service.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var emailexist = await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.Email == model.Email, ct);
            var phoneexist = await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.Phone == model.Phone, ct);
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
            _unitOfWork.GetRepository<Member>().AddAsync(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if (member is null) return false;
            var hasFutureBooking = await _unitOfWork.GetRepository<Booking>().AnyAsync(x => x.MemberId == memberId
            && x.Session.StartDate > DateTime.Now,ct);
            if (hasFutureBooking) return false;
            _unitOfWork.GetRepository<Member>().DeleteAsync(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;

        }

        public async Task<IEnumerable<MemberViewModel>> GetALlMemberAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
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
            var record= await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(x=> x.MemberId == MemberId);
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
            var Member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(MemberId, ct);
            if (Member == null) return null;
            var model = new MemberViewModel()
            {
                Name = Member.Name,
                Phone = Member.Phone,
                DateOfBirth = Member.DateOfBirth.ToShortDateString(),
                Gender = Member.Gender.ToString(),
                Address = $"{Member.address.Street} {Member.address.BuildingNumber} {Member.address.City}"

            };
            var activemembership = await _unitOfWork.GetRepository<Membership>().FirstOrDefaultAsync(x => x.MemberId == MemberId && x.EndDate >DateTime.Now);
            if (activemembership is not null) 
            {
                var activeplan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(activemembership.PlanId);
                model.PlanName = activeplan.Name;
                model.MembershipStartDate = activemembership.CreatedAt.ToString();
                model.MembershipEndDate = activemembership.EndDate.ToString();
            }
            return model;
        }

        public async Task<MemberToUpdateViewModel> GetMemberToUpdate(int MemberId, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(MemberId, ct);
            if (member is null) return null;
            else
                return new MemberToUpdateViewModel()
                {
                    Name = member.Name,
                    Phone = member.Phone,
                    Email = member.Email,
                    BuildingNumber = member.address.BuildingNumber,
                    City = member.address.City,
                    Street = member.address.Street,

                };

        }

        public async Task<bool> UpdateMemberDetails(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (member is null) return false;
            var emailexist = await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.Email == model.Email && x.Id != id);
            var phoneexist = await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.Phone == model.Phone && x.Id != id);
            if (emailexist || phoneexist) return false;
            member.Email= model.Email;
            member.Phone= model.Phone;
            member.address.Street = model.Street;
            member.address.City = model.City;
            member.address.BuildingNumber= model.BuildingNumber;
            member.UpdatedAt = DateTime.Now;
            _unitOfWork.GetRepository<Member>().UpdateAsync(member);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }
    }
}
