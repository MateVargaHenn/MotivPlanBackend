using MediatR;
using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
