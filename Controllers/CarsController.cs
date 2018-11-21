using System;
using System.Threading.Tasks;
using AutoMapper;
using cars.Controllers.Resources;
using cars.Core.Models;
using cars.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace cars.Controllers {
    [Route("/api/cars")] // this will be applied to all the actions in this controller
    public class CarsController : Controller {

        private readonly IMapper mapper;
        private readonly ICarRepository repository;
        private readonly IUnitOfWork unitOfWork;

        // Constructor
        public CarsController(IMapper mapper, ICarRepository repository, IUnitOfWork unitOfWork) {
            this.unitOfWork = unitOfWork;
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCar([FromBody] SaveCarResource carResource) {
            
            // Validations- check if not ModelState is valid
            // if (!ModelState.IsValid)
            //     return BadRequest(ModelState);

            // Map API resource to Domain Object
            var car = mapper.Map<SaveCarResource, Car>(carResource);
            // setting the last update property
            car.LastUpdate = DateTime.Now;
            // Add the Car object into the dbcontext
            repository.Add(car);
            // saving the changes
            await unitOfWork.CompleteAsync();
            // fetch a complete representation of a Car
            car = await repository.GetCar(car.Id);

            // map the result back to a CarResource
            var result = mapper.Map<Car, CarResource>(car);
            // return Ok (carResource); // Ok() inherited from the base controller class. Will return it with HTTP Status 200 
            return Ok(result);
        }
        // Updating a car
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] SaveCarResource savecarResource) {
            // Validations- check if not ModelState is valid
            if (!ModelState.IsValid)
                // if it is invalid we return BadRequest
                return BadRequest(ModelState);

            var car = await repository.GetCar(id);

            // if car is null instead of letting the application crash
            if (car == null)
                // we return a proper response to the user
                return NotFound();

            // Map API resource to Domain Object
            mapper.Map<SaveCarResource, Car>(savecarResource, car);
            // setting the last update property
            car.LastUpdate = DateTime.Now;

            // saving the changes
            await unitOfWork.CompleteAsync();
            // reset the car object
            car = await repository.GetCar(car.Id);
            // map the result back to a CarResource
            var result = mapper.Map<Car, CarResource>(car);
            // return Ok (carResource); // Ok() inherited from the base controller class. Will return it with HTTP Status 200 
            return Ok(result);
        }

        // Delete Car
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCar(int id) {
            // get a car with this ID
            var car = await repository.GetCar(id, includeRelated : false);

            // if car is null instead of letting the application crash
            if (car == null)
                // we return a proper response to the user
                return NotFound();
            // remove it from the context
            repository.Remove(car);

            // save the changes
            await unitOfWork.CompleteAsync();

            // return a response with the same ID
            return Ok(id);
        }

        // API to get a Car
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCar(int id) {
            // get a car with this ID
            var car = await repository.GetCar(id);

            // if car is null instead of letting the application crash
            if (car == null)
                // we return a proper response to the user
                return NotFound();

            // if we have a Car Object we need to map it to CarResource and return it
            var carResource = mapper.Map<Car, CarResource>(car);

            // return a response with the carResource
            return Ok(carResource);
        }

        [HttpGet]
        public async Task<QueryResultResource<CarResource>> GetCars(CarQueryResource FilterResource)
        {
            // map to filter domain object
            var filter = mapper.Map<CarQueryResource, CarQuery>(FilterResource);
            var queryResult = await repository.GetCars(filter);

            // Map QueryResult to QueryResultResource
            return mapper.Map<QueryResult<Car>, QueryResultResource<CarResource>>(queryResult);
        }
    }
}