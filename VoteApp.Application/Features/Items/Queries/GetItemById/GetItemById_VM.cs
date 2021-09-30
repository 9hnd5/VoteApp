using AutoMapper;
using System;
using VoteApp.Application.Commons.Mappings;
using VoteApp.Domain.Entities;

namespace VoteApp.Application.Features.Items.Queries.GetItemById
{
    public class GetItemById_VM : IMapFrom<Item>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int Vote { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public void Mapping(Profile p) => p.CreateMap<Item, GetItemById_VM>();
    }
}
