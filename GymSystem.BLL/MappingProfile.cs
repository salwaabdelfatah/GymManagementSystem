using AutoMapper;
using AutoMapper.Execution;
using GymSystem.BLL.ViewModels.MemberViewModels;
using GymSystem.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<DAL.Data.Models.Member, MemberViewModel>()
                .ForMember(dest=>dest.Address,opt=>opt.MapFrom(src=>$"{src.address.BuildingNumber}" +
                $"-{src.address.Street}-{src.address.City}"))
                .ForMember(dest=>dest.DateOfBirth,opt=>opt.MapFrom(src=> src.DateOfBirth
                .ToShortDateString()));

        }
    }
}
