using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotivPlanBackend.Application.CommandQueryResponsibilitySegregation.Workout;
using MotivPlanBackend.Shared.Common;
using System.Diagnostics.CodeAnalysis;

namespace MotivPlanBackend.WebApi.Controllers;

[SuppressMessage("Minor Code Smell", "CA1515:Because an application's API isn't typically referenced from outside the assembly, types can be made internal",
    Justification = "ASP.NET Core controller discovery and Swagger require public controllers.")]
[Route("api/[controller]")]
[ApiController]
public class WorkoutsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("today")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWorkoutToday()
    {
        var query = new GetWorkoutTodayQuery();
        var result = await _mediator.Send(query);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return result.Error.Type switch
        {
            ErrorType.NotFound => NotFound(result),
            _ => BadRequest()
        };
    }
}
