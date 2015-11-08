using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AppSheetProject
{
    class Program
    {
        
        static void Main(string[] args)
        {
            const string ServiceUrlKey = "serviceUrl";

            // read the service base address from configuration, enables to change the endpoint dynamically(production versus test/int environment)
            string serviceUrl = Utils.ReadSetting(ServiceUrlKey);
            if (string.IsNullOrWhiteSpace(serviceUrl))
            {
                Console.WriteLine("serviceUrl is null, verify that the key is present in the configuration");
                // log & throw ex  if this was not a console app (i.e., in production app/site)
                return;
            }

            // Initialize the data client. This can be done through Mef or some Dependency Resolver in a large project
            ICustomerDataClient dataClient = new CustomerDataClient(serviceUrl);

            // Initialize the data processor
            Processor customerDataProcessor = new Processor(dataClient);

            // Retrieve the youngest users and display the results
            Task<List<Customer>> getYoungestUsersTask = customerDataProcessor.GetYoungestCustomersAsync(5);

            Task.WaitAll(getYoungestUsersTask);

            List<Customer> result = getYoungestUsersTask.Result;

            if (result == null)
            {
                // we are not swallowing any exceptions, so web service calls succeeded but there are no users with valid phone numbers
                Console.WriteLine("There are no users with valid phone numbers");
            }
            else
            { 
                Console.WriteLine("The list of youngest users sorted by name are: \n{0}", 
                                string.Join("\n", result.Select(
                                        c=> string.Format("Name = {0},\tCustomerId = {1}\tAge= {2},\tNumber = {3}",
                                                            c.Name, c.Id, c.Age, c.Number))));
            }
        }
    }
}
