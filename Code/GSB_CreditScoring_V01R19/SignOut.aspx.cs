using System;
using System.Web;
using GSB.Class;
using System.Data;

namespace GSB
{
    public partial class SignOut : System.Web.UI.Page
    {
        SQLToDataTable conn = new SQLToDataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
            HttpCookie delCookie = new HttpCookie("Credit_Scoring_GSB");
            delCookie.Expires = DateTime.Now.AddDays(-1D);
            Response.Cookies.Add(delCookie);
            Response.Redirect("~/SignIn.aspx");
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
                    //case 1000: message.Text = "Please enter your username."; break;
                    //case 2000: message.Text = "You do not have permission to login."; break;
                    //case 3000: message.Text = "Username or Password is not correct."; break;
                }
        }

        private bool UserAuthorization(string Username, string Password)
        {
            string Query = "SELECT TOP 1 * " +
                            "FROM S_USER SU LEFT JOIN S_GROUP SG ON SU.GROUP_ID = SG.GROUP_ID " +
                            "WHERE SU.USER_ID = '" + Username + "' AND SU.USER_PWD = '" + Password + "'  " +
                            "IF @@ROWCOUNT = 1 " +
                            "UPDATE S_USER SET LAST_LOGIN = GETDATE() WHERE USER_ID =  '" + Username + "' ";

            if (Username != null && Password != null)
            {
                DataTable dt = conn.ExcuteSQL(Query);
                if (dt != null)
                    if (dt.Rows.Count > 0)
                    {
                        if (Convert.ToBoolean(dt.Rows[0]["ACTIVE_FLAG"].ToString()))
                        {
                            HttpCookie AddCookie = new HttpCookie("Credit_Scoring_GSB");
                            AddCookie["UID"] = dt.Rows[0]["UID"].ToString();
                            AddCookie["USER_ID"] = dt.Rows[0]["USER_ID"].ToString();
                            AddCookie["GROUP_ID"] = dt.Rows[0]["GROUP_ID"].ToString();
                            AddCookie.Expires = DateTime.Now.AddDays(1);
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
    }
}