using AutoMapper;
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
        private UnitOfWork unitOfWork = new UnitOfWork();
        private readonly IMapper _mapper;
        public GenreController(IMapper mapper)
        {           
            _mapper = mapper;
        }

        /// <summary>
        /// 2.7.4.1.	Просмотр всех жанров. (без книг) 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllGenres")]
        public List<GenreDTO> GetAllGenres()
        {
            return (List<GenreDTO>)_mapper.Map<IEnumerable<GenreDTO>>(unitOfWork.GenreRepository.Get().ToList());
        }

        /// <summary>
        /// 2.7.4.2.	Добавление нового жанра. (без книги) 
        /// </summary>
        /// <param name="genreDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<Genre> AddGenre([FromBody] GenreDTO genreDTO)
        {
            Genre genre = _mapper.Map<Genre>(genreDTO);
            unitOfWork.GenreRepository.Insert(genre);
            unitOfWork.Save();
            return CreatedAtAction("AddGenre", new { id = genre.Id }, genre);
        }

        /// <summary>
        /// 2.7.4.3.	Вывод статистики Жанр - количество книг
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetStatictic")]
        public List<StatisticGenreDTO> GetStatictic()
        {
            List<StatisticGenreDTO> statistic = new();
            var genres = (List<GenreDTO>)_mapper.Map<IEnumerable<GenreDTO>>(unitOfWork.GenreRepository
                                                .Get(includeProperties: "Book")
                                                .ToList());
            foreach (var genre in genres)
            {
                statistic.Add(new StatisticGenreDTO
                {
                    GenreName = genre.GenreName,
                    Count = genre.Books.Count()

                });


            }

            return statistic;
        }


    }
}
