using AutoMapper;
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
        private readonly IMapper mapper;
        public HumanDtoController(ILogger<HumanDtoController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            _logger = logger;
            this.mapper = mapper;
        }

        /// <summary>
        /// 1.3.1.1 - метод Get, возвращающий список всех людей
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        public IEnumerable<HumanDto> GetAll()
        {
            return mapper.Map<List<HumanDto>>(unitOfWork.GetRepository<Person>().Get());
        }


        /// <summary>
        /// 1.3.1.3 - Поиск людей, в имени, фамилии или отчестве которых содержится поисковая фраза
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("getHuman")]
        public IEnumerable<HumanDto> GetHuman([FromRoute] string filter)
        {
            return mapper.Map<List<HumanDto>>(unitOfWork.GetRepository<Person>()
                                                     .Get(e => e.FirstName.ToLower() == filter.ToLower()
                                                    || e.LastName.ToLower() == filter.ToLower()
                                                    || e.MiddleName.ToLower() == filter.ToLower()));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HumanDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var human = mapper.Map<HumanDto>(unitOfWork.GetRepository<Person>().Get(e => e.Id == id).FirstOrDefault());
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
        public ActionResult<HumanDto> AddHumanDTO([FromBody] HumanDto human)
        {
            var person = mapper.Map<Person>(human);
            unitOfWork.GetRepository<Person>().Insert(person);
            return CreatedAtAction("AddHumanDTO", new { id = person.Id }, human);
        }

        /// <summary>
        /// 1.3.3 - метод DELETE, удаляющий человека.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteHuman([FromRoute] int id)
        {
            var person = unitOfWork.GetRepository<Person>().Get(e => e.Id == id).FirstOrDefault();

            if (person == null)
            {
                return NotFound();
            }
            unitOfWork.GetRepository<Person>().Delete(person);

            return NoContent();
        }

    }
}
