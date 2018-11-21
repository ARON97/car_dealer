using System.Collections.Generic;
using System.Threading.Tasks;
using cars.Core.Models;

namespace cars.Core
{
    public interface IPhotoRepository
    {
        Task<IEnumerable<Photo>> GetPhotos(int carId);
    }
}