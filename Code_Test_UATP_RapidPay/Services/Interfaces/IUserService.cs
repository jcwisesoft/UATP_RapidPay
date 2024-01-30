using Code_Test_UATP_RapidPay.Models.Entities;
using Code_Test_UATP_RapidPay.Models.RequestModels;

namespace Code_Test_UATP_RapidPay.Services.Interfaces
{
    public interface IUserService
    {
        Task CreateUser(UserModel model);
        Task<List<User>> GetAll();
        Task<dynamic> Authenticate(UserModel model);
    }
}
