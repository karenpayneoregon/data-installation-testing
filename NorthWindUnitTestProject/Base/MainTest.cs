using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; // for BuildConnection
using NorthWindCoreLibrary.Classes.North.Classes;
using NorthWindCoreLibrary.Data;
using NorthWindCoreLibrary.Models;


// ReSharper disable once CheckNamespace - do not change
namespace NorthWindUnitTestProject
{
    public partial class MainTest
    {
        protected static string BuildConnection()
        {

            var configuration = (new ConfigurationBuilder()).AddJsonFile("appsettings.json", true, true).Build();

            var sections = configuration.GetSection("database").GetChildren().ToList();

            return $"Data Source={sections[1].Value};Initial Catalog={sections[0].Value};Integrated Security={sections[2].Value}";

        }

        protected static string ConnectionString => BuildConnection();

        /// <summary>
        /// Perform any initialize for the class
        /// </summary>
        /// <param name="testContext"></param>
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            TestResults = new List<TestContext>();
        }

        public static async Task<List<CustomerEntity>> AllCustomersToJsonAsync()
        {

            return await Task.Run(async () =>
            {
                await using var context = new NorthwindContext();
                List<CustomerEntity> customerItemsList = await context.Customers
                    .Include(customer => customer.Contact)
                    .Select(Customers.Projection)
                    .ToListAsync();

                return customerItemsList.OrderBy((customer) => customer.CompanyName).ToList();
            });
        }

        public static (bool success, Exception exception) TestConnection()
        {
            try
            {
                using var cn = new SqlConnection() { ConnectionString = ConnectionString };
                cn.Open();
                return (true, null);
            }
            catch (Exception exception)
            {
                return (false, exception);
            }
        }

        public static (bool success, List<Categories> categories, Exception exception) ReadCategoriesList()
        {
            List<Categories> categoriesList = new List<Categories>();
            try
            {

                var selectStatement =
                    "SELECT CategoryID, CategoryName, Picture " +
                    "FROM Categories AS C " +
                    "WHERE Picture IS NOT NULL;";

                using var cn = new SqlConnection() { ConnectionString = ConnectionString };
                using var cmd = new SqlCommand() { Connection = cn, CommandText = selectStatement };
                cn.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    var category = new Categories
                    {
                        CategoryId = reader.GetInt32(0),
                        CategoryName = reader.GetString(1),
                        Picture = (byte[])reader["Picture"]
                    };

                    categoriesList.Add(category);

                }

                return (true,categoriesList,null);
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"{exception.Message}");
                return (false, null, exception);
            }

        }
      
    }
}
