using AutoMapper;
using Library.Interfaces;
using Library.Models;
using Library.Models.DTO;
using Library.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        public GenreController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// 2.7.4.1.	Просмотр всех жанров. (без книг) 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAllGenres")]
        public List<GenreDto> GetAllGenres()
        {
            return _mapper.Map<List<GenreDto>>(unitOfWork.GetRepository<Genre>().Get());
        }

        /// <summary>
        /// 2.7.4.2.	Добавление нового жанра. (без книги) 
        /// </summary>
        /// <param name="genreDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Genre> AddGenre([FromBody] GenreDto genreDTO)
        {
            Genre genre = _mapper.Map<Genre>(genreDTO);
            unitOfWork.GetRepository<Genre>().Insert(genre);
            unitOfWork.Save();
            return CreatedAtAction("AddGenre", new { id = genre.Id }, genre);
        }

        /// <summary>
        /// 2.7.4.3.	Вывод статистики Жанр - количество книг
        /// </summary>
        /// <returns></returns>
        [HttpGet("getStatictic")]
        public List<StatisticGenreDto> GetStatictic()
        {
            List<StatisticGenreDto> statistic = new();
            var genres = _mapper.Map<List<GenreDto>>(unitOfWork.GetRepository<Genre>()
                                                .Get(includeProperties: "Book"));

            foreach (var genre in genres)
            {
                statistic.Add(new StatisticGenreDto
                {
                    GenreName = genre.GenreName,
                    Count = genre.Books.Count()

                });
            }
            return statistic;
        }
    }
}
