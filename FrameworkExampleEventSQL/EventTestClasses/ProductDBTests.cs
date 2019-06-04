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
    public class ProductDBTests
    {
        //private string folder = "C:\\Courses\\CS234CSharp\\Demos\\FrameworkExampleEvent\\Files\\";
        // *** changed the name AND folder to db connection string
        private string dataSource = "Data Source=1912851-C20251;Initial Catalog=MMABooksUpdated;Integrated Security=True";

        [SetUp]
        public void TestResetDatabase()
        {
            ProductSQLDB db = new ProductSQLDB(dataSource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestRetrieve()
        {
            ProductSQLDB db = new ProductSQLDB(dataSource);
            // remember to cast!
            ProductProps props = (ProductProps)db.Retrieve(6);
            // 6    CS10    Murach's C# 2010   56.50   80020  5136
            Assert.AreEqual(6, props.ID);
            Assert.AreEqual("Murach's C# 2010", props.description);
            Assert.AreEqual(56.50, props.unitPrice);
        }

        [Test]
        public void TestRetrieveAll()
        {
            ProductSQLDB db = new ProductSQLDB(dataSource);
            // create and fill list of ProductProps
            List<ProductProps> cProps = (List<ProductProps>)db.RetrieveAll(db.GetType());

            // cProps count should be equal to 16 (length of MMABooksUpdated Products table)
            Assert.AreEqual(16, cProps.Count);
        }

        [Test]
        public void TestCreateProduct()
        {
            ProductSQLDB db = new ProductSQLDB(dataSource);
            // declare and instantiate new ProductProps p
            ProductProps p = new ProductProps();
            p.prodCode = "FS30";
            p.description = "Murach's F# 2015";
            p.unitPrice = 50.00M;
            p.onHandQuantity = 100;

            // Create record for p in the database
            db.Create(p);

            // Retrieve Product p from the database and store in variable pCreated
            ProductProps pCreated = (ProductProps)db.Retrieve(p.ID);

            Assert.AreEqual("FS30", pCreated.prodCode);
            Assert.AreEqual(100, pCreated.onHandQuantity);
        }

        [Test]
        public void TestDeleteProduct()
        {
            ProductSQLDB db = new ProductSQLDB(dataSource);
            ProductProps props = (ProductProps)db.Retrieve(14);
            //14    SQ12    Murach's SQL Server 2012     57.50   2465

            // delete ProductProps props from the database
            db.Delete(props);

            // attempting to retrieve props from the database should result in exception throw
            Assert.Throws<Exception>(() => db.Retrieve(14));
        }

        [Test]
        public void TestUpdate()
        {
            ProductSQLDB db = new ProductSQLDB(dataSource);
            ProductProps props = (ProductProps)db.Retrieve(14);

            props.unitPrice = 55.50M;
            bool ok = db.Update(props);

            ProductProps propsUpdated = (ProductProps)db.Retrieve(14);
            //14    SQ12    Murach's SQL Server 2012     57.50   2465

            Assert.AreEqual("SQ12", props.prodCode);
            Assert.AreEqual(55.50M, props.unitPrice);
        }
    }
}