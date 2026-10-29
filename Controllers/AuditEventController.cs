using CyberArk_Idira_MockApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CyberArk_Idira_MockAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditEventController : ControllerBase
    {
        private readonly IAuditEventService _service;

        public AuditEventController(IAuditEventService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var events = await _service.GetAllAsync(cancellationToken);
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
        {
            var auditEvent = await _service.GetByIdAsync(id, cancellationToken);

            return auditEvent is null
                ? NotFound()
                : Ok(auditEvent);
        }

        [HttpGet("resource/{resourceType}/{resourceId}")]
        public async Task<IActionResult> GetByResource(string resourceType, string resourceId, CancellationToken cancellationToken)
        {
            var events = await _service.GetByResourceAsync(resourceType, resourceId, cancellationToken);
            return Ok(events);
        }

        [HttpGet("actor/{actorUserId}")]
        public async Task<IActionResult> GetByActor(string actorUserId, CancellationToken cancellationToken)
        {
            var events = await _service.GetByActorAsync(actorUserId, cancellationToken);
            return Ok(events);
        }
    }
}
