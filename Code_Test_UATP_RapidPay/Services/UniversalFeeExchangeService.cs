namespace Code_Test_UATP_RapidPay.Services
{
    public class UniversalFeeExchangeService
    {
        private static UniversalFeeExchangeService _instance;
        private decimal _currentFee;

        // Private constructor to prevent instantiation from outside
        private UniversalFeeExchangeService(decimal initialFee)
        {
            _currentFee = initialFee;
        }

        // Public method to get the Singleton instance
        public static UniversalFeeExchangeService GetInstance(decimal initialFee)
        {
            if (_instance == null)
            {
                _instance = new UniversalFeeExchangeService(initialFee);
            }
            return _instance;
        }

        public void UpdateFee()
        {
            var random = new Random();
            _currentFee *= (decimal)(random.NextDouble() * 2);
        }

        public decimal GetCurrentFee() => _currentFee;
    }
}
