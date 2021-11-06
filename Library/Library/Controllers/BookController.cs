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
    public class BookController : ControllerBase
    {

        private readonly ILogger<BookController> _logger;

        public BookController(ILogger<BookController> logger)
        {
            _logger = logger;

        }

        /// <summary>
        /// 1.4.1.1 - метод Get, возвращающий список всех книг
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllBook")]
        public IEnumerable<BookDTO> GetAllBook()
        {

            return DataDTO.allBook;
        }

        /// <summary>
        /// 1.4.1.2 - метод Get, возвращающий список всех книг по автору (фильтрация AuthorId)
        /// </summary>
        /// <param name="AuthorId"></param>
        /// <returns></returns>
        [HttpGet("GetBookByAuthor")]
        public IEnumerable<BookDTO> GetBookByAuthor(int AuthorId) 
        {
            return DataDTO.allBook.Where(e => e.Author.Id == AuthorId).ToList();

        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var book = DataDTO.allBook.Where(e => e.Id == id).FirstOrDefault();
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
        public ActionResult<BookDTO> AddHumanBookDTO(BookDTO book)
        {
            if (book == null)
            {
                return BadRequest();
            }

            DataDTO.allBook.Add(book);


            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        /// <summary>
        /// 1.4.3 - метод DELETE, удаляющий книгу
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = DataDTO.allBook.Where(e => e.Id == id).FirstOrDefault();

            if (book == null)
            {
                return NotFound();
            }

            DataDTO.allBook.Remove(book);

            return NoContent();
        }
    }
}
