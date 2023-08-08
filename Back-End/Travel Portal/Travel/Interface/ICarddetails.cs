using Travel.Model;

namespace Travel.Interface
{
    public interface ICarddetailsRepository
    {
        IEnumerable<Carddetails> GetCarddetails();
        Carddetails GetCarddetailById(int id);
        Carddetails CreateCarddetail(Carddetails carddetail);
        Carddetails UpdateCarddetail(int id, Carddetails updatedCarddetail);
        Carddetails DeleteCarddetail(int id);
    }
}
