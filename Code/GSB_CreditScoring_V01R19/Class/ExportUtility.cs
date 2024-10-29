using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSB.Class
{
    public sealed class ExportUtility
    {
        public static string getCurrentUserId
        {
            get
            {
                HttpCookie authCookie = HttpContext.Current.Request.Cookies["Credit_Scoring_GSB"];
                if (authCookie != null)
                {
                    return string.IsNullOrEmpty(authCookie["USER_ID"])? string.Empty : authCookie["USER_ID"];
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}