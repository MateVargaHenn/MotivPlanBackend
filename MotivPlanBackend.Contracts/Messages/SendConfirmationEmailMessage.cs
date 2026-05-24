namespace MotivPlanBackend.Contracts.Messages;

public sealed record SendConfirmationEmailMessage(
    string Id,
    string Email,
    string ConfirmationLink);
