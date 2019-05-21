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
    public class CustomerPropsTests
    {
        // declare field variables
        CustomerProps props1;
        CustomerProps props2;

        [SetUp]
        public void TestSetUp()
        {
            props1 = new CustomerProps();
            props1.ID = 1;
            props1.name = "Donald";
            props1.address = "Main Street";
            props1.city = "Orlando";
            props1.state = "FL";
            props1.zipcode = "33333";
            props1.ConcurrencyID = 12;

            props2 = new CustomerProps();
        }

        [Test]
        public void TestClone()
        {
            // must cast to CustomerProps because Clone() returns an object
            CustomerProps props2 = (CustomerProps)props1.Clone();

            Assert.NotNull(props2);

            // change props1 name to "Goofy"
            props1.name = "Goofy";

            Assert.AreNotEqual(props1.name, props2.name);
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

            Assert.AreEqual(props1.name, props2.name);
            Assert.AreEqual(props1.address, props2.address);
        }
    }
}
