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
    public class PersonController : ControllerBase
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private readonly IMapper _mapper;
        public PersonController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// 2.7.1.1 - Пользователь может быть добавлен
        /// </summary>
        /// <param name="human"></param>
        /// <returns></returns>
        // POST: api/Person
        [HttpPost]
        public ActionResult<Person> AddPerson([FromBody] HumanDTO human)
        {
            Person person = _mapper.Map<Person>(human);
            unitOfWork.PersonRepository.Insert(person);
            unitOfWork.Save();
            return CreatedAtAction("AddPerson", new { id = person.Id }, person);
        }

        /// <summary>
        /// 2.7.1.2.	Информация о пользователе может быть изменена (PUT) (вернуть пользователя)
        /// </summary>
        /// <param name="human"></param>
        /// <returns></returns>
        // PUT: api/Person/5
        [HttpPut]
        public ActionResult<Person> UpdatePerson([FromBody] HumanDTO human)
        {
            Person person = _mapper.Map<Person>(human);
            try
            {
                unitOfWork.PersonRepository.Update(person);

            }
            catch (Exception ex)
            {
                return NotFound();

            }

            unitOfWork.Save();

            return CreatedAtAction("UpdatePerson", new { id = person.Id }, person);
        }



        /// <summary>
        /// 2.7.1.3. Пользователь может быть удалён по ID (DELETE) (ок или ошибку, если такого id нет)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Person/5
        [HttpDelete("{id}")]
        public IActionResult DeletePerson([FromRoute] int id)
        {

            var human = DataDTO.AllHuman.FirstOrDefault(e => e.Id == id);

            if (human == null)
            {
                return NotFound();
            }

            Person person = _mapper.Map<Person>(human);

            if (person == null)
            {
                return NotFound();
            }

            unitOfWork.PersonRepository.Delete(person.Id);
            unitOfWork.Save();

            return Ok();
        }

        /// <summary>
        /// 2.7.1.4. Пользователь или пользователи могут быть удалены по ФИО (не заботясь о том что могут быть полные 
        /// тёзки. Без пощады.) (DELETE) Ok - или ошибку, если что-то пошло не так. 
        /// </summary>
        /// <param name="human"></param>
        /// <returns></returns>
        [HttpDelete("DeletePersonByName")]
        public IActionResult DeletePersonByName([FromBody] HumanDTO human )
        {
            if (human == null)
            {
                return NotFound();
            }

            Person person = _mapper.Map<Person>(human);

            var listPersons = unitOfWork.PersonRepository.Get().ToList().Where(e => e.FirstName == person.FirstName
                                            && e.LastName == person.LastName
                                            && e.MiddleName == person.MiddleName)
                                            .ToList();

            if (listPersons.Count() > 0)
            {
                foreach (var p in listPersons)
                {
                    unitOfWork.PersonRepository.Delete(p.Id);
                }

                unitOfWork.Save();
            }
            else 
            {
                return NotFound();
            }

            return Ok();
        }


        /// <summary>
        /// 2.7.1.5. Получить список всех взятых пользователем книг (GET) в качестве параметра поиска - ID пользователя.
        /// Полное дерево: Книги - автор - жанр
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetListBook")]
        public List<AuthorBooksGenresDTO> GetListBook([FromRoute] int Id) 
        {
            List<AuthorBooksGenresDTO> listBooks = new();
            var human = DataDTO.AllHuman.FirstOrDefault(e => e.Id == Id);         

            Person person = _mapper.Map<Person>(human);

            var books = unitOfWork.BookRepository.Get(includeProperties: "Author,Genre")
                                                            .ToList()
                                                            .Where(e => e.Persons.Any(q=>q.Id == person.Id))
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
        /// 2.7.1.6.	Пользователь может взять книгу (добавить в список книг пользователя книгу)  Пользователь + книги
        /// </summary>
        /// <param name="id"></param>
        /// <param name="book"></param>
        /// <returns></returns>

        [HttpPut("PutBookForPerson")]
        public PersonBooks PutBookForPerson(int id, BookDTO bookDTO)
        {

            var human = DataDTO.AllHuman.FirstOrDefault(e => e.Id == id);

            Person person = _mapper.Map<Person>(human);
            Book book = _mapper.Map<Book>(bookDTO);

            person.Books.Add(book);
            unitOfWork.PersonRepository.Update(person);
            unitOfWork.Save();

            PersonBooks obj = new();
            obj.Human = _mapper.Map<HumanDTO>(person);
            obj.Books = (List<BookDTO>)_mapper.Map<IEnumerable<BookDTO>>(person.Books);
            return obj;
        }

        /// <summary>
        /// 2.7.1.7.	Пользователь может вернуть книгу (удалить из списка книг пользователя книгу) пользователь + книги
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bookDTO"></param>
        /// <returns></returns>
        [HttpDelete("ReturnBook")]
        public PersonBooks ReturnBook( int id, BookDTO bookDTO)
        {
            var human = DataDTO.AllHuman.FirstOrDefault(e => e.Id == id);

            Person person = _mapper.Map<Person>(human);
            Book book = _mapper.Map<Book>(bookDTO);

            person.Books.Remove(book);
            unitOfWork.PersonRepository.Update(person);
            unitOfWork.Save();

            PersonBooks obj = new();
            obj.Human = _mapper.Map<HumanDTO>(person);
            obj.Books = (List<BookDTO>)_mapper.Map<IEnumerable<BookDTO>>(person.Books);
            return obj;
        }


        
    }
}
