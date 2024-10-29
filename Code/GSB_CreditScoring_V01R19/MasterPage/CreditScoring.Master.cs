using System;
using System.Data;
using GSB.Class;

namespace GSB.MasterPage
{
    public partial class CreditScoring : System.Web.UI.MasterPage
    {
        //SQLToDataTable comm = new SQLToDataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["Credit_Scoring_GSB"] == null)
                Response.Redirect("~/SignIn.aspx");
            else
            {
                SQLToDataTable comm = new SQLToDataTable();
                try
                {
                    string UserID = string.Format("{0}", Request.Cookies["Credit_Scoring_GSB"]["UID"]);

                    string Query = "SELECT TOP 1 USER_PNAME + USER_FNAME + ' ' + USER_SNAME AS NAME " +
                                "FROM S_USER WHERE UID = '" + UserID + "' ";

                    DataTable dtMaster = comm.ExcuteSQL(Query);

                    string fullname = string.Format("{0}", dtMaster.Rows[0]["NAME"]);
                    userFirstname.Text = string.Format("{0}", fullname.ToString());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    comm.CloseConnection();
                }
                
            }
        }
    }
}