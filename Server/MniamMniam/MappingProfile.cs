using AutoMapper;
using MniamMniam.Models.CookBookModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MniamMniam
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Recipe, Recipe>()
                .ForMember(rec => rec.UpdatedAt, opt => opt.MapFrom(rec => DateTime.Now))
                .ForMember(rec => rec.CreatedAt, opt => opt.Ignore())
                .ForMember(rec => rec.ApplicationUserId, opt => opt.Ignore());
        }
    }
}
