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
    public class HumanDTOController : ControllerBase
    {
     
        private readonly ILogger<HumanDTOController> _logger;

        public HumanDTOController(ILogger<HumanDTOController> logger)
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
            return DataDTO.AllHuman;
        }


        /// <summary>
        /// 1.3.1.3 - Поиск людей, в имени, фамилии или отчестве которых содержится поисковая фраза
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("GetHuman")]
        public IEnumerable<HumanDTO> GetHuman([FromRoute] string filter)
        {

            return DataDTO.AllHuman.Where(e => e.Name.ToLower() == filter.ToLower()
                                            || e.Surname.ToLower() == filter.ToLower()
                                            || e.Patronymic.ToLower() == filter.ToLower()).ToList();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HumanDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var human = DataDTO.AllHuman.FirstOrDefault(e => e.Id == id);
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
        public ActionResult<HumanDTO> AddHumanDTO ([FromBody] HumanDTO human)
        {
            DataDTO.AllHuman.Add(human);
            return CreatedAtAction(nameof(GetById), new { id = human.Id }, human);
        }

        /// <summary>
        /// 1.3.3 - метод DELETE, удаляющий человека.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteHuman([FromRoute] int id)
        {
            var human = DataDTO.AllHuman.Where(e => e.Id == id).FirstOrDefault();

            if (human == null)
            {
                return NotFound();
            }

            DataDTO.AllHuman.Remove(human);

            return NoContent();
        }

    }
}
