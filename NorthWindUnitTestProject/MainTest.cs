using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthWindCoreLibrary.Classes.North.Classes;
using NorthWindCoreLibrary.Data;
using NorthWindCoreLibrary.Models;
using NorthWindUnitTestProject.Base;

namespace NorthWindUnitTestProject
{
    [TestClass]
    public partial class MainTest : TestBase
    {
        [TestMethod]
        [TestTraits(Trait.EntityFrameworkCoreReadProjectionTest)]
        public async Task GetCustomersTask()
        {
            List<CustomerEntity> customerEntitiesList = await AllCustomersToJsonAsync();
            Assert.AreEqual(customerEntitiesList.Count, 91, "Expected 91 Customers");
        }

        [TestMethod]
        [TestTraits(Trait.DataProviderTest)]
        public void DataProviderTestConnection()
        {
            var (success, _) = TestConnection();
            Assert.IsTrue(success,"Expected to successfully connect to database");
        }

        [TestMethod]
        [TestTraits(Trait.DataProviderTest)]
        public void DataProviderReadCategories()
        {
            var (success, categories, _) = ReadCategoriesList();
            Assert.IsTrue(success);
            Assert.AreEqual(categories.Count,8,"Expected 8 category records");
            
        }



    }
}
