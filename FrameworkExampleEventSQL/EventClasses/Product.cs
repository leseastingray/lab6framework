using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ToolsCSharp;
using EventPropsClasses;
using ProductDB = EventDBClasses.ProductSQLDB;
using System.Data;

namespace EventClasses
{
    public class Product : BaseBusiness
    {
        // constructors
        /// <summary>
        /// Default constructor - does nothing.
        /// </summary>
        public Product() : base()
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
        public Product(string cnString)
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
        public Product(int key, string cnString)
            : base(key, cnString)
        {
        }
        // this constructor requires a configuration file
        public Product(int key)
            : base(key)
        {
        }

        // *** I added these 2 so that I could create a 
        // business object from a properties object
        // I added the new constructors to the base class
        public Product(ProductProps props)
            : base(props)
        {
        }

        public Product(ProductProps props, string cnString)
            : base(props, cnString)
        {
        }
        // Interface implementations
        public override object GetList()
        {
            List<Product> products = new List<Product>();
            List<ProductProps> props = new List<ProductProps>();


            props = (List<ProductProps>)mdbReadable.RetrieveAll(props.GetType());
            foreach (ProductProps prop in props)
            {
                Product p = new Product(prop, this.mConnectionString);
                products.Add(p);
            }

            return products;
        }
        /// <summary>
        /// Deletes the product
        /// </summary>
        public static void Delete(ProductProps p)
        {
            ProductDB db = new ProductDB();
            db.Delete(p);
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
            mRules.RuleBroken("prodCode", true);
            mRules.RuleBroken("description", true);
            mRules.RuleBroken("unitPrice", true);
            mRules.RuleBroken("onHandQuantity", true);
        }
        /// <summary>
        /// Instantiates mProps and mOldProps as new Props objects.
        /// Instantiates mbdReadable and mdbWriteable as new DB objects.
        /// </summary>
        protected override void SetUp()
        {
            mProps = new ProductProps();
            mOldProps = new ProductProps();

            if (this.mConnectionString == "")
            {
                mdbReadable = new ProductDB();
                mdbWriteable = new ProductDB();
            }

            else
            {
                mdbReadable = new ProductDB(this.mConnectionString);
                mdbWriteable = new ProductDB(this.mConnectionString);
            }
        }
        /// <summary>
        /// Read-only ID property.
        /// </summary>
        public int ID
        {
            get
            {
                return ((ProductProps)mProps).ID;
            }
        }
        /// <summary>
        /// Read/Write property. 
        /// </summary>
        /// <exception cref="ArgumentException">
        /// 
        /// </exception>
        public string ProductCode
        {
            get
            {
                return ((ProductProps)mProps).prodCode;
            }

            set
            {
                // if the new value does not equal the product code
                if (!(value == ((ProductProps)mProps).prodCode))
                {
                    // if the length is greater than or equal to 1 and less than or equal to 10
                    if (value.Length >= 1 && value.Length <= 10)
                    {
                        // the prodCode rule is not broken
                        // set the prodCode property to the new value
                        mRules.RuleBroken("prodCode", false);
                        ((ProductProps)mProps).prodCode = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Name must be between 1 and 10 characters");
                    }
                }
            }
        }
        public string Description
        {
            get
            {
                return ((ProductProps)mProps).description;
            }

            set
            {
                // if the new value does not equal the description
                if (!(value == ((ProductProps)mProps).description))
                {
                    // if the length is greater than or equal to 1 and less than or equal to 50
                    if (value.Length >= 1 && value.Length <= 50)
                    {
                        // the description rule is not broken
                        // set the description property to the new value
                        mRules.RuleBroken("description", false);
                        ((ProductProps)mProps).description = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Description must be between 1 and 50 characters, inclusive");
                    }
                }
            }
        }
        public decimal UnitPrice
        {
            get
            {
                return ((ProductProps)mProps).unitPrice;
            }

            set
            {
                // if the new value does not equal the unit price
                if (!(value == ((ProductProps)mProps).unitPrice))
                {
                    // if the length is greater than or equal to 0
                    if (value >= 0)
                    {
                        // the unit price rule is not broken
                        // set the unitPrice property to the new value
                        mRules.RuleBroken("unitPrice", false);
                        ((ProductProps)mProps).unitPrice = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Unit Price must be a positive number");
                    }
                }
            }
        }
        public int OnHandQuantity
        {
            get
            {
                return ((ProductProps)mProps).onHandQuantity;
            }

            set
            {
                // if the new value does not equal the onhand quantity
                if (!(value == ((ProductProps)mProps).onHandQuantity))
                {
                    // if the length is greater than or equal to 0
                    if (value >= 0)
                    {
                        // the onhand quantity rule is not broken
                        // set the onHandQuantity property to the new value
                        mRules.RuleBroken("onHandQuantity", false);
                        ((ProductProps)mProps).onHandQuantity = value;
                        mIsDirty = true;
                    }

                    else
                    {
                        throw new ArgumentException("Onhand Quantity must be a positive number");
                    }
                }
            }
        }
    }
}