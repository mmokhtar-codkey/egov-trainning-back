
using Training.Common.DTO.Lookup.Action;
using Training.Domain.Entities.Lookups;



namespace Training.Application.Mapping
{
    public partial class MappingService
    {
        public void MapAction()
        {
            CreateMap<Action, ActionDto>().ReverseMap();
            CreateMap<Action, EditActionDto>().ReverseMap();
            CreateMap<Action, AddActionDto>().ReverseMap();
        }
    }
}