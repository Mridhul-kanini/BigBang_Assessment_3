using Travel.Model;

namespace Travel.Interface
{
    public interface IBookingdetailsRepository
    {
        IEnumerable<Bookingdetails> GetBookingdetails();
        Bookingdetails GetBookingdetailById(int id);
        Bookingdetails CreateBookingdetail(Bookingdetails bookingdetail);
        Bookingdetails UpdateBookingdetail(int id, Bookingdetails updatedBookingdetail);
        Bookingdetails DeleteBookingdetail(int id);
    }
}
