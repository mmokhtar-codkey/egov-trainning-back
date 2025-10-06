
using Training.Common.DTO.Lookup.Status;
using Training.Domain.Entities.Lookups;


namespace Training.Application.Mapping
{
    public partial class MappingService
    {
        public void MapStatus()
        {

            CreateMap<Status, StatusDto>().ReverseMap();
            CreateMap<Status, EditStatusDto>().ReverseMap();
            CreateMap<Status, AddStatusDto>().ReverseMap();
        }
    }
}