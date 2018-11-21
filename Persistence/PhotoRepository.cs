using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cars.Core;
using cars.Core.Models;

namespace cars.Persistence 
{
    public class PhotoRepository : IPhotoRepository 
    {

        private readonly CarsDbContext context;

        public PhotoRepository(CarsDbContext context) 
        {
            this.context = context;
        }


        public async Task<IEnumerable<Photo>> GetPhotos(int carId) 
        {
            return await context.Photos
                .Where (p => p.CarId == carId)
                .ToListAsync ();
        }
    }
}