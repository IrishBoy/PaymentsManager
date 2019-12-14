using PaymentsManager.Interfaces;
using System;
using System.Collections.Generic;

namespace PaymentsManager
{
    class PaymentsOperator
    {
        List<IPayment> _payments;
        decimal _needToPay;

        public event Action<IPayment, decimal> OnPaymentAdded;
        public event Action<IPayment> OnPaymentFailed;

        public PaymentsOperator(decimal needToPay)
        {
            _payments = new List<IPayment>();
            _needToPay = needToPay;
        }

        public void AddPayment(IPayment payment)
        {
            if (_needToPay < payment.Sum)
                throw new InvalidOperationException("You really don't have to~");  // Somehow exception may not be caught in try-catch inside Program.Main

            _payments.Add(payment);
            _needToPay -= payment.Sum;
            OnPaymentAdded?.Invoke(payment, _needToPay);
        }
        public decimal GetSumToPay()
        {
            return _needToPay;
        }
        public void CommitTransaction()
        {
            if (_needToPay > 0)
                throw new InvalidOperationException("Where is the money, Lebowski?");  // Somehow exception may not be caught in try-catch inside Program.Main

            _payments.ForEach(payment => {
                if (!payment.IsSuccessful.HasValue)
                {
                    IFailablePayment failablePayment = (IFailablePayment) payment;
                    failablePayment.TryCommit();
                    if (!failablePayment.IsSuccessful.Value)
                    {
                        OnPaymentFailed?.Invoke(payment);
                        return;
                    }
                }
            });
        }
    }
}
