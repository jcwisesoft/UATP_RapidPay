using Code_Test_UATP_RapidPay.Models.Entities;

namespace Code_Test_UATP_RapidPay.Services.Interfaces
{
    public interface ICardService
    {
        Task<Card> CreateCard();
        Task<List<Card>> GetAll();
        Task<bool> Pay(string cardNumber, decimal amount);
        Task<decimal> GetBalance(string cardNumber);
    }
}
