using AppSheetProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSheetTestProject.Mocks
{
    class MockDataClient : ICustomerDataClient
    {
        Dictionary<int, Customer> customerMap = new Dictionary<int, Customer>();

        public MockDataClient(string serviceUrl)
        {
            if (string.IsNullOrWhiteSpace(serviceUrl))
            {
                throw new ArgumentNullException("serviceUrl must be a valid uri string");
            }
            this.InitializeData();
        }

        public UserIdListData UserIds
        {
            get
            {
                return new UserIdListData
                {
                    Result = new List<int> { 1, 2, 3, 4, 5 }
                };
            }
        }

        public Dictionary<int, Customer> CustomerMap
        {
            get
            {
                return this.customerMap;
            }
        }

        public Task<Customer> GetUserDetailsAsync(int userId)
        {
            return Task.Run(() =>
                { return this.CustomerMap[userId]; });
        }

        public Task<UserIdListData> GetUserListAsync(string token)
        {
            return Task.Run(() => { return this.UserIds; });
        }

        private void InitializeData()
        {
            this.customerMap = new Dictionary<int, Customer>();
            foreach (int id in this.UserIds.Result)
            {
                this.CustomerMap.Add(id, new Customer { Id = id, Age = id * 2, Name = "MyName" + id, Number = "1234567890" });
                // tests can modify the data directly
            }
        }
    }
}
