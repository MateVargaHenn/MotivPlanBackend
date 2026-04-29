using MediatR;
using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Application.Abstractions.Messaging;

public interface ICommand : IRequest
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
