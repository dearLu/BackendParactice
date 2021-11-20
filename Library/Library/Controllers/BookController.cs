using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.Models;
using Library.Interfaces;
using Library.Services;
using AutoMapper;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly  IRepository<Book> repo;
        private readonly IMapper _mapper;
        public BookController(IMapper mapper)
        {
            repo = new BookRepository();
            _mapper = mapper;

        }

        // GET: api/Book
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }

        // GET: api/Book/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Book/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, BookDTO book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// 7.2.1.	Книга может быть добавлена (POST) (вместе с автором и жанром) книга + автор + жанр
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        // POST: api/Book
        [HttpPost]
        public async Task<ActionResult<Book>> AddBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("AddBook", new { id = book.Id }, book);
        }

        /// <summary>
        /// 7.2.2.	Книга может быть удалена из списка библиотеки (но только если она не у пользователя) по ID (ок, или
        /// ошибка, что книга у пользователя)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Book/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            if (!FindPersonWithThisBook(id))
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
            else
            {
                return  Content($"Книга {book.Name} уже у пользователя. Невозможно удалить");              
            }

             return Ok();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
        private bool FindPersonWithThisBook(int id) 
        {
            return _context.Persons.Any(e => e.Books.Any(q => e.Id == id));
        }
    }
}
