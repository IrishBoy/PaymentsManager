namespace PaymentsManager.Interfaces
{
    public interface IPayment
    {
        decimal Sum { get; }
        bool? IsSuccessful { get; }
    }
}
