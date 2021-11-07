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
    /// 1.3 - Контроллер, который отвечает за человека
    /// </summary>
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class HumanController : ControllerBase
    {
     
        private readonly ILogger<HumanController> _logger;

        public HumanController(ILogger<HumanController> logger)
        {
            _logger = logger;

        }

        /// <summary>
        /// 1.3.1.1 - метод Get, возвращающий список всех людей
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public IEnumerable<HumanDTO> GetAll()
        {

            return DataDTO.allHuman;
        }

        /// <summary>
        /// 1.3.1.2 - метод Get, возвращающий список людей, которые пишут книги
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAuthor")]
        public IEnumerable<HumanDTO> GetAuthor()
        {

            return DataDTO.allBook.Select(e=>e.Author).Distinct().ToList();
        }

        /// <summary>
        /// 1.3.1.3 - Поиск людей, в имени, фамилии или отчестве которых содержится поисковая фраза
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("GetHuman")]
        public IEnumerable<HumanDTO> GetHuman(string filter)
        {

            return DataDTO.allHuman.Where(e => e.Name.ToLower() == filter
                                            || e.Surname.ToLower() == filter
                                            || e.Patronymic.ToLower() == filter).ToList();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HumanDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var human = DataDTO.allHuman.Where(e => e.Id == id).FirstOrDefault();
            if (human == null)
            {
                return NotFound();
            }

            return Ok(human);
        }

        /// <summary>
        ///  1.3.2 -  метод POST добавляющий нового человека
        /// </summary>
        /// <param name="human"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<HumanDTO> AddHumanHumanDTO (HumanDTO human)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            DataDTO.allHuman.Add(human);


            return CreatedAtAction(nameof(GetById), new { id = human.Id }, human);
        }

        /// <summary>
        /// 1.3.3 - метод DELETE, удаляющий человека.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteHuman(int id)
        {
            var human = DataDTO.allHuman.Where(e => e.Id == id).FirstOrDefault();

            if (human == null)
            {
                return NotFound();
            }

            DataDTO.allHuman.Remove(human);

            return NoContent();
        }

    }
}
