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
using Library.Interfaces;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IRepository<Person> repo;
        private readonly IMapper _mapper;
        public PersonController(IMapper mapper,)
        {
            repo = new PersonRepository();
            _mapper = mapper;

        }

        // GET: api/Person
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            return await _context.Persons.ToListAsync();
        }

        // GET: api/Person/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = await _context.Persons.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        /// <summary>
        ///  7.1.2.	Информация о пользователе может быть изменена (PUT) (вернуть пользователя)
        /// </summary>
        /// <param name="human"></param>
        /// <returns></returns>
        // PUT: api/Person/5
        [HttpPut]
        public async Task<ActionResult<Person>> PutPerson(HumanDTO human)
        {
            Person person = _mapper.Map<Person>(human);
            repo.Update(person);

            try
            {
                 repo.Update(person);
                
            }
            catch (Exception ex)
            {
                return NotFound();

            }

            repo.Save();

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        /// <summary>
        /// 7.1.1 - Пользователь может быть добавлен
        /// </summary>
        /// <param name="human"></param>
        /// <returns></returns>
        // POST: api/Person
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson([FromBody] HumanDTO human)
        {
            Person person = _mapper.Map<Person>(human);
            repo.Create(person);
            repo.Save();
            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        /// <summary>
        /// 7.1.3.	Пользователь может быть удалён по ID (DELETE) (ок или ошибку, если такого id нет)
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
            
            repo.Delete(person.Id);
            repo.Save();

            return Ok();
        }

        /// <summary>
        /// 7.1.4.	Пользователь или пользователи могут быть удалены по ФИО (не заботясь о том что могут быть полные 
        /// тёзки. Без пощады.) (DELETE) Ok - или ошибку, если что-то пошло не так. 
        /// </summary>
        /// <param name="human"></param>
        /// <returns></returns>
        [HttpDelete("DeletePersonByName")]
        public IActionResult DeletePersonByName(HumanDTO human )
        {
            var listPersons = repo.GetList().ToList().Where(e => e.FirstName == human.Name
                                            && e.LastName == human.Surname
                                            && e.MiddleName == human.Patronymic)
                                            .ToList();

            if (listPersons.Count() > 0)
            {
                foreach (var person in listPersons)
                {
                    repo.Delete(person.Id);
                }

                repo.Save();
            }
            else 
            {
                return NotFound();
            }

            return Ok();
        }


        /// <summary>
        /// 7.1.6.	Пользователь может взять книгу (добавить в список книг пользователя книгу)  Пользователь + книги
        /// </summary>
        /// <param name="id"></param>
        /// <param name="book"></param>
        /// <returns></returns>

        [HttpPut("PutBookForPerson")]
        public async Task<IActionResult> PutBookForPerson( int id, Book book)
        {
            if (!PersonExists(id))
            {
                return BadRequest();
            }

            Person person = await _context.Persons.FindAsync(id);           

            person.Books.Add(book);

            _context.Entry(person).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        [HttpDelete("ReturnBook")]
        public async Task<IActionResult> ReturnBook( int id, Book book)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(e => e.Id == id);
            if (person == null)
            {
                return NotFound();
            }
            if (person.Books.Where(e => e.Id==book.Id).Count() == 0)
            {
                return NotFound();
            }    

            person.Books.Remove(book);

            _context.Entry(person).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok();
        }


        
    }
}
