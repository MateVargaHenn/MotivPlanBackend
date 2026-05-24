using Microsoft.EntityFrameworkCore;
using MotivPlanBackend.Application.Abstractions.Data;
using MotivPlanBackend.Domain.Entities;
using MotivPlanBackend.Domain.Enums;

namespace MotivPlanBackend.Application.Managers;

/// <summary>
/// Provides the APIs for managing profile in a persistence store.
/// </summary>
/// <typeparam name="TProfile">The type encapsulating a profile.</typeparam>
public class ProfileManager<TProfile>(IMotivPlanDbContext dbContext) : IDisposable where TProfile : ProfileEntity
{
    private readonly IMotivPlanDbContext _dbContext = dbContext;






    private bool _disposed;
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }


    /// <summary>
    /// Releases the unmanaged resources used by the role manager and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing && !_disposed)
        {
            _disposed = true;
        }
    }

    internal async Task CreateProfileAsync(string id, string firstName, string lastName, DateOnly birthDate, Sex sex, double weight, int height, ICollection<Preference> preferences)
    {
        var profile = new ProfileEntity
        {
            UserId = id,
            FirstName = firstName,
            LastName = lastName,
            BirthDate = birthDate,
            SexId = sex,
            Weight = weight,
            Height = height,
        };

        var preferencesEntity = await _dbContext.Preferences.AllAsync(x => preferences.Contains((Preference)x.Id));

        _dbContext.Profiles.Add(profile);
        // Here you would typically add the profile to a database or other persistence store.
        await _dbContext.SaveChangesAsync();
    }
}
