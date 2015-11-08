using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppSheetProject;

namespace AppSheetTestProject
{
    [TestClass]
    public class UtilsTest
    {
#region PhoneNo Validation Tests

        // These are some sample tests for validating phone numbers
        [TestMethod]
        public void ValidatePhoneNumberWithoutSpaces()
        {
            Assert.IsTrue(Utils.IsValidPhoneNo("5555555555"));

        }

        [TestMethod]
        public void ValidatePhoneNumberWithSpaces()
        {
            Assert.IsTrue(Utils.IsValidPhoneNo("555 555 5555"));
            Assert.IsTrue(Utils.IsValidPhoneNo("555 5555555"));
        }

        [TestMethod]
        public void PhoneNumberWithLessThan10DigitsIsInvalid()
        {
            Assert.IsFalse(Utils.IsValidPhoneNo(""));
            Assert.IsFalse(Utils.IsValidPhoneNo("555 55 5555"));
            Assert.IsFalse(Utils.IsValidPhoneNo("555555555"));
        }

        // Todo, add more tests for validating phone numbers

#endregion PhoneNo 

#region ReadConfigSetting Tests
        // same read setting test
        [TestMethod]
        public void VerifyUtilReadsValidAppSettings()
        {
            Assert.AreEqual(Utils.ReadSetting("serviceUrl"), "https://mytestendpoint/sample/");
        }

        [TestMethod]
        public void VerifyUtilReturnsNullWhenKeyNotPresent()
        {
            Assert.IsNull(Utils.ReadSetting("keyNotPresent"));
        }
        // Todo, add more tests

#endregion
    }
}
