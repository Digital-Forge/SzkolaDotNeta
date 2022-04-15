using AutoMapper;
using ResourceManagementSystem.Application.Mapping;
using ResourceManagementSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceManagementSystem.Application.ViewModel.Departments
{
    public class DepartmentOfListVM : IMapFrom<Department>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int CountOfUsers { get; set; }
        public int CountOfItems { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Department, DepartmentOfListVM>()
                .ForMember(p => p.Id, opt => opt.MapFrom(s => s.Id.ToString().ToLower()))
                .ForMember(p => p.CountOfItems, opt => opt.MapFrom(s => s.Items != null ? s.Items.Count : 0))
                .ForMember(p => p.CountOfUsers, opt => opt.MapFrom(s => s.AppUsers != null ? s.AppUsers.Count : 0));
        }
    }
}
