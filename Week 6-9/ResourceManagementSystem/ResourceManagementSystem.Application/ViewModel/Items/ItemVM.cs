using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
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
        public string ImagePath { get; set; }
        public IFormFile ImageObj { get; set; }
        public List<AddRemoveStatusVM> SerialsList { get; set; } = new List<AddRemoveStatusVM>();
        public List<AddRemoveStatusVM> DepartmentsList { get; set; } = new List<AddRemoveStatusVM>();


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Item, ItemVM>()
                .ForMember(p => p.Id, opt => opt.MapFrom(s => s.Id.ToString().ToLower()))
                .ForMember(p => p.ImageObj, opt => opt.Ignore())
                .ForMember(p => p.SerialsList, opt => opt.Ignore())
                .ForMember(p => p.DepartmentsList, opt => opt.Ignore());

            profile.CreateMap<ItemVM, Item>()
                .ForMember(p => p.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(p => p.ImagePath, opt => opt.Ignore());
        }
    }

    public class ItemValidation : AbstractValidator<ItemVM>
    {
        public ItemValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.SerialsList).NotNull().NotEmpty();
            RuleFor(x => x.DepartmentsList).NotNull().NotEmpty();
        }
    }
}
