using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Travel.DB;
using Travel.Interface;
using Travel.Model;

namespace Travel.Services
{
    public class CarddetailsRepository : ICarddetailsRepository
    {
        private readonly TravelContext _context;

        public CarddetailsRepository(TravelContext context)
        {
            _context = context;
        }

        public IEnumerable<Carddetails> GetCarddetails()
        {
            return _context.Carddetails.ToList();
        }

        public Carddetails GetCarddetailById(int id)
        {
            return _context.Carddetails.FirstOrDefault(x => x.CardId == id);
        }

        public Carddetails CreateCarddetail(Carddetails carddetail)
        {
            _context.Add(carddetail);
            _context.SaveChanges();
            return carddetail;
        }

        public Carddetails UpdateCarddetail(int id, Carddetails updatedCarddetail)
        {
            var carddetail = _context.Carddetails.Find(id);
            if (carddetail != null)
            {
                carddetail.Username = updatedCarddetail.Username;
                carddetail.Cardnumber = updatedCarddetail.Cardnumber;
                carddetail.Expirydate = updatedCarddetail.Expirydate;
                carddetail.Cvv = updatedCarddetail.Cvv;

                _context.Entry(carddetail).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return carddetail;
        }

        public Carddetails DeleteCarddetail(int id)
        {
            var carddetail = _context.Carddetails.Find(id);
            if (carddetail != null)
            {
                _context.Carddetails.Remove(carddetail);
                _context.SaveChanges();
            }
            return carddetail;
        }
    }
}
