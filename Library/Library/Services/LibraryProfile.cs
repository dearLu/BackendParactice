using AutoMapper;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services
{
    public class LibraryProfile : Profile
	{
		public LibraryProfile()
		{
			CreateMap<Book, BookDTO>().ReverseMap();
			CreateMap<Person, HumanDTO>()
				.ForMember(dest =>
					dest.Id,
					opt => opt.MapFrom(src => src.Id))
				.ForMember(dest =>
					dest.Name,
					opt => opt.MapFrom(src => src.FirstName))
				.ForMember(dest =>
					dest.Surname,
					opt => opt.MapFrom(src => src.LastName))
				.ForMember(dest =>
					dest.Patronymic,
					opt => opt.MapFrom(src => src.MiddleName))
				.ForMember(dest =>
					dest.Birthday,
					opt => opt.MapFrom(src => src.BirthDate))
				.ReverseMap();
		}
	}
}
