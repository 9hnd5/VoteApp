using AutoMapper;

namespace VoteApp.Application.Commons.Mappings
{
    public interface IMapFrom<T>
    {
        public void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
