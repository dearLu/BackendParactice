﻿using Library.Models;
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
    public class BookDTOController : ControllerBase
    {

        private readonly ILogger<BookDTOController> _logger;

        public BookDTOController(ILogger<BookDTOController> logger)
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
            return DataDTO.AllBook;
        }

        /// <summary>
        /// 2.2.2 - Добавьте в список книг возможность сделать запрос с сортировкой по автору, имени книги и жанру
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetFilterBooks")]
        public IEnumerable<BookDTO> GetFilterBooks([FromRoute] bool title = false,bool genre = false)
        {
           
            if (title)
                return DataDTO.AllBook.OrderBy(e => e.Title).ToList();
            else if(genre)
                return DataDTO.AllBook.OrderBy(e => e.Genres).ToList();
            else
                return DataDTO.AllBook.OrderBy(e => e.Author).ToList();

        }

        /// <summary>
        /// 1.4.1.2 - метод Get, возвращающий список всех книг по автору (фильтрация AuthorId)
        /// </summary>
        /// <param name="AuthorId"></param>
        /// <returns></returns>
        [HttpGet("GetBookByAuthor")]
        public IEnumerable<BookDTO> GetBookByAuthor([FromRoute] int AuthorId) 
        {
            return DataDTO.AllBook.Where(e => e.Author.Id == AuthorId).ToList();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById([FromRoute] int id)
        {
            var book = DataDTO.AllBook.FirstOrDefault(e => e.Id == id);
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
        public ActionResult<BookDTO> AddBookDTO([FromBody] BookDTO book)
        {
            DataDTO.AllBook.Add(book);
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        /// <summary>
        /// 1.4.3 - метод DELETE, удаляющий книгу
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteBook([FromRoute] int id)
        {
            var book = DataDTO.AllBook.Where(e => e.Id == id).FirstOrDefault();

            if (book == null)
            {
                return NotFound();
            }

            DataDTO.AllBook.Remove(book);

            return NoContent();
        }
    }
}
