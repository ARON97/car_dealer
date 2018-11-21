using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using cars.Controllers.Resources;
using cars.Core.Models;
using cars.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cars.Controllers {
    public class MakesController : Controller {

        // This field is automatically created when the cursor is on context in the constructor
        // By using CTRL + . and selecting initialize field from parameter and initializes the argument in the constructor 
        private readonly CarsDbContext context;
        
        // initialize field from parameter selected when using CTRL + . when the cursor is on mapper
        private readonly IMapper mapper;
        // defining a constructor
        public MakesController (CarsDbContext context, IMapper mapper) {
            this.mapper = mapper;
            this.context = context;

        }
        // explicitly apply an attribute and specify what endpoint this action is for
        [HttpGet ("/api/makes")]
        // Defining an Action
        // return the list of Makes 
        public async Task<IEnumerable<MakeResource>> GetMakes () {
            // return the list of Makes and their Models from the database
            // await this when using ToListAsync() and add async and task in the method name
            var makes =  await context.Makes.Include (m => m.Models).ToListAsync ();

            // map to the API resources
            return mapper.Map<List<Make>, List<MakeResource>>(makes);
        }
    }
}