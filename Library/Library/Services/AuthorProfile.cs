using AutoMapper;
using Library.Models;
using Library.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services
{
    public class AuthorProfile : Profile
    {
		public AuthorProfile()
		{
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
		}
	}
}
