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
    public class ProductTests
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
        // Constructor Tests
        [Test]
        public void TestNewProductConstructor1()
        {
            // not in Data Store - no id
            Product p = new Product(dataSource);
            Console.WriteLine(p.ToString());
            Assert.Greater(p.ToString().Length, 1);
        }
        [Test]
        public void TestNewProductConstructor2()
        {
            // in Data Store, using key
            // 6    CS10    Murach's C# 2010   56.50   5136 
            Product p = new Product(6, dataSource);
            Console.WriteLine(p.ToString());
            Assert.Greater(p.ToString().Length, 1);
            Assert.AreEqual("CS10", p.ProductCode);
        }
        [Test]
        public void TestNewProductConstructor3()
        {
            // using ProductProps and connection string
            ProductSQLDB db = new ProductSQLDB(dataSource);
            ProductProps pProps = (ProductProps)db.Retrieve(14);
            // 14    SQ12    Murach's SQL Server 2012     57.50   2465           
            Product p = new Product(pProps, dataSource);
            Console.WriteLine(p.ToString());
            Assert.Greater(p.ToString().Length, 1);
            Assert.AreEqual("Murach's SQL Server 2012", p.Description);
            Assert.AreEqual(57.50M, p.UnitPrice);
        }
        [Test]
        public void TestRetrieveFromDataStoreContructor()
        {
            // retrieves from Data Store
            Product p = new Product(6, dataSource);
            Assert.AreEqual(p.ID, 6);
            Assert.AreEqual(5136, p.OnHandQuantity);
            Console.WriteLine(p.ToString());
        }

        [Test]
        public void TestSaveToDataStore()
        {
            ProductSQLDB db = new ProductSQLDB(dataSource);
            Product p = new Product(dataSource);
            p.ProductCode = "FS10";
            p.Description = "Murach's F# 2010";
            p.UnitPrice = 45.00M;
            p.OnHandQuantity = 99;
            p.Save();

            Assert.AreEqual("Murach's F# 2010", p.Description);
            Assert.AreEqual(99, p.OnHandQuantity);
        }

        [Test]
        public void TestUpdate()
        {
            // 6    CS10    Murach's C# 2010   56.50   80020  5136          
            Product p = new Product(6, dataSource);
            p.Description = "New&Updated Murach's C# 2015";
            p.UnitPrice = 62.50M;
            p.Save();

            p = new Product(6, dataSource);
            Assert.AreEqual("New&Updated Murach's C# 2015", p.Description);
            Assert.AreEqual(62.50M, p.UnitPrice);
            Assert.AreEqual("CS10", p.ProductCode);
        }

        [Test]
        public void TestDelete()
        {
            Product p = new Product(11, dataSource);
            p.Delete();
            p.Save();
            Assert.Throws<Exception>(() => new Product(11, dataSource));
        }

        [Test]
        public void TestGetList()
        {
            // 14    SQ12    Murach's SQL Server 2012     57.50   2465       
            Product p = new Product(dataSource);
            List<Product> products = (List<Product>)p.GetList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual(14, products[13].ID);
            Assert.AreEqual("Murach's SQL Server 2012", products[13].Description);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - product code, description, unit price, on-hand quantity must be provided
            Product p = new Product(dataSource);
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - product code, description, unit price, on-hand quantity must be provided
            Product p = new Product(dataSource);
            Assert.Throws<Exception>(() => p.Save());
            p.ProductCode = "FS2010";
            Assert.Throws<Exception>(() => p.Save());
            p.Description = "Murach's F# 2010";
            Assert.Throws<Exception>(() => p.Save());
            p.UnitPrice = 54.50M;
            Assert.Throws<Exception>(() => p.Save());
        }
        // Invalid Property Settings Tests
        [Test]
        public void TestInvalidPropertyCodeSet()
        {
            Product p = new Product(dataSource);
            Assert.Throws<ArgumentException>(() =>
                p.ProductCode = "abcdefghijklmnopqrstuvwxyz");
        }
        [Test]
        public void TestInvalidPropertyDescriptionSet()
        {
            Product p = new Product(dataSource);
            Assert.Throws<ArgumentException>(() => p.Description = "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz");
        }
        [Test]
        public void TestInvalidPropertyUnitPriceSet()
        {
            Product p = new Product(dataSource);
            Assert.Throws<ArgumentException>(() => p.UnitPrice = -1.50M);
        }
        [Test]
        public void TestInvalidPropertyOnHandQuantSet()
        {
            Product p = new Product(dataSource);
            Assert.Throws<ArgumentException>(() => p.OnHandQuantity = -50);
        }
    }
}