using AutoMapper;
using FluentValidation;
using ResourceManagementSystem.Application.Mapping;
using ResourceManagementSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceManagementSystem.Application.ViewModel.Users
{
    public class DetailsEditUserVM : IMapFrom<AppUser>
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public bool IsActive { get; set; } = true;

        public List<AddRemoveStatusVM> DepartmentsList { get; set; }
        public List<AddRemoveStatusVM> RolesList { get; set; }
        public List<AddRemoveStatusVM> ReservationItemList { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<AppUser, DetailsEditUserVM>()
                .ForMember(d => d.Password, opt => opt.Ignore())
                .ForMember(d => d.Phone, opt => opt.MapFrom(s => s.PhoneNumber))
                .ForMember(d => d.IsActive, opt => opt.MapFrom(s => s.isActive))
                .ForMember(d => d.DepartmentsList, opt => opt.Ignore())
                .ForMember(d => d.RolesList, opt => opt.Ignore())
                .ForMember(d => d.ReservationItemList, opt => opt.Ignore())
                .ReverseMap();
        }
    }

    public class DetailsEditUserValidation : AbstractValidator<DetailsEditUserVM>
    {
        public DetailsEditUserValidation()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.FullName).NotEmpty();
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
