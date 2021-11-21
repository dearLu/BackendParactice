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

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private readonly IMapper _mapper;
        public BookController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// 2.7.2.1.	Книга может быть добавлена (POST) (вместе с автором и жанром) книга + автор + жанр
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        // POST: api/Book
        [HttpPost]
        public ActionResult<Book> AddBook([FromBody] BookDTO bookDTO)
        {

            Book book = _mapper.Map<Book>(bookDTO);

            unitOfWork.BookRepository.Insert(book);

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
            var book = unitOfWork.BookRepository.GetByID(id);
            if (book == null)
            {
                return NotFound();
            }

            if (book.Persons.Count() == 0)
            {
                unitOfWork.BookRepository.Delete(id);
                unitOfWork.Save();
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

        [HttpPut("PutBook")]
        public AuthorBooksGenresDTO PutBook([FromBody] BookDTO bookDTO)
        {
            Genre genre = unitOfWork.GenreRepository.Get().ToList().LastOrDefault();
            Book book = _mapper.Map<Book>(bookDTO);
            if (book.Genres.Any(e => e.GenreName == genre.GenreName))
            {
                book.Genres.Remove(genre);
                genre.Books.Remove(book);
            }
            else 
            {
                book.Genres.Add(genre);
                genre.Books.Add(book);
            }
            unitOfWork.GenreRepository.Update(genre);
            unitOfWork.BookRepository.Update(book);
            unitOfWork.Save();

            AuthorBooksGenresDTO obj = new();
            obj.Author = _mapper.Map<AuthorDTO>(book.Author);
            obj.Book = _mapper.Map<BookDTO>(book);
            obj.Genre = (List<GenreDTO>)_mapper.Map<IEnumerable<GenreDTO>>(book.Genres);

            return obj;
        }

        /// <summary>
        /// 2.7.2.4. Можно получить список всех книг с фильтром по автору (По любой комбинации трёх полей сущности автор.
        /// Имеется  ввиду условие equals + and )
        /// </summary>
        /// <param name="authorDTO"></param>
        /// <returns></returns>
        // GET: api/Book
        [HttpGet("GetBooksByAuthor")]
        public IEnumerable<Book> GetBooksByAuthor([FromBody] AuthorDTO authorDTO)
        {
            Author author = _mapper.Map<Author>(authorDTO);
            var books = unitOfWork.BookRepository.Get(includeProperties: "Author,Genre")
                                                    .ToList()
                                                    .Where(e => e.Author.FirstName == author.FirstName
                                                    && e.Author.LastName == author.LastName
                                                    && e.Author.MiddleName == author.MiddleName)
                                                    .ToList();


            return books;
        }


        /// <summary>
        /// 2.7.2.5.	Можно получить список книг по жанру. Книга + жанр + автор
        /// </summary>
        /// <param name="genreDTO"></param>
        /// <returns></returns>
        [HttpGet("GetBooksByGenre")]
        public List<AuthorBooksGenresDTO> GetBooksByGenre([FromBody]  GenreDTO genreDTO)
        {
            List<AuthorBooksGenresDTO> listBooks = new();
            Genre genre = _mapper.Map<Genre>(genreDTO);

            var books = unitOfWork.BookRepository.Get(includeProperties: "Author,Genre")
                                                    .ToList()
                                                    .Where(e => e.Genres.Any(e => e.Id == genre.Id))
                                                    .ToList();
            foreach (var book in books)
            {
                listBooks.Add(new AuthorBooksGenresDTO
                {
                    Author = _mapper.Map<AuthorDTO>(book.Author),
                    Book = _mapper.Map<BookDTO>(book),
                    Genre = (List<GenreDTO>)_mapper.Map<IEnumerable<GenreDTO>>(book.Genres.Where(e => e.Id == genre.Id))
                });

            }

            return listBooks;
        }

    }
}
