using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Travel.Interface;
using Travel.Model;

namespace Travel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarddetailsController : ControllerBase
    {
        private readonly ICarddetailsRepository _repository;

        public CarddetailsController(ICarddetailsRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Carddetails
        [HttpGet]
        public ActionResult<IEnumerable<Carddetails>> GetCarddetails()
        {
            var carddetails = _repository.GetCarddetails();
            return Ok(carddetails);
        }

        // GET: api/Carddetails/5
        [HttpGet("{id}")]
        public ActionResult<Carddetails> GetCarddetail(int id)
        {
            var carddetail = _repository.GetCarddetailById(id);
            if (carddetail == null)
            {
                return NotFound();
            }
            return Ok(carddetail);
        }

        // POST: api/Carddetails
        [HttpPost]
        public ActionResult<Carddetails> PostCarddetail(Carddetails carddetail)
        {
            var createdCarddetail = _repository.CreateCarddetail(carddetail);
            return CreatedAtAction(nameof(GetCarddetail), new { id = createdCarddetail.CardId }, createdCarddetail);
        }

        // PUT: api/Carddetails/5
        [HttpPut("{id}")]
        public IActionResult PutCarddetail(int id, Carddetails carddetail)
        {
            if (id != carddetail.CardId)
            {
                return BadRequest();
            }
            var updatedCarddetail = _repository.UpdateCarddetail(id, carddetail);
            if (updatedCarddetail == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/Carddetails/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCarddetail(int id)
        {
            var carddetail = _repository.DeleteCarddetail(id);
            if (carddetail == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
