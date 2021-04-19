using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Visma.FamilyTree.DTO;
using Visma.FamilyTree.WebAPI.Managers.Interfaces;

namespace Visma.FamilyTree.WebAPI.Controllers
{
    [Route("api/child")]
    [ApiController]
    public class ChildController : ControllerBase
    {

        private readonly ILogger<ChildController> _logger;

        public ChildController(
            IChildManager childManager,
            ILogger<ChildController> logger)
        {
            ChildManager = childManager;
            Logger = logger;
            _logger = logger;
        }

        private IChildManager ChildManager { get; }

        private ILogger<ChildController> Logger { get; }

        [HttpPost]
        [Route("/api/person/{personId}/child")]
        public async Task<ActionResult<ChildDTO>> PostChild(
            [FromRoute] Guid personId,
            [FromBody] ChildDTO childDTO)
        {
            var child = await ChildManager.AddChild(personId, childDTO).ConfigureAwait(false);

            return child != null
                ? (ObjectResult)Ok(child)
                : BadRequest(new { ErrorMEssage = "Entity not found" });
        }

        [HttpPut]
        [Route("/api/person/{personId}/child/{childId}")]
        public async Task<ActionResult<ChildDTO>> PutChild(
            [FromRoute] Guid personId,
            [FromRoute] Guid childId,
            [FromBody] ChildDTO childDTO)
        {
            var child = await ChildManager.UpdateChild(personId, childId, childDTO).ConfigureAwait(false);

            return child != null
                ? (ObjectResult)Ok(child)
                : BadRequest(new { ErrorMEssage = "Entity not found" });
        }
    }
}
