using AutoMapper;
using FluentValidation;
using ResourceManagementSystem.Application.Mapping;
using ResourceManagementSystem.Application.ViewModel.ExtraViewModel;
using ResourceManagementSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceManagementSystem.Application.ViewModel.Items
{
    public class ItemVM :IMapFrom<Item>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<AddRemoveStatusVM> SerialsList { get; set; }
        public List<AddRemoveStatusVM> DepartmentsList { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Item, ItemVM>()
                .ForMember(p => p.SerialsList, opt => opt.Ignore())
                .ForMember(p => p.DepartmentsList, opt => opt.Ignore());
        }
    }

    public class CreateUserValidation : AbstractValidator<ItemVM>
    {
        public CreateUserValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.SerialsList).NotNull().NotEmpty();
            RuleFor(x => x.DepartmentsList).NotNull().NotEmpty();
        }
    }
}
