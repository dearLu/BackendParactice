using Library.Interfaces;
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
    public class HumanDtoController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<HumanDtoController> _logger;

        public HumanDtoController(ILogger<HumanDtoController> logger, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// 1.3.1.1 - метод Get, возвращающий список всех людей
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        public IEnumerable<HumanDto> GetAll()
        {
            return DataDTO.AllHuman;
        }


        /// <summary>
        /// 1.3.1.3 - Поиск людей, в имени, фамилии или отчестве которых содержится поисковая фраза
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("getHuman")]
        public IEnumerable<HumanDto> GetHuman([FromRoute] string filter)
        {

            return DataDTO.AllHuman.Where(e => e.Name.ToLower() == filter.ToLower()
                                            || e.Surname.ToLower() == filter.ToLower()
                                            || e.Patronymic.ToLower() == filter.ToLower()).ToList();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HumanDto))]
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
        public ActionResult<HumanDto> AddHumanDTO ([FromBody] HumanDto human)
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
