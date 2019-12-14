using PaymentsManager.Interfaces;
using PaymentsManager.Models;
using System;

namespace PaymentsManager
{
    class Program
    {
        static void Main(string[] args)
        {
            // Task 1
            var operator1 = new PaymentsOperator(25);

            IPayment cardPayment = new CardPayment()
            {
                BankName = "VTB",
                CardMask = "3456**************",
                Sum = 10
            };
            IPayment cashPayment = new CashPayment()
            {
                Currency = "RUB",
                HasChange = false,
                Sum = 15
            };

            operator1.AddPayment(cardPayment);
            operator1.AddPayment(cashPayment);

            Console.WriteLine($"Left to pay: {operator1.GetSumToPay()}");

            // Task 2
            operator1 = new PaymentsOperator(4);
            try
            {
                operator1.AddPayment(cardPayment);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception of type {exception.GetType().Name} raised");
            }

            operator1.OnPaymentAdded += (payment, leftToPay) => Console.WriteLine($"Added new {payment.GetType().Name} (sum: {payment.Sum}). Left to pay: {leftToPay}");
            operator1.AddPayment(new CashPayment() { Sum = 1 });
            operator1.AddPayment(new CardPayment() { Sum = 2 });

            // Task 3
            operator1 = new PaymentsOperator(25);
            operator1.AddPayment(new CashPayment() { Sum = 10 });
            try
            {
                operator1.CommitTransaction();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception of type {exception.GetType().Name} raised");
            }
            operator1.AddPayment(new CashPayment() { Sum = 15 });
            operator1.CommitTransaction();

            // Task 4
            operator1 = new PaymentsOperator(25);
            for (var i = 0; i < 25 / 5; i++)
                operator1.AddPayment(new CardPayment() { Sum = 5 });
            operator1.OnPaymentFailed += payment =>
                {
                    if (payment is CardPayment cp)
                    {
                        Console.WriteLine($"Card payment for {cp.Sum} has failed. Retrying...");
                        operator1.AddPayment(new CardPayment()
                        {
                            CardMask = cp.CardMask,
                            BankName = cp.BankName,
                            Sum = cp.Sum
                        });
                        operator1.CommitTransaction();
                    }
                };

            operator1.CommitTransaction();

            Console.ReadLine();
        }
    }
}
