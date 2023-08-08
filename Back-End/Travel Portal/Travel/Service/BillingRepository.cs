using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Travel.DB;
using Travel.Interface;
using Travel.Model;

namespace Travel.Services
{
    public class BillingRepository : IBillingRepository
    {
        private readonly TravelContext _context;

        public BillingRepository(TravelContext context)
        {
            _context = context;
        }

        public IEnumerable<Billing> GetBillings()
        {
            return _context.Billings.ToList();
        }

        public Billing GetBillingById(int id)
        {
            return _context.Billings.FirstOrDefault(x => x.BillingId == id);
        }

        public Billing CreateBilling(Billing billing)
        {
            _context.Add(billing);
            _context.SaveChanges();
            return billing;
        }

        public Billing UpdateBilling(int id, Billing updatedBilling)
        {
            var billing = _context.Billings.Find(id);
           
            return billing;
        }

        public Billing DeleteBilling(int id)
        {
            var billing = _context.Billings.Find(id);
            if (billing != null)
            {
                _context.Billings.Remove(billing);
                _context.SaveChanges();
            }
            return billing;
        }
    }
}
