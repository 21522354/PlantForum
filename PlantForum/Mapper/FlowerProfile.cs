using AutoMapper;
using PlantForum.Data.Models;
using PlantForum.Dtos;

namespace PlantForum.Mapper
{
    public class FlowerProfile : Profile
    {
        public FlowerProfile()
        {
            CreateMap<FlowerPostReadDto, FlowerPost>().ReverseMap();
            CreateMap<FlowerPostRequestDto, FlowerPost>().ReverseMap();     
        }
    }
}
