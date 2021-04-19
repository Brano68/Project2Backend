using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Visma.FamilyTree.DTO;
using Visma.FamilyTree.WebAPI.Managers.Interfaces;

namespace Visma.FamilyTree.WebAPI.Controllers
{
    [Route("api/person")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        public PersonController(
            IPersonManager personManager,
            ILogger<PersonController> logger)
        {
            PersonManager = personManager;
            Logger = logger;
        }

        public IPersonManager PersonManager { get; set; }

        public ILogger<PersonController> Logger { get; set; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonDTO>>> GetPersons()
        {
            Logger.LogInformation("Getting persons");

            var persons = await PersonManager.GetAllPersons().ConfigureAwait(false);

            Logger.LogInformation($"Number of Obtained persons: {persons.Count()}");

            return persons.Any()
                ? (ActionResult)Ok(persons)
                : NoContent();
        }

        [HttpGet]
        [Route("{personId}")]
        public async Task<ActionResult<IEnumerable<PersonDTO>>> GetPerson(
            [FromRoute] Guid personId)
        {
            var persons = await PersonManager.GetPerson(personId).ConfigureAwait(false);

            return persons != null
                ? (ActionResult)Ok(persons)
                : NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<PersonDTO>> PostPerson(
            [FromBody] PersonDTO personDTO)
        {
            return Ok(await PersonManager.AddPerson(personDTO).ConfigureAwait(false));
        }

        [HttpPut]
        [Route("{personId}")]
        public async Task<ActionResult<PersonDTO>> PutPerson(
            [FromRoute] Guid personId,
            [FromBody] PersonDTO personDTO)
        {
            var person = await PersonManager.UpdatePerson(personId, personDTO).ConfigureAwait(false);

            return person != null
                ? (ObjectResult)Ok(person)
                : BadRequest(new { ErrorMEssage = "Entity not found" });
        }

        [HttpPost]
        [Route("/api/exception")]
        public Task<ActionResult<PersonDTO>> GetException()
        {
            throw new Exception("Test Exception");
        }
    }
}
