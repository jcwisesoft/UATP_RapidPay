using Code_Test_UATP_RapidPay.Models.Entities;
using Code_Test_UATP_RapidPay.Models.RequestModels;
using Code_Test_UATP_RapidPay.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Code_Test_UATP_RapidPay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : BaseAPIController
    {
        private readonly ICardService _cardService;
        public CardController(ICardService cardService) {
            _cardService = cardService;
        }

        [HttpPost("create"), Authorize]
        public async Task<IActionResult> Create()
        {
            Card card = await _cardService.CreateCard();

            return Ok(card, "Card created successfully");
        }

        [HttpPost("pay"), Authorize]
        public async Task<IActionResult> Pay(CardModel model)
        {
            bool isSuccessful = await _cardService.Pay(model.CardNumber, model.Amount);
            if (isSuccessful)
            {
                return Ok( "Card payment was successfully");
            }
            else
            {
                return BadRequest("insufficient balance");
            }

        }

        [HttpGet("balance"), Authorize]
        public async Task<IActionResult> Balance(string cardNumber)
        {
            decimal balance = await _cardService.GetBalance(cardNumber);
            return Ok(new {balance});
            
        }

        [HttpGet("all_cards"), Authorize]
        public async Task<IActionResult> GetAll()
        {
            var allcards = await _cardService.GetAll();
            return Ok(allcards);

        }

    }
}
