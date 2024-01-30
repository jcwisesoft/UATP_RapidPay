using Code_Test_UATP_RapidPay.Models.Entities;
using Code_Test_UATP_RapidPay.Models.RequestModels;
using Code_Test_UATP_RapidPay.Services.Interfaces;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Code_Test_UATP_RapidPay.Services
{
    public class CardService : ICardService
    {
        private readonly ApiDbContext _context;

        public CardService(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<Card> CreateCard()
        {
            Card card = new Card();
            // Hash the password before saving
            card.CardNumber = GenerateRandomDigits(15);
            card.ExpireDate = CalculateExpirationDate(5);
            card.Cvv = Convert.ToInt32(GenerateRandomDigits(3));
            
            card = _context.Cards.Add(card).Entity;
            await _context.SaveChangesAsync();

            return card;

        }

        public async Task<bool> Pay(string cardNumber, decimal amount)
        {
            try
            {
                CardPayment cardPayment = null;

                var card = _context.Cards.SingleOrDefault(p => p.CardNumber == cardNumber);
                var lastCardPayment = _context.CardPayments.OrderByDescending(cp => cp.Id).FirstOrDefault();
                decimal currentFee = lastCardPayment is not null? lastCardPayment.Fee : 0;
                if (card is null) throw new Exception("Record not Found");
                var ufeService = UniversalFeeExchangeService.GetInstance(currentFee);
                var totalAmount = amount * ufeService.GetCurrentFee();

                if (card.Balance >= totalAmount)
                {
                    bool isFound = true;
                    string reference = null;
                    while (isFound)
                    {
                        reference = GenerateRandomDigits(10).ToString();
                        cardPayment = _context.CardPayments.SingleOrDefault(p => p.Reference == reference);
                        if (cardPayment == null)
                        {
                            isFound = false;
                        }
                    }
                    card.Balance -= totalAmount;
                    cardPayment = new CardPayment();
                    cardPayment.Amount = amount;
                    cardPayment.CardId = card.Id;
                    cardPayment.Reference = reference;
                    _context.CardPayments.Add(cardPayment);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        public async Task<decimal> GetBalance(string cardNumber)
        {
            var card = _context.Cards.SingleOrDefault(p => p.CardNumber == cardNumber);
            return card is not null? card.Balance: 0;
        }
        public async Task<List<Card>> GetAll()
        {
            return await _context.Cards.ToListAsync();

        }
        private string GenerateRandomDigits(int length)
        {
            var random = new Random();
            var randomNumber = string.Empty;

            for (int i = 0; i < length; i++)
            {
                randomNumber += random.Next(0, 10).ToString();
            }
            
            return randomNumber;
        }

        public static string CalculateExpirationDate(int validityYears)
        {
            // Get the current date
            DateTime currentDate = DateTime.Now;

            // Add the validity period in years to the current date
            DateTime expirationDate = currentDate.AddYears(validityYears);

            // Format the expiration date as "MM/YY"
            string formattedExpirationDate = expirationDate.ToString("MM/yy");

            return formattedExpirationDate;
        }
    }
}
