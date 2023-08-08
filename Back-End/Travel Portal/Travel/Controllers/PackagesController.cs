using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Travel.Interface;
using Travel.Model;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;

namespace Travel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private readonly IPackagesRepository _repository;
        private readonly BlobOptions _blobOptions;

        public PackagesController(IPackagesRepository repository, IOptions<BlobOptions> blobOptions)
        {
            _repository = repository;
            _blobOptions = blobOptions.Value;
        }

        // GET: api/Packages
        [HttpGet]
        public ActionResult<IEnumerable<Packages>> GetPackages(int? travelId)
        {
            if (travelId.HasValue)
            {
                var packagesByTravelId = _repository.GetPackagesByTravelId(travelId.Value);
                return Ok(packagesByTravelId);
            }

            var packages = _repository.GetPackages();
            return Ok(packages);
        }

        // GET: api/Packages/5
        [HttpGet("{id}")]
        public ActionResult<Packages> GetPackage(int id)
        {
            var package = _repository.GetPackageById(id);
            if (package == null)
            {
                return NotFound();
            }
            return Ok(package);
        }

        // GET: api/Packages/ByTravelId/5
        [HttpGet("ByTravelId/{travelId}")]
        public ActionResult<IEnumerable<Packages>> GetPackagesByTravelId(int travelId)
        {
            var packages = _repository.GetPackagesByTravelId(travelId);
            if (packages == null || !packages.Any())
            {
                return NotFound();
            }
            return Ok(packages);
        }

        // POST: api/Packages
        [HttpPost]
        public async Task<ActionResult<Packages>> PostPackage([FromForm] PackageCreateViewModel packageViewModel)
        {
            try
            {
                if (packageViewModel.Image == null || packageViewModel.Image.Length == 0)
                {
                    return BadRequest("No package image file was provided.");
                }

                // Save the package details to the database
                Packages packageDetails = new Packages
                {
                    TravelId = packageViewModel.TravelId,
                    Name = packageViewModel.Name,
                    Description = packageViewModel.Description,
                    Location = packageViewModel.Location,
                    Type = packageViewModel.Type,
                    Price = packageViewModel.Price,
                };

                // Assuming you have a DbContext instance named "dbContext" for your database
                _repository.CreatePackage(packageDetails);

                // Retrieve the newly generated PackageId
                int newPackageId = packageDetails.PackageId;

                // Retrieve the connection string and container name from the BlobOptions
                string connectionString = _blobOptions.ConnectionString;
                string containerName = "inagestorage"; // Replace with your container name

                // Create the BlobServiceClient
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                // Create the container if it does not exist
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync();

                // Generate a unique blob name based on the newPackageId and the uploaded image
                string blobName = $"{newPackageId}_{Guid.NewGuid().ToString()}{Path.GetExtension(packageViewModel.Image.FileName)}";

                // Get a reference to the blob
                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                // Upload the package image to the blob
                using (var stream = packageViewModel.Image.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }

                // After uploading the image to Blob Storage, set the ImageUrl property of the package
                packageDetails.ImageUrl = blobClient.Uri.ToString();

                // Update the package record in the database to include the ImageUrl
                _repository.UpdatePackage(newPackageId, packageDetails);

                // Return the URL of the uploaded package image
                return Ok(packageDetails.ImageUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error uploading package image: {ex.Message}");
            }
        }


        // PUT: api/Packages/5
        [HttpPut("{id}")]
        public IActionResult PutPackage(int id, Packages package)
        {
            if (id != package.PackageId)
            {
                return BadRequest();
            }
            var updatedPackage = _repository.UpdatePackage(id, package);
            if (updatedPackage == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/Packages/5
        [HttpDelete("{id}")]
        public IActionResult DeletePackage(int id)
        {
            var package = _repository.DeletePackage(id);
            if (package == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
