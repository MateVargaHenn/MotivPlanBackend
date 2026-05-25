using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using MotivPlanBackend.Application.Abstractions.Messaging;
using MotivPlanBackend.Contracts.Messages;
using MotivPlanBackend.Application.Managers;
using MotivPlanBackend.Domain.Entities;
using MotivPlanBackend.Shared.Common;
using System.Text;

namespace MotivPlanBackend.Application.Features.Account;

public sealed class SignUpAccountCommandHandler(
    UserManager<IdentityUser> userManager,
    ProfileManager<ProfileEntity> profileManager,
    IPublishEndpoint publishEndpoint)
    : ICommandHandler<SignUpAccountCommand, string>
{
    private readonly string frontendBaseUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? String.Empty;
    public async Task<Result<string>> Handle(
        SignUpAccountCommand request,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return Result.Failure<string>(
                Error.Problem("Auth.InvalidRequest"));
        }

        if (!request.SignUpAccountDto.Password.Equals(request.SignUpAccountDto.ConfirmPassword, StringComparison.OrdinalIgnoreCase))
        {
            return Result.Failure<string>(
                Error.Problem("Auth.PasswordMismatch"));
        }

        var user = new IdentityUser
        {
            UserName = request.SignUpAccountDto.Username,
            Email = request.SignUpAccountDto.Email,
            
        };

        var userSignUpResult = await userManager.CreateAsync(user, request.SignUpAccountDto.Password);
        
        if (!userSignUpResult.Succeeded)
        {
            var errors = userSignUpResult.Errors.ToList();

            var duplicateEmail = errors.FirstOrDefault(e => e.Code == "DuplicateEmail");
            if (duplicateEmail is not null)
                return Result.Failure<string>(Error.Conflict(duplicateEmail.Code));

            var duplicateUserName = errors.FirstOrDefault(e => e.Code == "DuplicateUserName");
            if (duplicateUserName is not null)
                return Result.Failure<string>(Error.Conflict(duplicateUserName.Code));


            return Result.Failure<string>(Error.Problem(string.Join(", ", errors.Select(e => e.Code))));
        }

        await userManager.AddToRoleAsync(user, "User");

        await profileManager.CreateProfileAsync(
            user.Id,
            request.SignUpAccountDto.FirstName,
            request.SignUpAccountDto.LastName,
            request.SignUpAccountDto.BirthDate,
            request.SignUpAccountDto.Sex,
            request.SignUpAccountDto.Weight,
            request.SignUpAccountDto.Height,
            request.SignUpAccountDto.Preferences
            );


        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(
            Encoding.UTF8.GetBytes(token));

        var confirmationLink =
    $"{frontendBaseUrl}/confirm-email" +
    $"?userId={Uri.EscapeDataString(user.Id)}" +
    $"&token={Uri.EscapeDataString(encodedToken)}";

        await publishEndpoint.Publish(
    new SendConfirmationEmailMessage(
        user.Id,
        user.Email!,
        confirmationLink),
    cancellationToken);

        if (!userSignUpResult.Succeeded)
        {
            var error = userSignUpResult.Errors.First();

            return Result.Failure<string>(
                Error.Failure(error.Code));
        }

        return Result.Success(user.Id);
    }
}
