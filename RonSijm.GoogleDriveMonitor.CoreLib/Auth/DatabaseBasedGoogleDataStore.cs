using RonSijm.GoogleDriveMonitor.DAL.Entities;

namespace RonSijm.GoogleDriveMonitor.CoreLib.Auth;

// ReSharper disable once ClassNeverInstantiated.Global - Justification: Used by DI
/// <inheritdoc />
/// <summary>
/// This implements the <see cref="T:Google.Apis.Util.Store.IDataStore" /> from Google, and saves the token in the sqlite database,
/// Instead of on disk
/// </summary>
/// <param name="dataContext"></param>
public class DatabaseBasedGoogleDataStore(LocalDataContext dataContext) : IDataStore
{
    public async Task StoreAsync<T>(string key, T value)
    {
        if (key == GoogleAuthConfig.GoogleUserKey)
        {
            if (value is not TokenResponse typedValue)
            {
                throw new NotImplementedException();
            }

            var entity = new TokenResponseEntity
            {
                AccessToken = typedValue.AccessToken,
                ExpiresInSeconds = typedValue.ExpiresInSeconds,
                IdToken = typedValue.IdToken,
                IssuedUtc = typedValue.IssuedUtc,
                RefreshToken = typedValue.RefreshToken,
                Scope = typedValue.Scope,
                TokenType = typedValue.TokenType
            };

            dataContext.TokenResponses.Add(entity);
            await dataContext.SaveChangesAsync();
        }
    }

    public Task DeleteAsync<T>(string key)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetAsync<T>(string key)
    {
        if (key != GoogleAuthConfig.GoogleUserKey)
        {
            throw new NotImplementedException();
        }

        var user = dataContext.TokenResponses.OrderBy(x => x.IssuedUtc).LastOrDefault();

        if (user == null)
        {
            return Task.FromResult<T>(default);
        }

        object response = new TokenResponse
        {
            AccessToken = user.AccessToken,
            ExpiresInSeconds = user.ExpiresInSeconds,
            IdToken = user.IdToken,
            IssuedUtc = user.IssuedUtc,
            RefreshToken = user.RefreshToken,
            Scope = user.Scope,
            TokenType = user.TokenType
        };

        return Task.FromResult((T)response);

    }

    public Task ClearAsync()
    {
        throw new NotImplementedException();
    }
}