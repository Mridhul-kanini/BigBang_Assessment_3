using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Travel.DB;
using Travel.Interface;
using Travel.Model;

namespace Travel.Services
{
    public class BookingdetailsRepository : IBookingdetailsRepository
    {
        private readonly TravelContext _context;

        public BookingdetailsRepository(TravelContext context)
        {
            _context = context;
        }

        public IEnumerable<Bookingdetails> GetBookingdetails()
        {
            return _context.Bookingdetails.ToList();
        }

        public Bookingdetails GetBookingdetailById(int id)
        {
            return _context.Bookingdetails.FirstOrDefault(x => x.BookingId == id);
        }

        public Bookingdetails CreateBookingdetail(Bookingdetails bookingdetail)
        {
            _context.Add(bookingdetail);
            _context.SaveChanges();
            return bookingdetail;
        }

        public Bookingdetails UpdateBookingdetail(int id, Bookingdetails updatedBookingdetail)
        {
            var bookingdetail = _context.Bookingdetails.Find(id);
            if (bookingdetail != null)
            {
                bookingdetail.Name = updatedBookingdetail.Name;
                bookingdetail.Address = updatedBookingdetail.Address;
                bookingdetail.Email = updatedBookingdetail.Email;

                _context.Entry(bookingdetail).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return bookingdetail;
        }

        public Bookingdetails DeleteBookingdetail(int id)
        {
            var bookingdetail = _context.Bookingdetails.Find(id);
            if (bookingdetail != null)
            {
                _context.Bookingdetails.Remove(bookingdetail);
                _context.SaveChanges();
            }
            return bookingdetail;
        }
    }
}
