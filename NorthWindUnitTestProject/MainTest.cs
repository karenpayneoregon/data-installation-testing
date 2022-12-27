
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthWindCoreLibrary.Classes;
using NorthWindCoreLibrary.Classes.North.Classes;
using NorthWindCoreLibrary.Data;
using NorthWindCoreLibrary.LanguageExtensions;
using NorthWindCoreLibrary.Models;
using NorthWindUnitTestProject.Base;

namespace NorthWindUnitTestProject
{
    [TestClass]
    public partial class MainTest : TestBase
    {
        /// <summary>
        /// Inspect connection string in appsettings.json
        /// </summary>
        [TestMethod]
        [TestTraits(Trait.ConnectionsTest)]
        public void A00_ViewConnection()
        {

            Debug.WriteLine(BuildConnection());
        }

        /// <summary>
        /// A_ Ensures 1st run
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [TestTraits(Trait.ConnectionsTest)]
        public async Task A01_TestConnectionTask()
        {
            await using var context = new NorthwindContext();
            Assert.IsTrue(await context.TestConnection(),"Connection failed");
        }

        /// <summary>
        /// Do nothing other then wake-up EF
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        [TestTraits(Trait.WarmupEntityFramework)]
        public async Task A02_ConnectionWarmup()
        {
            await using var context = new NorthwindContext();
            Assert.IsTrue(await context.Customers.CountAsync() >0,"Warmup failed");
        }


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

        [TestMethod]
        [TestTraits(Trait.PlaceHolder)]
        public async Task ProductsGroupBy()
        {
            var results = await OrderOperations.GetEmployeesTask();
            IGrouping<int, Employees> grouped = OrderOperations.EmployeeMostOrders(results);

            for (int index = 0; index < grouped.Count(); index++)
            {
                Console.WriteLine(grouped.Key);
            }

        }







    }
}
