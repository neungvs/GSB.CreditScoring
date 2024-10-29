using System;
using GSB.Class;
using System.Data;
using System.Web;
using Arsoft.Utility;
using Arsoft.Utility.MSSQL;

namespace GSB.MasterPage
{
    public partial class SignIn : System.Web.UI.Page
    {
        SQLToDataTable comm = new SQLToDataTable();
        MsSqlAccess dbaccess;

        protected void Page_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
            message.Visible = false;
            if (Request.Cookies["Credit_Scoring_GSB"] != null)
                Response.Redirect("~/Default.aspx");
            //if (Request.Cookies["Credit_Scoring_GSB"] != null)
            //{
            //    DeleteCookie();
            //}
        }

        private void LoginError(int Code)
        {
            message.Visible = true;
            if (message.Text.Length <= 0)
                switch (Code)
                {
                    case 1000: message.Text = "The Username and Password fields are both required."; break;
                    case 2000: message.Text = "ไม่พบข้อมูลผู้ใช้งาน"; break;
                    case 3000: message.Text = "ชื่อผู้ใช้งาน หรือรหัสผ่านไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง หรือติดต่อผู้ดูแลระบบ"; break;
                }
        }

        private bool UserAuthorization(string Username, string Password)
        {

            CryptographyUtil crypt = new CryptographyUtil();
            dbaccess = new MsSqlAccess();
            MsSqlParameter[] param = new MsSqlParameter[2];

            string sql = "SELECT TOP 1 * " +
                            "FROM S_USER SU LEFT JOIN S_GROUP SG ON SU.GROUP_ID = SG.GROUP_ID " +
                            "WHERE SU.USER_ID = @username AND SU.USER_PWD = @password " +
                            "IF @@ROWCOUNT = 1 " +
                            "UPDATE S_USER SET LAST_LOGIN = GETDATE() WHERE USER_ID = @username ";

            //string Query = "SELECT TOP 1 * " +
            //                "FROM S_USER SU LEFT JOIN S_GROUP SG ON SU.GROUP_ID = SG.GROUP_ID " +
            //                "WHERE SU.USER_ID = '" + Username + "' AND SU.USER_PWD = '" + Password + "' " +
            //                "IF @@ROWCOUNT = 1 " +
            //                "UPDATE S_USER SET LAST_LOGIN = GETDATE() WHERE USER_ID =  '" + Username + "' ";


            if (Username != null && Password != null)
            {
                param[0] = new MsSqlParameter("@username", Username, SqlDbType.NVarChar, 20);
                param[1] = new MsSqlParameter("@password", crypt.EncryptedMD5(Password), SqlDbType.NVarChar, 255);
                //DataTable dt = comm.ExcuteSQL(Query);

                string a = crypt.EncryptedMD5(Password);

                try
                {
                    DataTable dt = dbaccess.ExecuteAdapter(sql, param);

                if (dt != null)
                    if (dt.Rows.Count > 0)
                    {
                        if (Convert.ToBoolean(dt.Rows[0]["ACTIVE_FLAG"].ToString()))
                        {
                            HttpCookie AddCookie = new HttpCookie("Credit_Scoring_GSB");
                            AddCookie["UID"] = dt.Rows[0]["UID"].ToString();
                            AddCookie["USER_ID"] = dt.Rows[0]["USER_ID"].ToString();
                            AddCookie["GROUP_ID"] = dt.Rows[0]["GROUP_ID"].ToString();
                            //AddCookie.Expires = DateTime.Now.AddMinutes(120); //.AddDays(1);
                            Response.Cookies.Add(AddCookie);
                            return true;
                        }
                        else
                        {
                            LoginError(2000);
                        }
                    }
                    else
                    {
                        LoginError(3000);
                    }

                }
                catch (Exception ex)
                {
                    //LogFiles.WriteToLog("CustomerAccess", "Insert()", ex.Message);
                    //throw new Exception(ex.Message, ex.InnerException);
                }
                finally
                {
                    if (dbaccess != null)
                    {
                        dbaccess.Dispose();
                        dbaccess = null;
                    }
                }
            }
            else LoginError(1000);

            return false;
        }

        protected void imgbtnLogin_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            message.Visible = false;
            message.Text = string.Empty;

            if (txtUsername.Text.Length <= 0 || txtPassword.Text.Length <= 0)
                LoginError(1000);
            else if (UserAuthorization(txtUsername.Text, txtPassword.Text))
                Response.Redirect("~/Default.aspx");
        }

        public void DeleteCookie()
        {            
            int limit = Request.Cookies.Count; //Get the number of cookies and 
            //use that as the limit.
            HttpCookie aCookie;   //Instantiate a cookie placeholder
            string cookieName;

            //Loop through the cookies
            for (int i = 0; i < limit; i++)
            {
                cookieName = Request.Cookies[i].Name;    //get the name of the current cookie
                aCookie = new HttpCookie(cookieName);    //create a new cookie with the same
                // name as the one you're deleting
                aCookie.Value = "";    //set a blank value to the cookie 
                aCookie.Expires = DateTime.Now.AddDays(-1);    //Setting the expiration date
                //in the past deletes the cookie

                Response.Cookies.Add(aCookie);    //Set the cookie to delete it.
            }
        }

    }
}