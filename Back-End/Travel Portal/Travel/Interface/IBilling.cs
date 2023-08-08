using Travel.Model;

namespace Travel.Interface
{
    public interface IBillingRepository
    {
        IEnumerable<Billing> GetBillings();
        Billing GetBillingById(int id);
        Billing CreateBilling(Billing billing);
        Billing UpdateBilling(int id, Billing updatedBilling);
        Billing DeleteBilling(int id);
    }
}
