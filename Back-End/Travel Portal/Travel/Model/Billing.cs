using System.ComponentModel.DataAnnotations;

namespace Travel.Model
{
    public class Billing
    {
        [Key]
        public int BillingId { get; set; }
        public int PackageId { get; set; }
        public int UserId { get; set; }
        public int AgentId { get; set; }

    }
}
