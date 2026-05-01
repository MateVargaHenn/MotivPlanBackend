using MediatR;
using MotivPlanBackend.Shared.Common;

namespace MotivPlanBackend.Application.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}