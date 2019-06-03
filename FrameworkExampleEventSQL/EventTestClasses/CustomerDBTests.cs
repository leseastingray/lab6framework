using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ToolsCSharp;

using EventDBClasses;
using EventPropsClasses;
using System.Data;
using DBCommand = System.Data.SqlClient.SqlCommand;

namespace EventTestClasses
{
    [TestFixture]
    public class CustomerDBTests
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
        public void TestRetrieve()
        {
            CustomerSQLDB db = new CustomerSQLDB(dataSource);
            // remember to cast!
            CustomerProps props = (CustomerProps)db.Retrieve(23);
            //23 Newlin, Sherman 2400 Bel Air, Apt. 345 Bronfield CO 80020
            Assert.AreEqual(23, props.ID);
            Assert.AreEqual("Newlin, Sherman", props.name);
        }

        [Test]
        public void TestRetrieveAll()
        {
            CustomerSQLDB db = new CustomerSQLDB(dataSource);
            // create and fill list of CustomerProps
            List<CustomerProps> cProps = (List<CustomerProps>)db.RetrieveAll(db.GetType());

            // cProps count should be equal to 696 (length of MMABooksUpdated Customers table)
            Assert.AreEqual(696, cProps.Count);
        }

        [Test]
        public void TestCreateCustomer()
        {
            CustomerSQLDB db = new CustomerSQLDB(dataSource);
            // declare and instantiate new CustomerProps c
            CustomerProps c = new CustomerProps();
            c.name = "John Rolfe";
            c.address = "1 Branch Hut";
            c.city = "Jamestown";
            c.state = "VA";
            c.zipcode = "23233";

            // Create record for c in the database
            db.Create(c);

            // Retrieve Customer c from the database and store in variable cCreated
            CustomerProps cCreated = (CustomerProps)db.Retrieve(c.ID);

            Assert.AreEqual("John Rolfe", cCreated.name);
            Assert.AreEqual("23233", cCreated.zipcode);
        }

        [Test]
        public void TestDeleteCustomer()
        {
            CustomerSQLDB db = new CustomerSQLDB(dataSource);
            CustomerProps props = (CustomerProps)db.Retrieve(7);
            //(7, N'Lutonsky, Christopher', N'293 Old Holcomb Bridge Way', N'Woodland Hills', N'CA', N'91365          ')   

            // delete CustomerProps props from the database
            db.Delete(props);

            // attempting to retrieve props from the database should result in exception throw
            Assert.Throws<Exception>(() => db.Retrieve(7));

        }

        [Test]
        public void TestUpdate()
        {
            CustomerSQLDB db = new CustomerSQLDB(dataSource);
            CustomerProps props = (CustomerProps)db.Retrieve(23);

            props.state = "NY";
            bool ok = db.Update(props);

            CustomerProps propsUpdated = (CustomerProps)db.Retrieve(23);
            //23 Newlin, Sherman 2400 Bel Air, Apt. 345 Bronfield CO 80020

            Assert.AreEqual("NY", props.state);
        }

    }
}
