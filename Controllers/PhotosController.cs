using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using cars.Controllers.Resources;
using cars.Core;
using cars.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace cars.Controllers {
    // Handles Photos 
    [Route("/api/cars/{carId}/photos")]
    public class PhotosController : Controller {
        private readonly PhotoSettings photoSettings;
        private readonly IHostingEnvironment host;
        private readonly ICarRepository carRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public ICarRepository CarRepository { get; }
        private readonly IPhotoRepository photoRepository;
        public PhotosController (IHostingEnvironment host, ICarRepository carRepository, IPhotoRepository photoRepository, 
                IUnitOfWork unitOfWork, IMapper mapper, IOptionsSnapshot<PhotoSettings> options) 
        {
            this.photoRepository = photoRepository;
            this.photoSettings = options.Value;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.carRepository = carRepository;
            this.host = host;
        }

        [HttpGet]
        public async Task<IEnumerable<PhotoResource>> GetPhotos(int carId) 
        {
            // Get photos from the repository
            var photos = await photoRepository.GetPhotos (carId);
            // Map the Photos to PhotoResource
            return mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(photos);
        }

        [HttpPost]
        public async Task<IActionResult> Upload (int carId, IFormFile file) {

            // get the car's Id
            var car = await carRepository.GetCar(carId, includeRelated : false);
            // when car's Id does not exist
            if (car == null)
                return NotFound ();

            // file is null
            if (file == null) return BadRequest ("Null file");

            // file is empty
            if (file.Length == 0) return BadRequest ("Empty file");

            // file size limit
            if (file.Length > photoSettings.MaxBytes) return BadRequest ("Max file size exceeded");

            // not an image
            if (!photoSettings.IsSupported (file.FileName)) return BadRequest ("Invalid file type.");
            // returns the exact path of uploads in the hosting machine
            var uploadsFolderPath = Path.Combine (host.WebRootPath, "uploads");
            // when upload does not exist
            if (!Directory.Exists (uploadsFolderPath))
                // create the uploads folder
                Directory.CreateDirectory (uploadsFolderPath);

            // Generating a new filename for security reasons
            var fileName = Guid.NewGuid ().ToString () + Path.GetExtension (file.FileName);
            // Get the filePath
            var filePath = Path.Combine (uploadsFolderPath, fileName);
            // reading the file
            using (var stream = new FileStream (filePath, FileMode.Create)) {
                await file.CopyToAsync (stream);
            }

            // updating the database
            var photo = new Photo { FileName = fileName };
            car.Photos.Add (photo);

            // save
            await unitOfWork.CompleteAsync ();

            return Ok (mapper.Map<Photo, PhotoResource> (photo));
        }

    }
}