using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Travel.DB;
using Travel.Interface;
using Travel.Model;

namespace Travel.Services
{
    public class HotelsRepository : IHotelsRepository
    {
        private readonly TravelContext _context;

        public HotelsRepository(TravelContext context)
        {
            _context = context;
        }

        public IEnumerable<Hotels> GetHotels()
        {
            return _context.Hotels.ToList();
        }

        public Hotels GetHotelById(int id)
        {
            return _context.Hotels.FirstOrDefault(x => x.HotelId == id);
        }

        public Hotels CreateHotel(Hotels hotel)
        {
            _context.Add(hotel);
            _context.SaveChanges();
            return hotel;
        }

        public Hotels UpdateHotel(int id, Hotels updatedHotel)
        {
            var hotel = _context.Hotels.Find(id);
            if (hotel != null)
            {
                hotel.Name = updatedHotel.Name;
                hotel.Description = updatedHotel.Description;
                hotel.Type = updatedHotel.Type;
                hotel.Location = updatedHotel.Location;
                hotel.ImageUrl = updatedHotel.ImageUrl;

                _context.Entry(hotel).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return hotel;
        }

        public Hotels DeleteHotel(int id)
        {
            var hotel = _context.Hotels.Find(id);
            if (hotel != null)
            {
                _context.Hotels.Remove(hotel);
                _context.SaveChanges();
            }
            return hotel;
        }
    }
}
