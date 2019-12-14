namespace PaymentsManager.Interfaces
{
    interface IFailablePayment : IPayment
    {
        void TryCommit();
    }
}
