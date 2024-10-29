using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Arsoft.Utility.MSSQL;

namespace GSB.MasterPage
{
    public partial class Login : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MsSqlConfiguration.ConfigurationConnectionString("GSBSQLServer");
        }
    }
}