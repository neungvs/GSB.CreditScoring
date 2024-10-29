using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Arsoft.Utility;
using Arsoft.Utility.MSSQL;

namespace GSB
{
    public partial class pwd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MsSqlConfiguration.ConfigurationConnectionString("GSBSQLServer");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ConvertPassword(1);
        }

        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            ConvertPassword(2);
        }

        private bool ConvertPassword(int typeid)
        {
            bool resutl = true;            
            MsSqlAccess dbaccess = new MsSqlAccess();
            MsSqlAccess dbexec = new MsSqlAccess();
            MsSqlParameter[] param = new MsSqlParameter[2];
            CryptographyUtil crypt = new CryptographyUtil();
            string strsql = "";
            try
            {
                switch (typeid)
                {
                    case 1:
                        strsql = "select * from S_USER where USER_ID=@user";
                        param[0] = new MsSqlParameter("@user", txtUser.Text, SqlDbType.NVarChar, 20);
                        dbaccess.ExecuteReader(strsql, param);
                        break;
                    case 2:
                        strsql = "select * from S_USER";
                        dbaccess.ExecuteReader(strsql);
                        break;
                    default:
                        break;
                }

                strsql = "UPDATE s_user SET user_pwd=@password WHERE uid=@uid";
                int uid;
                string pwd = "";
                while (dbaccess.Read())
                {
                    uid = (int)dbaccess.GetItem("uid");
                    pwd = (string)dbaccess.GetItem("user_pwd");
                    
                    param[0] = new MsSqlParameter("@password", crypt.EncryptedMD5(pwd), SqlDbType.NVarChar, 100);
                    param[1] = new MsSqlParameter("@uid", uid, SqlDbType.Int, 7);
                    dbexec.ExecuteNonQuery(strsql, param);
                }
                dbaccess.CloseReader();
                resutl = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dbaccess != null)
                {
                    dbexec.Dispose();
                    dbaccess.Dispose();
                    dbexec = null;
                    dbaccess = null;
                }
            }

            return resutl;
        }
    }
}