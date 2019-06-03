using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using ToolsCSharp;
using DBDataReader = System.Data.SqlClient.SqlDataReader;

namespace EventPropsClasses
{
    [Serializable()]
    public class ProductProps : IBaseProps
    {

        #region instance variables - all the fields in the current database
        /// <summary>
        /// 
        /// </summary>
        public int ID = Int32.MinValue;

        /// <summary>
        /// 
        /// </summary>
        public string prodCode = "";

        /// <summary>
        /// 
        /// </summary>
        public string description = "";

        /// <summary>
        /// 
        /// </summary>
        public decimal unitPrice = Decimal.MinValue;

        /// <summary>
        /// 
        /// </summary>
        public int onHandQuantity = Int32.MinValue;

        /// <summary>
        /// ConcurrencyID. See main docs, don't manipulate directly
        /// </summary>
        public int ConcurrencyID = 0;
        #endregion

        #region constructor
        /// <summary>
        /// Constructor. This object should only be instantiated by Customer, not used directly.
        /// </summary>
        public ProductProps()
        {
        }

        #endregion

        #region BaseProps Members
        /// <summary>
        /// Serializes this props object to XML, and writes the key-value pairs to a string.
        /// It would be possible to make this return a json instead of an xml
        /// </summary>
        /// <returns>String containing key-value pairs</returns>	
        public string GetState()
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, this);
            return writer.GetStringBuilder().ToString();
        }

        // I don't always want to generate xml in the db class so the 
        // props class can read in from xml
        public void SetState(DBDataReader dr)
        {
            this.ID = (Int32)dr["ProductID"];
            // notice the added trim! important
            this.prodCode = ((string)dr["ProductCode"]).Trim();
            this.description = (string)dr["Description"];
            this.unitPrice = (decimal)dr["UnitPrice"];
            this.onHandQuantity = (int)dr["OnHandQuantity"];
            this.ConcurrencyID = (Int32)dr["ConcurrencyID"];
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetState(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            StringReader reader = new StringReader(xml);
            ProductProps p = (ProductProps)serializer.Deserialize(reader);
            this.ID = p.ID;
            this.prodCode = p.prodCode;
            this.description = p.description;
            this.unitPrice = p.unitPrice;
            this.onHandQuantity = p.onHandQuantity;
            this.ConcurrencyID = p.ConcurrencyID;
        }
        #endregion

        #region ICloneable Members
        /// <summary>
        /// Clones this object. This is needed because IBaseProps
        /// has ICloneable, so it must be implemented in child class
        /// </summary>
        /// <returns>A clone of this object.</returns>
        public Object Clone()
        {
            ProductProps p = new ProductProps();
            p.ID = this.ID;
            p.prodCode = this.prodCode;
            p.description = this.description;
            p.unitPrice = this.unitPrice;
            p.onHandQuantity = this.onHandQuantity;
            p.ConcurrencyID = this.ConcurrencyID;
            return p;
        }
        #endregion

    }
}
