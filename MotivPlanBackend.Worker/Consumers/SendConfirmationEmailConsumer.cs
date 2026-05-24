using MassTransit;
using MotivPlanBackend.Contracts.Messages;

namespace MotivPlanBackend.Worker.Consumers;

internal sealed class SendConfirmationEmailConsumer
    : IConsumer<SendConfirmationEmailMessage>
{
    public async Task Consume(
        ConsumeContext<SendConfirmationEmailMessage> context)
    {
        var message = context.Message;

        Console.WriteLine($"Email sent to {message.Email}");

        await Task.CompletedTask;
    }
}