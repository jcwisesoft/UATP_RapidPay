using Microsoft.EntityFrameworkCore;

namespace Code_Test_UATP_RapidPay.Models.Entities
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardPayment> CardPayments { get; set; }

        // Define other DbSets for additional models
    }
}
