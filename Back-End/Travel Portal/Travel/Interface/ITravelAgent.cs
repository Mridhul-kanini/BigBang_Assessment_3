using System.Collections.Generic;
using Travel.Model;

namespace Travel.Interface
{
    public interface ITravelAgentRepository
    {
        IEnumerable<TravelAgent> GetTravelAgents();
        TravelAgent GetTravelAgentById(int id);
        TravelAgent CreateTravelAgent(TravelAgent travelAgent);
        TravelAgent UpdateTravelAgent(int id, TravelAgent updatedTravelAgent);
        TravelAgent DeleteTravelAgent(int id);
    }
}
