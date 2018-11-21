using System.Threading.Tasks;

namespace cars.Core
{
    public interface IUnitOfWork {
        Task CompleteAsync();
    }
}