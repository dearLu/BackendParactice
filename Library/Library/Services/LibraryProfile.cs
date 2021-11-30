using AutoMapper;
using Library.Models;
using Library.Models.DTO;
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
			CreateMap<Person, HumanDto>()
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

			CreateMap<Genre, GenreDto>()
				.ForMember(dest =>
					dest.Id,
					opt => opt.MapFrom(src => src.Id))
				.ForMember(dest =>
					dest.GenreName,
					opt => opt.MapFrom(src => src.GenreName))
				.ReverseMap();

			CreateMap<Author, AuthorDto>()
				.ForMember(dest =>
					dest.Id,
					opt => opt.MapFrom(src => src.Id))
				.ForMember(dest =>
					dest.FirstName,
					opt => opt.MapFrom(src => src.FirstName))
				.ForMember(dest =>
					dest.LastName,
					opt => opt.MapFrom(src => src.LastName))
				.ForMember(dest =>
					dest.MiddleName,
					opt => opt.MapFrom(src => src.MiddleName))
				.ReverseMap();

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
