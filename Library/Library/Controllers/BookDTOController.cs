                                              using AutoMapper;
using Library.Interfaces;
using Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Library.Controllers
{
    /// <summary>
    /// 1.4 - Контроллер, который отвечает за книгу
    /// </summary>
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class BookDtoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BookDtoController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// 1.4.1.1 - метод Get, возвращающий список всех книг
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAllBook")]
        public List<BookDto> GetAllBook()
        {
            return _mapper.Map<List<BookDto>>(_unitOfWork.GetRepository<Book>().Get());
        }

        /// <summary>
        /// 2.2.2 - Добавьте в список книг возможность сделать запрос с сортировкой по автору, имени книги и жанру
        /// </summary>
        /// <returns></returns>
        [HttpGet("getFilterBooks")]
        public List<BookDto> GetFilterBooks(bool descending= false )
        {
            if(descending)
                 return _mapper.Map<List<BookDto>>(_unitOfWork.GetRepository<Book>()
                                                        .Get(orderBy: q => q.OrderByDescending(d => d.Name)
                                                                            .ThenByDescending(d => d.Genres)
                                                                            .ThenByDescending(d => d.Author),
                                                                            includeProperties: "Author,Genre"));

                return _mapper.Map<List<BookDto>>(_unitOfWork.GetRepository<Book>()
                                                            .Get(orderBy: q => q.OrderBy(d => d.Name)
                                                                                .ThenBy(d => d.Genres)
                                                                                .ThenBy(d => d.Author),
                                                                                includeProperties: "Author,Genre"));


        }

        /// <summary>
        /// 1.4.1.2 - метод Get, возвращающий список всех книг по автору (фильтрация AuthorId)
        /// </summary>
        /// <param name="AuthorId"></param>
        /// <returns></returns>
        [HttpGet("getBookByAuthor")]
        public List<BookDto> GetBookByAuthor([FromRoute] int AuthorId) 
        {

            return _mapper.Map<List<BookDto>>(_unitOfWork.GetRepository<Book>()
                                                        .Get(e => e.Author.Id == AuthorId,
                                                            null,
                                                            includeProperties: "Author,Genre"));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById([FromRoute] int id)
        {
            var book = _mapper.Map<List<BookDto>>(_unitOfWork.GetRepository<Book>()
                                                        .Get(q => q.Id == id))
                                                        .FirstOrDefault();
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        /// <summary>
        /// 1.4.2 - метод POST, добавляющий новую книгу
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<BookDto> AddBookDTO([FromBody] BookDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            _unitOfWork.GetRepository<Book>().Insert(book);
            _unitOfWork.Save();

            return CreatedAtAction("AddBookDTO", new { id = book.Id }, bookDto);
        }

        /// <summary>
        /// 1.4.3 - метод DELETE, удаляющий книгу
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteBook([FromRoute] int id)
        {

            var book = _unitOfWork.GetRepository<Book>()
                                            .Get(q => q.Id == id)
                                            .FirstOrDefault();
            if (book == null)
            {
                return NotFound();
            }

            _unitOfWork.GetRepository<Book>().Delete(book);

            return NoContent();
        }
    }
}
