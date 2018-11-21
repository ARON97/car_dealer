using System.Threading.Tasks;
using cars.Core;

namespace cars.Persistence
{
    // implement the interface
    public class UnitOfWork : IUnitOfWork 
    {
        // contructor
        private readonly CarsDbContext context;
        public UnitOfWork(CarsDbContext context) 
        {
            this.context = context;

        }
        public async Task CompleteAsync() {
            // delegate this context
            await context.SaveChangesAsync();
        }
    }
}