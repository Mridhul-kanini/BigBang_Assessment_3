using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Travel.DB;
using Travel.Interface;
using Travel.Model;

namespace Travel.Services
{
    public class TravelAgentRepository : ITravelAgentRepository
    {

        private readonly TravelContext _context;

        public TravelAgentRepository(TravelContext context)
        {
            _context = context;
        }

        public IEnumerable<TravelAgent> GetTravelAgents()
        {
            return _context.TravelAgents.ToList();
        }

        public TravelAgent GetTravelAgentById(int id)
        {
            return _context.TravelAgents.FirstOrDefault(x => x.TravelId == id);
        }

        public TravelAgent CreateTravelAgent(TravelAgent travelAgent)
        {
            _context.Add(travelAgent);
            _context.SaveChanges();
            return travelAgent;
        }

        public TravelAgent UpdateTravelAgent(int id, TravelAgent updatedTravelAgent)
        {
            var travelAgent = _context.TravelAgents.Find(id);
            if (travelAgent != null)
            {
                travelAgent.Name = updatedTravelAgent.Name;
                travelAgent.Email = updatedTravelAgent.Email;
                travelAgent.ImageUrl = updatedTravelAgent.ImageUrl;
                travelAgent.Address = updatedTravelAgent.Address;
                travelAgent.Phone = updatedTravelAgent.Phone;
                travelAgent.Password = updatedTravelAgent.Password;
                travelAgent.Status = updatedTravelAgent.Status;

                _context.Entry(travelAgent).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return travelAgent;
        }

        public TravelAgent DeleteTravelAgent(int id)
        {
            var travelAgent = _context.TravelAgents.Find(id);
            if (travelAgent != null)
            {
                _context.TravelAgents.Remove(travelAgent);
                _context.SaveChanges();
            }
            return travelAgent;
        }
    }
}
