using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Azure.Storage.Blobs;
using Travel.Interface;
using Travel.Model;

namespace Travel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImagesRepository _repository;
        private readonly BlobOptions _blobOptions;

        public ImagesController(IImagesRepository repository, IOptions<BlobOptions> blobOptions)
        {
            _repository = repository;
            _blobOptions = blobOptions.Value;
        }

        // GET: api/Images
        [HttpGet]
        public ActionResult<IEnumerable<Images>> GetImages()
        {
            var images = _repository.GetImages();
            return Ok(images);
        }

        // GET: api/Images/location/{location}
        [HttpGet("location/{location}")]
        public ActionResult<IEnumerable<Images>> GetImagesByLocation(string location)
        {
            var images = _repository.GetImages();

            if (!string.IsNullOrEmpty(location))
            {
                images = images.Where(image => image.Location == location);
            }

            return Ok(images);
        }

        // GET: api/Images/5
        [HttpGet("{id}")]
        public ActionResult<Images> GetImage(int id)
        {
            var image = _repository.GetImageById(id);
            if (image == null)
            {
                return NotFound();
            }
            return Ok(image);
        }

        // POST: api/Images
        [HttpPost]
        public async Task<ActionResult<Images>> PostImage([FromForm] ImagesCreateViewModel imageViewModel)
        {
            try
            {
                if (imageViewModel.Image == null || imageViewModel.Image.Length == 0)
                {
                    return BadRequest("No image file was provided.");
                }

                Images imageDetails = new Images
                {
                    Name = imageViewModel.Name,
                    Location = imageViewModel.Location
                };

                _repository.CreateImage(imageDetails);

                int newImageId = imageDetails.ImgId;

                string connectionString = _blobOptions.ConnectionString;
                string containerName = "inagestorage";

                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync();

                string blobName = $"{newImageId}_{Guid.NewGuid().ToString()}{Path.GetExtension(imageViewModel.Image.FileName)}";

                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                using (var stream = imageViewModel.Image.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }

                imageDetails.ImageUrl = blobClient.Uri.ToString();

                _repository.UpdateImage(newImageId, imageDetails);

                return CreatedAtAction(nameof(GetImage), new { id = newImageId }, imageDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error uploading image: {ex.Message}");
            }
        }

        // PUT: api/Images/5
        [HttpPut("{id}")]
        public IActionResult PutImage(int id, Images image)
        {
            if (id != image.ImgId)
            {
                return BadRequest();
            }
            var updatedImage = _repository.UpdateImage(id, image);
            if (updatedImage == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/Images/5
        [HttpDelete("{id}")]
        public IActionResult DeleteImage(int id)
        {
            var image = _repository.DeleteImage(id);
            if (image == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}