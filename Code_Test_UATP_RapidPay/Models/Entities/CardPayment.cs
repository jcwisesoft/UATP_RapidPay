namespace Code_Test_UATP_RapidPay.Models.Entities
{
    public class CardPayment
    {
        public int Id { get; set; }
        public int CardId { get;set; }
        public decimal Amount { get; set; } = 0;
        public decimal Fee { get; set; } = 0;
        public string Reference { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
