using Training.Common.DTO.Lookup.Category;
using Training.Domain.Entities.Lookups;

namespace Training.Application.Mapping
{
    public partial class MappingService
    {
        public void MapCategory()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, EditCategoryDto>().ReverseMap();
            CreateMap<Category, AddCategoryDto>().ReverseMap();
        }
    }
}
