using System.Collections.Generic;
using System.Threading.Tasks;
using cars.Core.Models;

namespace cars.Core
{
    public interface ICarRepository
    {
         Task<Car> GetCar (int id, bool includeRelated = true);
         void Add(Car car);
         void Remove(Car car);
         Task<QueryResult<Car>> GetCars(CarQuery filter);
    }
}