using DDTek.Oracle;
using IComm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Xml;

namespace Framework.Core.Support
{

    public class GetDataBaseInfo
    {
        private string _dbConnection;
        private Dictionary<string, object> _dbPermeterTypes = new Dictionary<string, object>();
        private DbTypeName _dbTypeName;
        public static readonly GetDataBaseInfo Instance = new GetDataBaseInfo();

        private GetDataBaseInfo()
        {
            this.BuilderDBInfoName();
            this.FillDBPermeterTypes();
        }

        public void BuilderDBInfoName()
        {
            XmlDocument document = new XmlDocument();
                document.Load(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\DBConnection\dbconfig.xml");
            
            XmlNodeList list = document.SelectNodes("//DBInfo");
            string content = "";
            string innerText = "";
            if (list != null)
            {
                foreach (XmlNode node in list)
                {
                    content = node.SelectSingleNode("ConnectionString").InnerText;
                    innerText = node.SelectSingleNode("Provider").InnerText;
                }
                IEncrypt encrypt = CreateInstanctFactory.CreateIEncrypt("Comm", "Comm.DESEncrypt");
                if (encrypt != null)
                {
                    content = encrypt.Decrypt(content);
                }
            }
            DbTypeName sQLServer = DbTypeName.SQLServer;
            if ((innerText.IndexOf("System.Data.SqlClient") != -1) || (content.IndexOf("Provider=System.Data.SqlClient") != -1))
            {
                sQLServer = DbTypeName.SQLServer;
            }
            else if ((innerText.IndexOf("DDTek") != -1))
            {
                sQLServer = DbTypeName.DDTek;
            }
            else if ((innerText.IndexOf("System.Data.OracleClient") != -1) || (content.IndexOf("Provider=System.Data.OracleClient") != -1))
            {
                sQLServer = DbTypeName.Oracle;
            }
            else if ((innerText.IndexOf("Microsoft.Jet") != -1) || (content.IndexOf("Provider=Microsoft.Jet") != -1))
            {
                sQLServer = DbTypeName.Access;
            }
            else if ((innerText.IndexOf("Microsoft.Jet") != -1) || (content.IndexOf("Provider=Microsoft.Jet") != -1))
            {
                sQLServer = DbTypeName.Access;
            }
            this._dbTypeName = sQLServer;
            this.DbConnection = content;
        }

        private void FillDBPermeterTypes()
        {
            if ((this._dbPermeterTypes == null) || (this._dbPermeterTypes.Count == 0))
            {
                if (this._dbTypeName == DbTypeName.SQLServer)
                {
                    this._dbPermeterTypes.Add("System.Boolean", SqlDbType.Bit);
                    this._dbPermeterTypes.Add("System.Byte[]", SqlDbType.Binary);
                    this._dbPermeterTypes.Add("System.DateTime", SqlDbType.DateTime);
                    this._dbPermeterTypes.Add("System.Decimal", SqlDbType.Decimal);
                    this._dbPermeterTypes.Add("System.Double", SqlDbType.Decimal);
                    this._dbPermeterTypes.Add("System.Guid", SqlDbType.UniqueIdentifier);
                    this._dbPermeterTypes.Add("System.Int32", SqlDbType.Int);
                    this._dbPermeterTypes.Add("System.String", SqlDbType.NVarChar);
                }
                else if (this._dbTypeName == DbTypeName.Oracle)
                {
                    this._dbPermeterTypes.Add("System.Byte[]", OracleType.BFile);
                    this._dbPermeterTypes.Add("System.DateTime", OracleType.DateTime);
                    this._dbPermeterTypes.Add("System.Decimal", OracleType.Number);
                    this._dbPermeterTypes.Add("System.Double", OracleType.Double);
                    this._dbPermeterTypes.Add("System.Int32", OracleType.Int32);
                    this._dbPermeterTypes.Add("System.String", OracleType.NVarChar);
                }
                else if (this._dbTypeName == DbTypeName.DDTek)
                {
                    this._dbPermeterTypes.Add("System.Byte[]", OracleDbType.Bfile);
                    this._dbPermeterTypes.Add("System.DateTime", OracleDbType.Date);
                    this._dbPermeterTypes.Add("System.Decimal", OracleDbType.Number);
                    this._dbPermeterTypes.Add("System.Double", OracleDbType.Double);
                    this._dbPermeterTypes.Add("System.Int32", OracleDbType.Int32);
                    this._dbPermeterTypes.Add("System.String", OracleDbType.NVarChar);
                }
                else
                {
                    this._dbPermeterTypes.Add("System.Boolean", OleDbType.Boolean);
                    this._dbPermeterTypes.Add("System.Byte[]", OleDbType.Binary);
                    this._dbPermeterTypes.Add("System.DateTime", OleDbType.Date);
                    this._dbPermeterTypes.Add("System.Decimal", OleDbType.Decimal);
                    this._dbPermeterTypes.Add("System.Double", OleDbType.Double);
                    this._dbPermeterTypes.Add("System.Guid", OleDbType.Guid);
                    this._dbPermeterTypes.Add("System.Int32", OleDbType.Integer);
                    this._dbPermeterTypes.Add("System.String", OleDbType.VarChar);
                }
            }
        }

        public object GetDbPermeterTypeName(string propertyTypeName)
        {
            if (propertyTypeName.IndexOf("System.Nullable`1") != -1)
            {
                propertyTypeName = propertyTypeName.Substring(propertyTypeName.IndexOf("[[") + 2);
                propertyTypeName = propertyTypeName.Substring(0, propertyTypeName.IndexOf(","));
            }
            if (this._dbPermeterTypes.ContainsKey(propertyTypeName))
            {
                return this._dbPermeterTypes[propertyTypeName];
            }
            return null;
        }

        public string DbConnection
        {
            get
            {
                return this._dbConnection;
            }
            private set
            {
                this._dbConnection = value;
            }
        }
    }
}

