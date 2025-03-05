using MediatR;
using Microsoft.AspNetCore.Mvc;
using SMSIntegration.Application.Features.Sms.Commands;

namespace SMSIntegration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SmsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendSms([FromBody] SendSmsCommand command)
        {
            var result = await _mediator.Send(command);
            if (result)
                return Ok(new { Message = "SMS sent successfully!" });

            return BadRequest(new { Message = "Failed to send SMS." });
        }
    }
}
