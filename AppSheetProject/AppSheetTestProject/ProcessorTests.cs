using AppSheetProject;
using AppSheetTestProject.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AppSheetTestProject
{
    [TestClass]
    public class ProcessorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsWhenDataClientIsNull()
        {
            CustomerDataClient client = null;

            new Processor(client);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetYoungestCustomersAsync_ThrowsWhenCountIsNull()
        {
            CustomerDataClient client = new CustomerDataClient("http://myurl");

            Processor p = new Processor(client);

            try
            {
                List<Customer> c= p.GetYoungestCustomersAsync(0).Result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        //Sample test to show how mock can be used to replace the ICustomerDataClient with a mock
        [TestMethod]
        public void GetYoungestCustomersAsync_ReturnsValidResults()
        {
            MockDataClient client = new MockDataClient("http://myurl");

            Processor p = new Processor(client);

            try
            {
                List<Customer> c = p.GetYoungestCustomersAsync(2).Result;

                Assert.IsTrue(c.Count == 2);
                Assert.IsTrue(c.Contains(client.CustomerMap[1]));
                Assert.IsTrue(c.Contains(client.CustomerMap[2]));

                // make number invalid
                client.CustomerMap[1].Number = "555";

                c = p.GetYoungestCustomersAsync(2).Result;

                // result should contain 2 & 3 since 1 is invalid
                Assert.IsTrue(c.Count == 2);
                Assert.IsTrue(c.Contains(client.CustomerMap[2]));
                Assert.IsTrue(c.Contains(client.CustomerMap[3]));

            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        // Todo : Add more tests with different Mock data and test processor

    }
}
