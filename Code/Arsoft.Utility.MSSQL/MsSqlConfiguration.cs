using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Arsoft.Utility.MSSQL
{
    public class MsSqlConfiguration
    {
        #region "Attributes"
            private static string sqlServerConnection;
        #endregion

        #region "Methods"

        public static string ConnectionString
        {
            get { return sqlServerConnection; }
            set { sqlServerConnection = value; }
        }

        public static string ConfigurationConnectionString(string connectionName)
        {
            //if (( sqlServerConnection == null ))
            //{
            //    sqlServerConnection = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            //    //sqlServerConnection = ConfigurationManager.AppSettings[connectionName].ToString( );
            //}
            sqlServerConnection = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            return sqlServerConnection;
        }

        #endregion

    }
}
