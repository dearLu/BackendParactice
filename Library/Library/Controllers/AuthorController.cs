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
    public class AuthorController : ControllerBase
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private readonly IMapper _mapper;
        public AuthorController(IMapper mapper)
        {            
            _mapper = mapper;
        }

        /// <summary>
        /// 2.7.3.1.	Можно получить список всех авторов. (без книг, как и везде, где не указано обратное)
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAuthors")]
        public ActionResult<IEnumerable<Author>> GetAuthors()
        {
            return unitOfWork.AuthorRepository.Get().ToList();
        }

        /// <summary>
        /// 2.7.3.2.	Можно получить список книг автора (книг может и не быть). автор + книги + жанры
        /// </summary>
        /// <param name="authorDTO"></param>
        /// <returns></returns>
        [HttpGet("GetAuthorWithBooks")]
        public List<AuthorBooksGenresDTO> GetAuthorWithBooks([FromBody] AuthorDTO authorDTO)
        {
            List<AuthorBooksGenresDTO> listBooks = new();
            Author author = _mapper.Map<Author>(authorDTO);
            var books = unitOfWork.BookRepository.Get(includeProperties: "Author,Genre")
                                                    .ToList()
                                                    .Where(e=>e.AuthorId == author.Id)
                                                    .ToList();
            foreach (var book in books)
            {
                listBooks.Add(new AuthorBooksGenresDTO
                {
                    Author = _mapper.Map<AuthorDTO>(book.Author),
                    Book = _mapper.Map<BookDTO>(book),
                    Genre = (List<GenreDTO>)_mapper.Map<IEnumerable<GenreDTO>>(book.Genres)
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
        public AuthorBooksDTO AddAuthor([FromBody] AuthorDTO authorDTO) 
        {
            AuthorBooksDTO obj = new();
            Author author = _mapper.Map<Author>(authorDTO);
            List<BookDTO> books = (List<BookDTO>)_mapper.Map<IEnumerable<BookDTO>>(author.Books);

            unitOfWork.AuthorRepository.Insert(author);                        
            unitOfWork.Save();

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
        [HttpDelete("DeleteAuthor")]
        public IActionResult DeleteAuthor([FromBody] AuthorDTO authorDTO)
        {
            Author author = _mapper.Map<Author>(authorDTO);

            var books = unitOfWork.BookRepository.Get(includeProperties: "Author")
                                                    .ToList()
                                                    .Where(e => e.AuthorId == author.Id)
                                                    .ToList();
            if (books.Count() > 0)
            {
                return Content($"У автора {author.LastName} есть книги. Нельзя удалить автора, пока есть его книги");
            }
            else 
            {
                unitOfWork.AuthorRepository.Delete(author.Id);
                unitOfWork.Save();
            }

            return Ok();
        }
    }
}
