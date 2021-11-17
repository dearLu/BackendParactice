using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.Models;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly LibraryContext _context;

        public PersonController(LibraryContext context)
        {
            _context = context;
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
        /// 7.1.2.	Информация о пользователе может быть изменена (PUT) (вернуть пользователя)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        // PUT: api/Person/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Person>> PutPerson(int id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }

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

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        /// <summary>
        /// 7.1.1 - Пользователь может быть добавлен
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        // POST: api/Person
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        /// <summary>
        /// 7.1.3.	Пользователь может быть удалён по ID (DELETE) (ок или ошибку, если такого id нет)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Person/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// 7.1.4.	Пользователь или пользователи могут быть удалены по ФИО (не заботясь о том что могут быть полные 
        /// тёзки. Без пощады.) (DELETE) Ok - или ошибку, если что-то пошло не так. 
        /// </summary>
        /// <param name="LastName"></param>
        /// <returns></returns>
        [HttpDelete("DeletePersonByName")]
        public async Task<IActionResult> DeletePersonByName([FromRoute] string LastName)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(e => e.LastName == LastName);
            if (person == null)
            {
                return NotFound();
            }

            _context.Persons.Remove(person);

            await _context.SaveChangesAsync();

            return Ok();
        }

        // GET: api/Person
        //[HttpGet("GetBooks")]
        //public async Task<ActionResult<IEnumerable<Book>>> GetBooks(int id)
        //{
        //    return await _context.Persons.ToListAsync();
        //}

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
        private bool PersonExists(int id)
        {
            return _context.Persons.Any(e => e.Id == id);
        }
    }
}
