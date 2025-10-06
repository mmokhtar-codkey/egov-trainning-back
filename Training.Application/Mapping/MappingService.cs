using AutoMapper;

namespace Training.Application.Mapping
{
    public partial class MappingService : Profile
    {
        public MappingService()
        {

            MapAction();
            MapStatus();
            MapCategory();

        }
    }
}