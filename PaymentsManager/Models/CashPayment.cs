using PaymentsManager.Interfaces;

namespace PaymentsManager.Models
{
    public class CashPayment : IPayment
    {
        public string Currency { get; set; }
        public decimal Sum { get; set; }
        public bool HasChange { get; set; }
        public bool? IsSuccessful { get; } = true;
    }
}
