using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WoL.Data
{
    // see https://github.com/dotnet/efcore/issues/26630#issuecomment-1014837147
    internal class CovariantDbContextFactory<TContextOut, TContextIn>
        : IDbContextFactory<TContextOut>
        where TContextOut : DbContext
        where TContextIn : TContextOut
    {
        private readonly IDbContextFactory<TContextIn> _contextInFactory;

        public CovariantDbContextFactory(IDbContextFactory<TContextIn> contextInFactory)
        {
            _contextInFactory = contextInFactory;
        }

        public TContextOut CreateDbContext()
            => _contextInFactory.CreateDbContext();

        public async Task<TContextOut> CreateDbContextAsync(CancellationToken cancellationToken = default)
            => await _contextInFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
    }
}
