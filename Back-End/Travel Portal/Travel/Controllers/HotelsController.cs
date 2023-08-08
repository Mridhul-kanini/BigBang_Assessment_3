using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Travel.Interface;
using Travel.Model;

namespace Travel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelsRepository _repository;

        public HotelsController(IHotelsRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Hotels
        [HttpGet]
        public ActionResult<IEnumerable<Hotels>> GetHotels()
        {
            var hotels = _repository.GetHotels();
            return Ok(hotels);
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public ActionResult<Hotels> GetHotel(int id)
        {
            var hotel = _repository.GetHotelById(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return Ok(hotel);
        }

        // POST: api/Hotels
        [HttpPost]
        public ActionResult<Hotels> PostHotel(Hotels hotel)
        {
            var createdHotel = _repository.CreateHotel(hotel);
            return CreatedAtAction(nameof(GetHotel), new { id = createdHotel.HotelId }, createdHotel);
        }

        // PUT: api/Hotels/5
        [HttpPut("{id}")]
        public IActionResult PutHotel(int id, Hotels hotel)
        {
            if (id != hotel.HotelId)
            {
                return BadRequest();
            }
            var updatedHotel = _repository.UpdateHotel(id, hotel);
            if (updatedHotel == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public IActionResult DeleteHotel(int id)
        {
            var hotel = _repository.DeleteHotel(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
