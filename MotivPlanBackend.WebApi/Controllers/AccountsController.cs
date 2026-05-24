using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotivPlanBackend.Application.Features.Account;

namespace MotivPlanBackend.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] SignUpAccountCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok();
        }

        return BadRequest(result.Error);
    }
}
