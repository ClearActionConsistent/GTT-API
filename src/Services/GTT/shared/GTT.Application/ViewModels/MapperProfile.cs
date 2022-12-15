using AutoMapper;
using GTT.Application.DtoModels;

namespace GTT.Application.ViewModels
{
    public class MapperProfile : Profile
    {
       public MapperProfile()
       {
            //Create class not custom anything
            CreateMap<ClassesVM, ClassDto>();
        }
    }
}
