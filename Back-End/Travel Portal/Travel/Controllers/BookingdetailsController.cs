using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Travel.Interface;
using Travel.Model;

namespace Travel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingdetailsController : ControllerBase
    {
        private readonly IBookingdetailsRepository _repository;

        public BookingdetailsController(IBookingdetailsRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Bookingdetails
        [HttpGet]
        public ActionResult<IEnumerable<Bookingdetails>> GetBookingdetails()
        {
            var bookingdetails = _repository.GetBookingdetails();
            return Ok(bookingdetails);
        }

        // GET: api/Bookingdetails/5
        [HttpGet("{id}")]
        public ActionResult<Bookingdetails> GetBookingdetail(int id)
        {
            var bookingdetail = _repository.GetBookingdetailById(id);
            if (bookingdetail == null)
            {
                return NotFound();
            }
            return Ok(bookingdetail);
        }

        // POST: api/Bookingdetails
        [HttpPost]
        public ActionResult<Bookingdetails> PostBookingdetail(Bookingdetails bookingdetail)
        {
            var createdBookingdetail = _repository.CreateBookingdetail(bookingdetail);
            return CreatedAtAction(nameof(GetBookingdetail), new { id = createdBookingdetail.BookingId }, createdBookingdetail);
        }

        // PUT: api/Bookingdetails/5
        [HttpPut("{id}")]
        public IActionResult PutBookingdetail(int id, Bookingdetails bookingdetail)
        {
            if (id != bookingdetail.BookingId)
            {
                return BadRequest();
            }
            var updatedBookingdetail = _repository.UpdateBookingdetail(id, bookingdetail);
            if (updatedBookingdetail == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/Bookingdetails/5
        [HttpDelete("{id}")]
        public IActionResult DeleteBookingdetail(int id)
        {
            var bookingdetail = _repository.DeleteBookingdetail(id);
            if (bookingdetail == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
