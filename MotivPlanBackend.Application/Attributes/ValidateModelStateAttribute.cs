using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;

namespace MotivPlanBackend.Application.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class ValidateModelStateAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context is null || context.ModelState.IsValid ||
            context.ModelState.Values.Any(value =>
            value.Errors.Any(error => error.Exception != null)
            ))
        {
            base.OnActionExecuting(context!);
            return;
        }

        var errors = context.ModelState
            .Where(ms => ms.Value!.Errors.Count > 0)
            .Select(ms => new
            {
                Field = ms.Key,
                Errors = ms.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            })
            .ToArray();

        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ValidateModelStateAttribute>>();
        var validationErrors = string.Join($",{Environment.NewLine} ", errors);


        context.Result = new JsonResult(errors)
        {
            StatusCode = (int)HttpStatusCode.BadRequest
        };
    }
}
