using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AppSheetProject
{
    public class CustomerDataClient : ICustomerDataClient
    {
        const string GetCustomersUri = "list/";
        const string GetCustomersWithTokenUri = "list/?token={0}";
        const string GetCustomerDetailsUri = "detail/{0}";

        const string JsonMediaTypeValue = "application/json";

        Uri serviceBaseUri;

        public CustomerDataClient(string serviceUrl)
        {
            if (string.IsNullOrWhiteSpace(serviceUrl))
            {
                throw new ArgumentNullException("serviceUrl must be a valid uri string");
            }

            this.serviceBaseUri = new Uri(serviceUrl);
        }

        public Uri ServiceUri
        {
            get
            {
                return this.serviceBaseUri;
            }
        }

        public async Task<UserIdListData> GetUserListAsync(string token)
        {
            using (var client = new HttpClient())
            {
                // setup httclient
                client.BaseAddress = this.ServiceUri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMediaTypeValue));

                try
                {
                    string getCustomersUri = string.IsNullOrWhiteSpace(token) ? GetCustomersUri : string.Format(GetCustomersWithTokenUri, token);

                    HttpResponseMessage response = await client.GetAsync(getCustomersUri);
                    response.EnsureSuccessStatusCode();

                    UserIdListData content = await response.Content.ReadAsAsync<UserIdListData>();
                    return content;
                }
                catch (HttpRequestException ex)
                {
                    string message = string.Format("Failed to process the request for GetUsersList. Exception: {0}", ex);
                    // log the message for monitoring 
             
                    throw new InvalidOperationException(message, ex);
                }
            }
        }


        public async Task<Customer> GetUserDetailsAsync(int userId)
        {
            using (var client = new HttpClient())
            {
                // setup httpClient
                client.BaseAddress = this.ServiceUri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMediaTypeValue));

                try
                {
                    string userDetailsUri = string.Format(GetCustomerDetailsUri, userId);

                    HttpResponseMessage response = await client.GetAsync(userDetailsUri);
                    response.EnsureSuccessStatusCode();
                    
                    Customer customerDetails = await response.Content.ReadAsAsync<Customer>();
                    return customerDetails;
                }
                catch (HttpRequestException ex)
                {
                    string message = string.Format("Failed to process the request for GetUserDetails. Exception: {0}", ex);
                    // log the message for monitoring 

                    throw new InvalidOperationException(message, ex);
                }
            }
        }
    }
}
