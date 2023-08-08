using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Travel.Interface;
using Travel.Model;

namespace Travel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingsController : ControllerBase
    {
        private readonly IBillingRepository _repository;

        public BillingsController(IBillingRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Billings
        [HttpGet]
        public ActionResult<IEnumerable<Billing>> GetBillings()
        {
            var billings = _repository.GetBillings();
            return Ok(billings);
        }

        // GET: api/Billings/5
        [HttpGet("{id}")]
        public ActionResult<Billing> GetBilling(int id)
        {
            var billing = _repository.GetBillingById(id);
            if (billing == null)
            {
                return NotFound();
            }
            return Ok(billing);
        }

        // POST: api/Billings
        [HttpPost]
        public ActionResult<Billing> PostBilling(Billing billing)
        {
            var createdBilling = _repository.CreateBilling(billing);
            return CreatedAtAction(nameof(GetBilling), new { id = createdBilling.BillingId }, createdBilling);
        }

        // PUT: api/Billings/5
        [HttpPut("{id}")]
        public IActionResult PutBilling(int id, Billing billing)
        {
            if (id != billing.BillingId)
            {
                return BadRequest();
            }
            var updatedBilling = _repository.UpdateBilling(id, billing);
            if (updatedBilling == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/Billings/5
        [HttpDelete("{id}")]
        public IActionResult DeleteBilling(int id)
        {
            var billing = _repository.DeleteBilling(id);
            if (billing == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
