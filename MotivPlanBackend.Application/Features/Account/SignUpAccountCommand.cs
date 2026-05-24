using MotivPlanBackend.Application.Abstractions.Messaging;
using MotivPlanBackend.Contracts.DataTransferObjects.Account;

namespace MotivPlanBackend.Application.Features.Account;

public sealed record SignUpAccountCommand(SignUpAccountDto 
    SignUpAccountDto) : ICommand<string>
{
    public SignUpAccountDto SignUpAccountDto { get; private set; } = SignUpAccountDto;
}
