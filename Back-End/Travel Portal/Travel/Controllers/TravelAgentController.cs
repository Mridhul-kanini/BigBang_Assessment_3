using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using Travel.DB;
using Travel.Interface;
using Travel.Model;
using Microsoft.Extensions.Options;

namespace Travel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelAgentController : ControllerBase
    {
        private readonly ITravelAgentRepository _repository;
        private readonly BlobOptions _blobOptions;

        public TravelAgentController(ITravelAgentRepository repository, IOptions<BlobOptions> blobOptions)
        {
            _repository = repository;
            _blobOptions = blobOptions.Value;
        }

        // GET: api/TravelAgent
        [HttpGet]
        public ActionResult<IEnumerable<TravelAgent>> GetTravelAgents()
        {
            var travelAgents = _repository.GetTravelAgents();
            return Ok(travelAgents);
        }

        // GET: api/TravelAgent/5
        [HttpGet("{id}")]
        public ActionResult<TravelAgent> GetTravelAgent(int id)
        {
            var travelAgent = _repository.GetTravelAgentById(id);
            if (travelAgent == null)
            {
                return NotFound();
            }
            return Ok(travelAgent);
        }

        // POST: api/TravelAgent
        [HttpPost]
        public async Task<ActionResult<TravelAgent>> PostTravelAgent([FromForm] AgentCreateViewModel AgentViewModel)
        {
            try
            {
                if (AgentViewModel.Image == null || AgentViewModel.Image.Length == 0)
                {
                    return BadRequest("No package image file was provided.");
                }

                // Save the package details to the database
                TravelAgent agentDetails = new TravelAgent
                {
                    Name = AgentViewModel.Name,
                    Description = AgentViewModel.Description,
                    Email = AgentViewModel.Email,
                    Address = AgentViewModel.Address,
                    Phone = AgentViewModel.Phone,
                    Password = AgentViewModel.Password,
                    Status = AgentViewModel.Status,
                };

                // Assuming you have a DbContext instance named "dbContext" for your database
                _repository.CreateTravelAgent(agentDetails);

                // Retrieve the newly generated PackageId
                int newPackageId = agentDetails.TravelId;

                // Retrieve the connection string and container name from the BlobOptions
                string connectionString = _blobOptions.ConnectionString;
                string containerName = "inagestorage"; // Replace with your container name

                // Create the BlobServiceClient
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                // Create the container if it does not exist
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync();

                // Generate a unique blob name based on the newPackageId and the uploaded image
                string blobName = $"{newPackageId}_{Guid.NewGuid().ToString()}{Path.GetExtension(AgentViewModel.Image.FileName)}";

                // Get a reference to the blob
                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                // Upload the package image to the blob
                using (var stream = AgentViewModel.Image.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }

                // After uploading the image to Blob Storage, set the ImageUrl property of the package
                agentDetails.ImageUrl = blobClient.Uri.ToString();

                // Update the package record in the database to include the ImageUrl
                _repository.UpdateTravelAgent(newPackageId, agentDetails);

                // Return the URL of the uploaded package image
                return Ok(agentDetails.ImageUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error uploading package image: {ex.Message}");
            }
        }

        // PUT: api/TravelAgent/5
        [HttpPut("{id}")]
        public IActionResult PutTravelAgent(int id, TravelAgent travelAgent)
        {
            if (id != travelAgent.TravelId)
            {
                return BadRequest();
            }
            var updatedTravelAgent = _repository.UpdateTravelAgent(id, travelAgent);
            if (updatedTravelAgent == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/TravelAgent/5
        [HttpDelete("{id}")]
        public IActionResult DeleteTravelAgent(int id)
        {
            var travelAgent = _repository.DeleteTravelAgent(id);
            if (travelAgent == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
