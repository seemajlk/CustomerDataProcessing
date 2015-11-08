using System;
using System.Threading.Tasks;

namespace AppSheetProject
{
    public interface ICustomerDataClient
    {
        Task<Customer> GetUserDetailsAsync(int userId);
        Task<UserIdListData> GetUserListAsync(string token);
    }
}
