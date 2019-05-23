using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventPropsClasses;
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
            EventSQLDB db = new EventSQLDB(dataSource);
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
