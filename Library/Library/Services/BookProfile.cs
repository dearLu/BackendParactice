using AutoMapper;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services
{
    public class BookProfile : Profile
    {
        BookProfile() 
        {
			CreateMap<Book, BookDto>()
				.ForMember(dest =>
					dest.Id,
					opt => opt.MapFrom(src => src.Id))
				.ForMember(dest =>
					dest.Title,
					opt => opt.MapFrom(src => src.Name))
				.ForMember(dest =>
					dest.Author,
					opt => opt.MapFrom(src => src.Author))
				.ReverseMap();
		}
    }
}
