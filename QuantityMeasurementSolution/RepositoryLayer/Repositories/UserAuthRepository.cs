using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuantityMeasurement.ModelLayer.Entities;
using QuantityMeasurement.RepositoryLayer.Data;
using RepositoryLayer.Interfaces;

namespace QuantityMeasurement.RepositoryLayer.Repositories
{
    /// <summary>
    /// Persists and reads API users through EF Core.
    /// </summary>
    public sealed class UserAuthRepository : IUserAuthRepository
    {
        private readonly QuantityMeasurementDbContext _dbContext;

        public UserAuthRepository(QuantityMeasurementDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            // Caller provides normalized email, so this query stays index-friendly.
            return _dbContext.Users.AnyAsync(x => x.Email == email, cancellationToken);
        }

        public Task<UserRecord?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            // Login flow uses this single-record lookup by unique email index.
            return _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        public async Task AddAsync(UserRecord user, CancellationToken cancellationToken = default)
        {
            // Persist user in a single unit-of-work call.
            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
