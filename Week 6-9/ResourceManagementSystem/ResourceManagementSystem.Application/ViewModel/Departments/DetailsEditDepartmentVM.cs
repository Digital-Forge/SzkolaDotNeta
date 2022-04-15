using AutoMapper;
using ResourceManagementSystem.Application.Mapping;
using ResourceManagementSystem.Application.ViewModel.ExtraViewModel;
using ResourceManagementSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceManagementSystem.Application.ViewModel.Departments
{
    public class DetailsEditDepartmentVM : IMapFrom<Department>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<AddRemoveStatusVM> UsersList { get; set; } = new List<AddRemoveStatusVM>();
        public List<AddRemoveStatusVM> ItemsList { get; set; } = new List<AddRemoveStatusVM>();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Department, DetailsEditDepartmentVM>()
                .ForMember(p => p.Id, opt => opt.MapFrom(s => s.Id.ToString().ToLower()))
                .ForMember(p => p.UsersList, opt => opt.Ignore())
                .ForMember(p => p.ItemsList, opt => opt.Ignore());

            profile.CreateMap<DetailsEditDepartmentVM, Department>()
                .ForMember(p => p.Id, opt => opt.MapFrom(s => new Guid(s.Id)));
                
        }
    }
}
