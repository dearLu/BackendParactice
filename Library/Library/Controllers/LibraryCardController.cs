﻿using Library.Models;
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
        public IActionResult GetById(int id)
        {
            var card = DataDTO.cards.Where(e => e.Id == id).FirstOrDefault();
            if (card == null)
            {
                return NotFound();
            }

            return Ok(card);
        }


        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public  ActionResult<LibraryCard> AddLibraryCard(LibraryCard card)
        {
            if (card == null)
            {
                return BadRequest();
            }

            DataDTO.cards.Add(card);


            return  CreatedAtAction(nameof(GetById), new { id = card.Id }, card);
        }


    }
}
