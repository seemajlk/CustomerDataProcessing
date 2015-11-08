using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppSheetProject
{
    /// <summary>
    /// Class that processes the customer data and provide methods to retrieve various analytics
    /// </summary>
    public class Processor
    {
        const int MaxDegreeOfParallelism = 8;
        ICustomerDataClient customerDataClient;
 
        /// <summary>
        /// Initializes and instance of Processor class
        /// </summary>
        /// <param name="client"> customerDataClient that handles the calls to web service</param>
        public Processor(ICustomerDataClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client", "customerDataClient must not be null");
            }

            this.customerDataClient = client;
        }
        
        /// <summary>
        /// Gets the youngest customers with valid phone numbers
        /// </summary>
        /// <param name="count"> customer count</param>
        /// <returns>List of youngest 'count' number of customers with valid phone numbers sorted by their name </returns>
        public async Task<List<Customer>> GetYoungestCustomersAsync(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentException(string.Format("'count' must be greater than 0. Actual value is {0}", count));
            }

            UserIdListData content = await this.customerDataClient.GetUserListAsync(null);
            
            List<int> userIds = new List<int>();
            userIds.AddRange(content.Result);
            
            // In this approach we are getting all the user ids sequentially and then retrieving the user details in parallel
            // The assumption here is that the size of the data here is less since we are only retrieving the ids, hence the calls will be faster
            // If there are too many users, we could start querying the userDetails in parallel, without waiting to retrieve
            // all the user ids.
            while (content.Token != null)
            {
                content = await this.customerDataClient.GetUserListAsync(content.Token);
                if (content.Result != null)
                {
                    userIds.AddRange(content.Result);
                }
            }
            
            ConcurrentBag<Customer> userDetails = new ConcurrentBag<Customer>();
            
            // here we are concurrently retrieving all user details with a maximum degree of parallelism
            // and saving it into a thread safe collection, i.e., a Concurrent Bag
            await userIds.ForEachAsync(MaxDegreeOfParallelism, async id =>
                {
                    Customer c = await this.customerDataClient.GetUserDetailsAsync(id);
                    userDetails.Add(c);
                });


            // Get the youngest users with Valid Phone Numbers and sort the list by their name
            // Phone number validation is very simple here, but we could potentially do more thorough validation 
            // based on specific application requirement

            // Here we are using the Linq operations to sort. However, if the number of items is in very large numbers(in millions), 
            // we could use a more efficient algorithm like Selection Rank 

            List<Customer> youngestUsers = userDetails
                                    .Where(c => Utils.IsValidPhoneNo(c.Number)) // consider only those with valid phone number
                                    .OrderBy(c => c.Age)                        // sort by age
                                    .Take(count)                                // get the top count users
                                    .OrderBy(c=>c.Name)                         // sort by the customer name
                                    .ToList();

            return youngestUsers;
        }
    }
}
