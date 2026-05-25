using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MotivPlanBackend.Application.Features.Account;
using MotivPlanBackend.Contracts.DataTransferObjects.Account;
using MotivPlanBackend.Domain.Enums;

namespace MotivPlanBackend.Test.IntegrationTests;

public sealed class SignUpAccountCommandHandlerTest
{
    [Fact]
    public async Task HandleShouldReturnTrueWhenPasswordIsInvalid()
    {
        // Arrange
        using var provider = IntegrationTestFactory.Create();
        using var scope = provider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<SignUpAccountCommandHandler>();
        ICollection<Preference> preferences = new List<Preference> { Preference.Visual, Preference.Spoken };
        var dto = new SignUpAccountDto("testuser", "test@example.com", "testp@ssword", "testp@ssword", "Test", "User", DateOnly.FromDateTime(DateTime.Now.AddYears(-14)), Sex.Male, 70, 180, preferences);
        // Act
        var command = new SignUpAccountCommand(dto);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Error.Obj.Should().Contain("PasswordRequiresUpper").And.Contain("PasswordRequiresDigit");
    }

    [Fact]
    public async Task HandleShouldReturnTrueWhenPasswordMismatch()
    {
        // Arrange
        using var provider = IntegrationTestFactory.Create();
        using var scope = provider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<SignUpAccountCommandHandler>();
        ICollection<Preference> preferences = new List<Preference> { Preference.Visual, Preference.Spoken };
        var dto = new SignUpAccountDto("testuser", "test@example.com", "Testp@ssword8", "differentpassword", "Test", "User", DateOnly.FromDateTime(DateTime.Now.AddYears(-14)), Sex.Male, 70, 180, preferences);
        // Act
        var command = new SignUpAccountCommand(dto);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Error.Obj.Should().Be("Auth.PasswordMismatch");
    }

}
