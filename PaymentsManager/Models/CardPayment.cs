using PaymentsManager.Interfaces;
using System;

namespace PaymentsManager.Models
{
    public class CardPayment : IFailablePayment
    {
        public string BankName { get; set; }
        public string CardMask { get; set; }
        public decimal Sum { get; set; }
        public bool? IsSuccessful { get; private set; }

        public void TryCommit()
        {
            Random rndm = new Random();
            bool coinFlip = rndm.Next(0, 1) == 1 ? true : false;
            IsSuccessful = coinFlip;
        }
    }
}
