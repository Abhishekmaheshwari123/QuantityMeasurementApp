using System.Threading;
using System.Threading.Tasks;
using QuantityMeasurement.ModelLayer.Entities;

namespace RepositoryLayer.Interfaces
{
    /// <summary>
    /// Data-access contract used by authentication services.
    /// </summary>
    public interface IUserAuthRepository
    {
        /// <summary>
        /// Checks whether a user already exists for the given normalized email.
        /// </summary>
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a user by normalized email, or null when not found.
        /// </summary>
        Task<UserRecord?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts a new user record.
        /// </summary>
        Task AddAsync(UserRecord user, CancellationToken cancellationToken = default);
    }
}
