using Travel.Model;

namespace Travel.Interface
{
    public interface IHotelsRepository
    {
        IEnumerable<Hotels> GetHotels();
        Hotels GetHotelById(int id);
        Hotels CreateHotel(Hotels hotel);
        Hotels UpdateHotel(int id, Hotels updatedHotel);
        Hotels DeleteHotel(int id);
    }
}
