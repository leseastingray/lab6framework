using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

using EventClasses;
using EventPropsClasses;
using EventDBClasses;
using ToolsCSharp;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

using System.Data;
using System.Data.SqlClient;

using DBCommand = System.Data.SqlClient.SqlCommand;

namespace EventTestClasses
{
    [TestFixture]
    public class CustomerTests
    {
        //private string folder = "C:\\Courses\\CS234CSharp\\Demos\\FrameworkExampleEvent\\Files\\";
        // *** changed the name AND folder to db connection string
        private string dataSource = "Data Source=1912851-C20251;Initial Catalog=MMABooksUpdated;Integrated Security=True";

        [SetUp]
        public void TestResetDatabase()
        {
            CustomerSQLDB db = new CustomerSQLDB(dataSource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestNewCustomerConstructor()
        {
            // not in Data Store - no id
            Customer c = new Customer(dataSource);
            Console.WriteLine(c.ToString());
            Assert.Greater(c.ToString().Length, 1);
        }

        [Test]
        public void TestRetrieveFromDataStoreContructor()
        {
            // retrieves from Data Store
            Customer c = new Customer(1, dataSource);
            Assert.AreEqual(c.ID, 1);
            Assert.AreEqual("Molunguri, A", c.Name);
            Console.WriteLine(c.ToString());
        }

        [Test]
        public void TestSaveToDataStore()
        {
            Customer c = new Customer(dataSource);
            c.Name = "Krait, Sea";
            c.Address = "1 Large Ocean";
            c.City = "Tropics";
            c.State = "HI";
            c.Zipcode = "00000";
            c.Save();
            Assert.AreEqual("1 Large Ocean", c.Address);
            Assert.AreEqual("HI", c.State);
        }

        [Test]
        public void TestUpdate()
        {
            // 7  Lutonsky, Christopher  293 Old Holcomb Bridge Way  Woodland Hills CA 91365           
            Customer c = new Customer(7, dataSource);
            c.Address = "New Address";
            c.City = "New City";
            c.Save();

            c = new Customer(7, dataSource);
            Assert.AreEqual("New Address", c.Address);
            Assert.AreEqual("New City", c.City);
            Assert.AreEqual("CA", c.State);
        }

        [Test]
        public void TestDelete()
        {
            Customer c = new Customer(2, dataSource);
            c.Delete();
            c.Save();
            Assert.Throws<Exception>(() => new Customer(2, dataSource));
        }

        [Test]
        public void TestGetList()
        {
            // 1  Molunguri, A  1108 Johanna Bay Drive  Birmingham AL 35216-6909 
            Customer c = new Customer(dataSource);
            List<Customer> customers = (List<Customer>)c.GetList();
            Assert.AreEqual(696, customers.Count);
            Assert.AreEqual(1, customers[0].ID);
            Assert.AreEqual("Molunguri, A", customers[0].Name);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - name, address, city, state, and zipcode must be provided
            Customer c = new Customer(dataSource);
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - name, address, city, state, and zipcode must be provided
            Customer c = new Customer(dataSource);
            Assert.Throws<Exception>(() => c.Save());
            c.Name = "Ray, Manta";
            Assert.Throws<Exception>(() => c.Save());
            c.Address = "test ocean";
            Assert.Throws<Exception>(() => c.Save());
            c.City = "coral reef";
            Assert.Throws<Exception>(() => c.Save());
            c.State = "HI";
            Assert.Throws<Exception>(() => c.Save());
        }
        // Invalid Property Settings Tests
        [Test]
        public void TestInvalidPropertyStateSet()
        {
            Customer c = new Customer(dataSource);
            Assert.Throws<ArgumentException>(() => c.State = "Hawaii");
        }
        [Test]
        public void TestInvalidPropertyZipcodeSet()
        {
            Customer c = new Customer(dataSource);
            Assert.Throws<ArgumentException>(() => c.Zipcode = "1234567890123456789");
        }
        // *** I added this
        [Test]
        public void TestConcurrencyIssue()
        {
            Event e1 = new Event(1, dataSource);
            Event e2 = new Event(1, dataSource);

            e1.Title = "Updated this first";
            e1.Save();

            e2.Title = "Updated this second";
            Assert.Throws<Exception>(() => e2.Save());
        }
    }
}
