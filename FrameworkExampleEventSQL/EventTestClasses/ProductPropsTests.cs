using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventPropsClasses;
using NUnit.Framework;

namespace EventTestClasses
{
    [TestFixture]
    public class ProductPropsTests
    {
        // declare field variables
        ProductProps props1;
        ProductProps props2;

        [SetUp]
        public void TestSetUp()
        {
            props1 = new ProductProps();
            props1.ID = 1;
            props1.prodCode = "fgf5";
            props1.description = "South Sea Islands";
            props1.unitPrice = 30.00M;
            props1.onHandQuantity = 10;
            props1.ConcurrencyID = 4;

            props2 = new ProductProps();
        }

        [Test]
        public void TestClone()
        {
            // must cast to CustomerProps because Clone() returns an object
            ProductProps props2 = (ProductProps)props1.Clone();

            Assert.NotNull(props2);

            // change props1 description to Oceanic Languages"
            props1.description = "Oceanic Languages";

            Assert.AreNotEqual(props1.description, props2.description);
            Assert.AreNotSame(props1, props2);
        }
        [Test]
        public void TestGetState()
        {
            string xml = props1.GetState();
            Console.WriteLine(xml);
        }

        [Test]
        public void TestSetState()
        {
            string xml = props1.GetState();
            props2.SetState(xml);

            Assert.AreEqual(props1.description, props2.description);
            Assert.AreEqual(props1.unitPrice, props2.unitPrice);
        }
    }
}
