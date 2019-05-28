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
        // dunno what is up with this one
        public void TestStaticDelete()
        {
            Event.Delete(2, dataSource);
            Assert.Throws<Exception>(() => new Event(2, dataSource));
        }

        [Test]
        public void TestStaticGetList()
        {
            List<Event> events = Event.GetList(dataSource);
            Assert.AreEqual(2, events.Count);
            Assert.AreEqual(1, events[0].ID);
            Assert.AreEqual("First Event", events[0].Title);
        }

        [Test]
        public void TestGetList()
        {
            Event e = new Event(dataSource);
            List<Event> events = (List<Event>)e.GetList();
            Assert.AreEqual(2, events.Count);
            Assert.AreEqual(1, events[0].ID);
            Assert.AreEqual("First Event", events[0].Title);
        }

        // *** I added this
        [Test]
        public void TestGetTable()
        {
            DataTable events = Event.GetTable(dataSource);
            Assert.AreEqual(events.Rows.Count, 2);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Event e = new Event(dataSource);
            Assert.Throws<Exception>(() => e.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Event e = new Event(dataSource);
            Assert.Throws<Exception>(() => e.Save());
            e.UserID = 1;
            Assert.Throws<Exception>(() => e.Save());
            e.Title = "this is a test";
            Assert.Throws<Exception>(() => e.Save());
        }

        [Test]
        public void TestInvalidPropertyUserIDSet()
        {
            Event e = new Event(dataSource);
            Assert.Throws<ArgumentOutOfRangeException>(() => e.UserID = -1);
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
