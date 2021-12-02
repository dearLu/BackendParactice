using AutoMapper;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services
{
    public class GenreProfile : Profile
    {
        GenreProfile()
        {
            CreateMap<Genre, GenreDto>()
                .ForMember(dest =>
                    dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest =>
                    dest.GenreName,
                    opt => opt.MapFrom(src => src.GenreName))
                .ReverseMap();
        }
    }
}
