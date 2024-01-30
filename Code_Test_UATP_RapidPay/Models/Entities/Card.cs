namespace Code_Test_UATP_RapidPay.Models.Entities
{
    public class Card
    {
        public int Id { get; set; } // Primary key    }
        public string CardNumber { get; set; }
        public string ExpireDate { get; set; }
        public int? Cvv { get; set; }
        public decimal Balance { get; set; } = 1000; // Assuming that on creation, every card is initialized with 1000 amount
    }
}
