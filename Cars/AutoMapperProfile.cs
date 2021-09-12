using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cars
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<ModelDto, Model>();
            CreateMap<Model, ModelDto>();

            CreateMap<BrandDto, Brand>();
            CreateMap<Brand, BrandDto>();
        }
    }
}
