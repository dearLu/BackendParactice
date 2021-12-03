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
    public class AuthorController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AuthorController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        /// <summary>
        /// 2.7.3.1.	Можно получить список всех авторов. (без книг, как и везде, где не указано обратное)
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAuthors")]
        public ActionResult<IEnumerable<Author>> GetAuthors()
        {
            return _unitOfWork.GetRepository<Author>().Get().ToList();
        }

        /// <summary>
        /// 2.7.3.2.	Можно получить список книг автора (книг может и не быть). автор + книги + жанры
        /// </summary>
        /// <param name="authorDTO"></param>
        /// <returns></returns>
        [HttpGet("getAuthorWithBooks")]
        public List<AuthorBooksGenresDto> GetAuthorWithBooks([FromBody] AuthorDto authorDTO)
        {
            List<AuthorBooksGenresDto> listBooks = new();

            Author author = _mapper.Map<Author>(authorDTO);
            var books = _unitOfWork.GetRepository<Book>().Get(e => e.AuthorId == author.Id,
                                                                                null, 
                                                                                includeProperties: "Author,Genre");
            foreach (var book in books)
            {
                listBooks.Add(new AuthorBooksGenresDto
                {
                    Author = _mapper.Map<AuthorDto>(book.Author),
                    Book = _mapper.Map<BookDto>(book),
                    Genre = _mapper.Map<List<GenreDto>>(book.Genres)
                });

            }

            return listBooks;
        }

        /// <summary>
        /// 2.7.3.3.	Добавить автора (с книгами или без) ответ - автор + книги
        /// </summary>
        /// <param name="authorDTO"></param>
        /// <returns></returns>

        [HttpPost]
        public AuthorBooksDto AddAuthor([FromBody] AuthorDto authorDTO) 
        {
            AuthorBooksDto obj = new();
            Author author = _mapper.Map<Author>(authorDTO);
            List<BookDto> books = _mapper.Map<List<BookDto>>(author.Books);

            _unitOfWork.GetRepository<Author>().Insert(author);
            _unitOfWork.Save();

            obj.Author = authorDTO;
            obj.Books = books;

            return obj;
        }

        /// <summary>
        /// 2.7.3.4.Удалить автора (если только нет книг, иначе кидать ошибку с пояснением, что нельзя удалить автора пока 
        /// есть его книги) - Ок или Ошибка.
        /// </summary>
        /// <param name="authorDTO"></param>
        /// <returns></returns>
        [HttpDelete("deleteAuthor")]
        public IActionResult DeleteAuthor([FromBody] AuthorDto authorDTO)
        {
            Author author = _mapper.Map<Author>(authorDTO);

            var books = _unitOfWork.GetRepository<Book>().Get(e => e.AuthorId == author.Id,
                                                                                null,
                                                                                includeProperties: "Author");

            if (books.Count() > 0)
            {
                return BadRequest($"У автора {author.LastName} есть книги. Нельзя удалить автора, пока есть его книги");
            }
            else 
            {
                _unitOfWork.GetRepository<Author>().Delete(author.Id);
                _unitOfWork.Save();
            }

            return Ok();
        }
    }
}
