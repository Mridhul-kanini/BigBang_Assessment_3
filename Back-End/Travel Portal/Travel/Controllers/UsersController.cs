using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Travel.DB;
using Travel.Model;

namespace Travel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly TravelContext _context;
        private readonly BlobOptions _blobOptions;


        public UsersController(TravelContext context, IOptions<BlobOptions> blobOptions)
        {
            _context = context;
            _blobOptions = blobOptions.Value; 
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostUser([FromForm] UserCreateViewModel userViewModel)
        {
            try
            {
                if (userViewModel.Image == null || userViewModel.Image.Length == 0)
                {
                    return BadRequest("No user image file was provided.");
                }

                // Save the user details to the database
                User userDetails = new User
                {
                    Name = userViewModel.Name,
                    Email = userViewModel.Email,
                    Phone = userViewModel.Phone,
                    Password = userViewModel.Password,
                    Address = userViewModel.Address
                };

                // Assuming you have a DbContext instance named "dbContext" for your database
                _context.Users.Add(userDetails);
                await _context.SaveChangesAsync();

                // Retrieve the newly generated UserId
                int newUserId = userDetails.UserId;

                // Retrieve the connection string and container name
                string connectionString = _blobOptions.ConnectionString;
                string containerName = "inagestorage";

                // Create the BlobServiceClient
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                // Create the container if it does not exist
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync();

                // Generate a unique blob name based on the newUserId and the uploaded image
                string blobName = $"{newUserId}_{Guid.NewGuid().ToString()}{Path.GetExtension(userViewModel.Image.FileName)}";

                // Get a reference to the blob
                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                // Upload the user image to the blob
                using (var stream = userViewModel.Image.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }

                // After uploading the image to Blob Storage, set the Image property of the user
                userDetails.Image = blobClient.Uri.ToString();

                // Update the user record in the database to include the Image URL
                _context.Users.Update(userDetails);
                await _context.SaveChangesAsync();

                // Return the URL of the uploaded user image
                return Ok(userDetails.Image);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error uploading user image: {ex.Message}");
            }
        }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
