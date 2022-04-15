using AutoMapper;
using ResourceManagementSystem.Application.Mapping;
using ResourceManagementSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceManagementSystem.Application.ViewModel.Items
{
    public class ItemOfListVM : IMapFrom<Item>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int CountInUse { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Item, ItemOfListVM>()
                .ForMember(p => p.Id, opt => opt.MapFrom(s => s.Id.ToString().ToLower()))
                .ForMember(p => p.Count, opt => opt.MapFrom(s => s.Serials != null ? s.Serials.Count : 0))
                .ForMember(p => p.CountInUse, opt => opt.MapFrom(s => s.Reservations.Count));
        }
    }
}
