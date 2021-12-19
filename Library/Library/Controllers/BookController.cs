using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.Models;
using Library.Services;
using AutoMapper;
using Library.Models.DTO;
using Library.Interfaces;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BookController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// 2.7.2.1.	Книга может быть добавлена (POST) (вместе с автором и жанром) книга + автор + жанр
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        // POST: api/Book
        [HttpPost]
        public ActionResult<Book> AddBook([FromBody] BookDto bookDTO)
        {

            Book book = _mapper.Map<Book>(bookDTO);

            _unitOfWork.GetRepository<Book>().Insert(book);

            return CreatedAtAction("AddBook", new { id = book.Id }, book);
        }

        /// <summary>
        /// 2.7.2.2.	Книга может быть удалена из списка библиотеки (но только если она не у пользователя) по ID (ок, или
        /// ошибка, что книга у пользователя)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Book/5
        [HttpDelete("{id}")]
        public IActionResult DeleteBook([FromRoute] int id)
        {
            var book = _unitOfWork.GetRepository<Book>().GetById(id);
            if (book == null)
            {
                return NotFound();
            }

            if (book.Persons.Count() == 0)
            {
                _unitOfWork.GetRepository<Book>().Delete(id);
                _unitOfWork.Save();
            }
            else
            {
                return  Content($"Книга {book.Name} уже у пользователя. Невозможно удалить");              
            }

             return Ok();
        }

        /// <summary>
        /// 2.7.2.3.	Книге можно присвоить новый жанр, или удалить один из имеющихся (PUT с телом.На вход сущность Book 
        /// или её Dto) При добавлении или удалении вы должны просто либо добавлять запись, либо удалять из списка 
        /// жанров. 
        /// Каскадно удалять все жанры и книги с таким жанром нельзя! Книга + жанр + автор
        /// </summary>
        /// <param name="bookDTO"></param>
        /// <returns></returns>

        [HttpPut("putBook")]
        public AuthorBooksGenresDto PutBook(BookDto bookDTO)
        {
            Book book = _mapper.Map<Book>(bookDTO);
            Genre genre = _unitOfWork.GetRepository<Genre>().Get(e => !book.Genres.Contains(e)).FirstOrDefault();
           
            if (book.Genres.Any(e => e.Id == genre.Id))
            {
                book.Genres.Remove(genre);
                genre.Books.Remove(book);
            }
            else 
            {
                book.Genres.Add(genre);
                genre.Books.Add(book);
            }
            _unitOfWork.GetRepository<Genre>().Update(genre);
            _unitOfWork.GetRepository<Book>().Update(book);
            _unitOfWork.Save();

            AuthorBooksGenresDto obj = new();
            obj.Author = _mapper.Map<AuthorDto>(book.Author);
            obj.Book = _mapper.Map<BookDto>(book);
            obj.Genre = _mapper.Map<List<GenreDto>>(book.Genres);

            return obj;
        }

        /// <summary>
        /// 2.7.2.4. Можно получить список всех книг с фильтром по автору (По любой комбинации трёх полей сущности автор.
        /// Имеется  ввиду условие equals + and )
        /// </summary>
        /// <param name="authorDTO"></param>
        /// <returns></returns>
        // GET: api/Book
        [HttpGet("getBooksByAuthor")]
        public IEnumerable<Book> GetBooksByAuthor([FromBody] AuthorDto authorDTO)
        {
            Author author = _mapper.Map<Author>(authorDTO);
            var books = _unitOfWork.GetRepository<Book>().Get(e => e.Author.FirstName.ToLower() == author.FirstName.ToLower()
                                                            && e.Author.LastName.ToLower() == author.LastName.ToLower()
                                                            && e.Author.MiddleName.ToLower() == author.MiddleName.ToLower(), 
                                                            null, 
                                                            includeProperties: "Author,Genre");

            return books;
        }


        /// <summary>
        /// 2.7.2.5.	Можно получить список книг по жанру. Книга + жанр + автор
        /// </summary>
        /// <param name="genreDTO"></param>
        /// <returns></returns>
        [HttpGet("getBooksByGenre")]
        public List<AuthorBooksGenresDto> GetBooksByGenre([FromBody]  GenreDto genreDTO)
        {
            List<AuthorBooksGenresDto> listBooks = new();
            Genre genre = _mapper.Map<Genre>(genreDTO);

            var books = _unitOfWork.GetRepository<Book>().Get(e => e.Genres.Any(e => e.Id == genre.Id),
                                                            null,
                                                            includeProperties:"Author,Genre");
            foreach (var book in books)
            {
                listBooks.Add(new AuthorBooksGenresDto
                {
                    Author = _mapper.Map<AuthorDto>(book.Author),
                    Book = _mapper.Map<BookDto>(book),
                    Genre = _mapper.Map<List<GenreDto>>(book.Genres.Where(e => e.Id == genre.Id))
                });

            }

            return listBooks;
        }

    }
}
