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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public HumanDtoController(IMapper mapper,IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// 1.3.1.1 - метод Get, возвращающий список всех людей
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        public List<HumanDto> GetAll()
        {
            return _mapper.Map<List<HumanDto>>(_unitOfWork.GetRepository<Person>().Get());
        }


        /// <summary>
        /// 1.3.1.3 - Поиск людей, в имени, фамилии или отчестве которых содержится поисковая фраза
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("getHuman")]
        public List<HumanDto> GetHuman([FromRoute] string filter)
        {
            return _mapper.Map<List<HumanDto>>(_unitOfWork.GetRepository<Person>()
                                                     .Get(e => e.FirstName.ToLower() == filter.ToLower()
                                                    || e.LastName.ToLower() == filter.ToLower()
                                                    || e.MiddleName.ToLower() == filter.ToLower()));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HumanDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var human = _mapper.Map<HumanDto>(_unitOfWork.GetRepository<Person>().Get(e => e.Id == id).FirstOrDefault());
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
            var person = _mapper.Map<Person>(human);
            _unitOfWork.GetRepository<Person>().Insert(person);
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
            var person = _unitOfWork.GetRepository<Person>().Get(e => e.Id == id).FirstOrDefault();

            if (person == null)
            {
                return NotFound();
            }
            _unitOfWork.GetRepository<Person>().Delete(person);

            return NoContent();
        }

    }
}
