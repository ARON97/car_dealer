using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using cars.Core;
using cars.Core.Models;
using cars.Extensions;
using Microsoft.EntityFrameworkCore;

namespace cars.Persistence 
{
    public class CarRepository : ICarRepository
    {
        private readonly CarsDbContext context;
        public CarRepository (CarsDbContext context) {
            this.context = context;

        }
        public async Task<Car> GetCar (int id, bool includeRelated = true)
        {
            // Loading only the ID
            if (!includeRelated)
                return await context.Cars.FindAsync(id);

            // Loading the complete Car Obhect
            return await context.Cars
                .Include (c => c.Features)
                    .ThenInclude (cf => cf.Feature)
                .Include (c => c.Model)
                    .ThenInclude (m => m.Make)
                .SingleOrDefaultAsync (c => c.Id == id);
        }

        // public void Task<Car> GetCarWithMake(int id) 
        // {
        //     // telling the repository explicity what we want to load

        // }

        // Add an object to the dbcontext
        public void Add(Car car)
        {
            // encapsulating the add to dbcontext
            context.Cars.Add(car);
        }

        // Removing an object from dbcontext
        public void Remove(Car car)
        {
            // removing it from the context
            context.Remove(car);
        }

        public async Task<QueryResult<Car>> GetCars(CarQuery queryObj)
        {
            // new result object
            var result = new QueryResult<Car>();

            var query = context.Cars  
                .Include(c => c.Model)
                    .ThenInclude(m => m.Make)
                .Include(c => c.Features)
                    .ThenInclude(cf => cf.Feature)
                .AsQueryable();
            
            // fitler with MakeId
            if (queryObj.MakeId.HasValue)
                query = query.Where(c => c.Model.MakeId == queryObj.MakeId.Value);

            // filter with ModelId
            if (queryObj.ModelId.HasValue)
                query = query.Where(c => c.ModelId == queryObj.MakeId.Value);

            // Sorting
            // mapping the string and expression
            var columnsMap = new Dictionary<string, Expression<Func<Car, object>>>() {
                // using the object initialization syntax to add KeyValuePairs to the dictionary 
                ["make"]          = c => c.Model.Make.Name,
                ["model"]         = c => c.Model.Name,
                ["contactName"]   = c => c.ContactName
            };

            // the extension method
            query = query.ApplyOrdering(queryObj, columnsMap);
           
            // calculate the total for the pagination
            result.TotalItems = await query.CountAsync();
            // pagination
            query = query.ApplyPaging(queryObj);
            
            result.Items = await query.ToListAsync();

            return result;
        }
    }
}