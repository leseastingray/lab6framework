using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ToolsCSharp;
using EventPropsClasses;
using CustomerDB = EventDBClasses.CustomerSQLDB;
using System.Data;

namespace EventClasses
{
    public class Customer : BaseBusiness
    {
        // constructors
        /// <summary>
        /// Default constructor - does nothing.
        /// </summary>
        public Customer() : base()
        {
        }

        /// <summary>
        /// One arg constructor.
        /// Calls methods SetUp(), SetRequiredRules(), 
        /// SetDefaultProperties() and BaseBusiness one arg constructor.
        /// </summary>
        /// <param name="cnString">DB connection string.
        /// This value is passed to the one arg BaseBusiness constructor, 
        /// which assigns the it to the protected member mConnectionString.</param>
        public Customer(string cnString)
            : base(cnString)
        {
        }

        /// <summary>
        /// Two arg constructor.
        /// Calls methods SetUp() and Load().
        /// </summary>
        /// <param name="key">ID number of a record in the database.
        /// Sent as an arg to Load() to set values of record to properties of an 
        /// object.</param>
        /// <param name="cnString">DB connection string.
        /// This value is passed to the one arg BaseBusiness constructor, 
        /// which assigns the it to the protected member mConnectionString.</param>
        public Customer(int key, string cnString)
            : base(key, cnString)
        {
        }
        // this constructor requires a configuration file
        public Customer(int key)
            : base(key)
        {
        }

        // *** I added these 2 so that I could create a 
        // business object from a properties object
        // I added the new constructors to the base class
        public Customer(CustomerProps props)
            : base(props)
        {
        }

        public Customer(CustomerProps props, string cnString)
            : base(props, cnString)
        {
        }
        // Interface implementations
        public override object GetList()
        {
            List<Customer> customers = new List<Customer>();
            List<CustomerProps> props = new List<CustomerProps>();


            props = (List<CustomerProps>)mdbReadable.RetrieveAll(props.GetType());
            foreach (CustomerProps prop in props)
            {
                Customer c = new Customer(prop, this.mConnectionString);
                customers.Add(c);
            }

            return customers;
        }
        /// <summary>
        /// Deletes the customer
        /// </summary>
        public static void Delete(CustomerProps c)
        {
            CustomerDB db = new CustomerDB();
            db.Delete(c);
        }
        /// <summary>
        /// defaults
        /// </summary>		
        protected override void SetDefaultProperties()
        {
        }
        /// <summary>
        /// Sets required fields for a record.
        /// </summary>
        protected override void SetRequiredRules()
        {
            mRules.RuleBroken("name", true);
            mRules.RuleBroken("address", true);
            mRules.RuleBroken("city", true);
            mRules.RuleBroken("state", true);
            mRules.RuleBroken("zipcode", true);
        }
        /// <summary>
        /// Instantiates mProps and mOldProps as new Props objects.
        /// Instantiates mbdReadable and mdbWriteable as new DB objects.
        /// </summary>
        protected override void SetUp()
        {
            mProps = new CustomerProps();
            mOldProps = new CustomerProps();

            if (this.mConnectionString == "")
            {
                mdbReadable = new CustomerDB();
                mdbWriteable = new CustomerDB();
            }

            else
            {
                mdbReadable = new CustomerDB(this.mConnectionString);
                mdbWriteable = new CustomerDB(this.mConnectionString);
            }
        }
        /// <summary>
        /// Read-only ID property.
        /// </summary>
        public int ID
        {
            get
            {
                return ((CustomerProps)mProps).ID;
            }
        }
        /// <summary>
        /// Read/Write property. 
        /// </summary>
        /// <exception cref="ArgumentException">
        /// 
        /// </exception>
        public string Name
        {
            get
            {
                return ((CustomerProps)mProps).name;
            }

            set
            {
                // if the new value does not equal the name
                if (!(value == ((CustomerProps)mProps).name))
                {
                    // if the length is greater than or equal to 1 and less than or equal to 100
                    if (value.Length >= 1 && value.Length <= 100)
                    {
                        // the name rule is not broken
                        // set the name property to the new value
                        mRules.RuleBroken("name", false);
                        ((CustomerProps)mProps).name = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Name must be between 1 and 100 characters, inclusive");
                    }
                }
            }
        }
        public string Address
        {
            get
            {
                return ((CustomerProps)mProps).address;
            }

            set
            {
                // if the new value does not equal the address
                if (!(value == ((CustomerProps)mProps).address))
                {
                    // if the length is greater than or equal to 1 and less than or equal to 50
                    if (value.Length >= 1 && value.Length <= 50)
                    {
                        // the address rule is not broken
                        // set the address property to the new value
                        mRules.RuleBroken("address", false);
                        ((CustomerProps)mProps).address = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Address must be between 1 and 50 characters, inclusive");
                    }
                }
            }
        }
        public string City
        {
            get
            {
                return ((CustomerProps)mProps).city;
            }

            set
            {
                // if the new value does not equal the city
                if (!(value == ((CustomerProps)mProps).city))
                {
                    // if the length is greater than or equal to 1 and less than or equal to 20
                    if (value.Length >= 1 && value.Length <= 20)
                    {
                        // the city rule is not broken
                        // set the city property to the new value
                        mRules.RuleBroken("city", false);
                        ((CustomerProps)mProps).city = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Name must be between 1 and 20 characters, inclusive");
                    }
                }
            }
        }
        public string State
        {
            get
            {
                return ((CustomerProps)mProps).state;
            }

            set
            {
                // if the new value does not equal the state
                if (!(value == ((CustomerProps)mProps).state))
                {
                    // if the length is 2
                    if (value.Length == 2)
                    {
                        // the state rule is not broken
                        // set the state property to the new value
                        mRules.RuleBroken("state", false);
                        ((CustomerProps)mProps).state = value.ToUpper();
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("State must be 2 characters");
                    }
                }
            }
        }
       public string Zipcode
        {
            get
            {
                return ((CustomerProps)mProps).zipcode;
            }

            set
            {
                // if the new value does not equal the zipcode
                if (!(value == ((CustomerProps)mProps).zipcode))
                {
                    // if the length is greater than or equal to 1 and less than or equal to 15
                    if (value.Length >= 1 && value.Length <= 15)
                    {
                        // the zipcode rule is not broken
                        // set the zipcode property to the new value
                        mRules.RuleBroken("zipcode", false);
                        ((CustomerProps)mProps).zipcode = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Zip Code must be between 1 and 15 characters, inclusive");
                    }
                }
            }
        }
    }
}
