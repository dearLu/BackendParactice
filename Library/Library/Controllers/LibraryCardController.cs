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
    /// 2.1.2 - Создать третий контроллер, отвечающий за получение книги человеком
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryCardController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;

        public LibraryCardController(ILogger<BookController> logger)
        {
            _logger = logger;

        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LibraryCard))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById([FromRoute] int id)
        {
            var card = DataDTO.Cards.Where(e => e.Id == id).FirstOrDefault();
            if (card == null)
            {
                return NotFound();
            }

            return Ok(card);
        }

        /// <summary>
        /// 2.1.4 - метод POST отвечающий за взятие книги читателем. На вход - вышеописанный объект
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public  ActionResult<LibraryCard> AddLibraryCard([FromBody] LibraryCard card)
        {
            if (card == null)
            {
                return BadRequest();
            }

            DataDTO.Cards.Add(card);

            return  CreatedAtAction(nameof(GetById), new { id = card.Id }, card);
        }

    }
}
