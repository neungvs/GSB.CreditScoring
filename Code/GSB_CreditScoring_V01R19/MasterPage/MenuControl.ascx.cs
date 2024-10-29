using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using GSB.Class;

namespace GSB
{
    public class MenuProp
    {
        public int MenuId;
        public string MenuName;
        public string MenuUrl;
        public string ImageUrl;
        public string ReMark;
        public int ParentId;
        public List<MenuProp> ChildMenu = new List<MenuProp>();
    }

    public partial class MenuControl : System.Web.UI.UserControl
    {
        //SQLToDataTable conn = new SQLToDataTable();

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!this.IsPostBack)
            //{
                BindingMenu(string.Empty);
            //}
        }

        #endregion Page_Load

        #region Private Method

        public List<GSB.MenuProp> GetdatafromDb(string userLogin)
        {
            string GroupID = string.Format("{0}", Request.Cookies["Credit_Scoring_GSB"]["GROUP_ID"]);
            //string Query = ";with " +
            //                   "All_Program AS (SELECT  B.* FROM S_GPROG A INNER JOIN S_PROGRAM B ON B.PROG_ID = A.PROG_ID WHERE B.SHOW=1 AND A.CVIEW=1 AND A.GROUP_ID = '" + GroupID + "'), " +
            //                   "All_Parent_Need AS (SELECT DISTINCT ap.PARENTMENUID FROM All_Program ap), " +
            //                   "Sub_Parent_Need AS (SELECT DISTINCT sp.PARENTMENUID FROM All_Parent_Need ap LEFT JOIN S_PROGRAM sp ON ap.PARENTMENUID = sp.PROG_ID WHERE sp.PARENTMENUID != 0), " +
            //                   "Result AS ( " +
            //                        "SELECT ap.* FROM All_Program ap " +
            //                        "UNION SELECT sp.* FROM All_Parent_Need ap LEFT JOIN S_PROGRAM sp ON ap.PARENTMENUID = sp.PROG_ID " +
            //                        "UNION SELECT sp.* FROM Sub_Parent_Need ap LEFT JOIN S_PROGRAM sp ON ap.PARENTMENUID = sp.PROG_ID " +
            //                        "UNION SELECT sp.* FROM S_PROGRAM sp WHERE PROG_ID = 1 " +
            //                   ") SELECT * FROM Result ORDER BY PARENTMENUID, SORTINDEX ASC";

            string Query = "select * from ( " +
            "select ap.* from (SELECT  B.* FROM S_GPROG A INNER JOIN S_PROGRAM B ON B.PROG_ID = A.PROG_ID WHERE B.SHOW=1 AND A.CVIEW=1 AND A.GROUP_ID = '" + GroupID + "') as ap " +
            "UNION select sp.* from (SELECT DISTINCT ap.PARENTMENUID FROM (SELECT  B.* FROM S_GPROG A INNER JOIN S_PROGRAM B ON B.PROG_ID = A.PROG_ID WHERE B.SHOW=1 AND A.CVIEW=1 AND A.GROUP_ID = '" + GroupID + "') ap ) ap LEFT JOIN S_PROGRAM sp ON ap.PARENTMENUID = sp.PROG_ID " +
            "UNION select sp.* from (SELECT DISTINCT ap.PARENTMENUID FROM (SELECT DISTINCT ap.PARENTMENUID FROM (SELECT  B.* FROM S_GPROG A INNER JOIN S_PROGRAM B ON B.PROG_ID = A.PROG_ID WHERE B.SHOW=1 AND A.CVIEW=1 AND A.GROUP_ID = '" + GroupID + "') ap ) ap LEFT JOIN S_PROGRAM sp ON ap.PARENTMENUID = sp.PROG_ID ) ap LEFT JOIN S_PROGRAM sp ON ap.PARENTMENUID = sp.PROG_ID " +
            "UNION SELECT sp.* FROM S_PROGRAM sp WHERE PROG_ID = 1 " +
            ") Result ";

            List<MenuProp> FlatList = new List<MenuProp>();
            SQLToDataTable conn = new SQLToDataTable();
            try
            {
                DataTable dt = conn.ExcuteSQL(Query);

                string ls_Url = string.Empty;
                int menuID = 0;
                if (dt != null)
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["PARENTMENUID"].ToString().Length > 0)
                        {
                            ls_Url = HttpRuntime.AppDomainAppVirtualPath + "/" + dt.Rows[i]["NAVIGATEURL"].ToString();  // Return \Virtual directory of project
                        }

                        int.TryParse(dt.Rows[i]["PROG_ID"].ToString(), out menuID);

                        if (!string.IsNullOrEmpty(dt.Rows[i]["PROG_ID"].ToString()))
                        {
                            FlatList.Add(new MenuProp()
                            {
                                MenuId = menuID,
                                MenuName = dt.Rows[i]["PROG_NAME"].ToString(),
                                MenuUrl = dt.Rows[i]["NAVIGATEURL"].ToString(),
                                ParentId = Convert.ToInt32(dt.Rows[i]["PARENTMENUID"].ToString()),
                                ImageUrl = dt.Rows[i]["IMAGEURL"].ToString(),
                                ReMark = dt.Rows[i]["RAMARK"].ToString()
                            });
                        }


                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.CloseConnection();
            }
            return FlatList;
        }

        private void BindingMenu(string lsUserLogin)
        {
            StringBuilder buff = new StringBuilder();
            Literal ltrMenu = new Literal();
            List<GSB.MenuProp> listMenu = GetdatafromDb(lsUserLogin);
            List<GSB.MenuProp> rootMenus = ConvertCategoryListToTree(listMenu);

            int index = 0;
            foreach (GSB.MenuProp rootMenu in rootMenus)
            {
                index = index + 1;

                if (rootMenu.ReMark == "admin")
                    buff.Append("<li class=\"send_right\">");
                else
                    buff.Append("<li class=\"send_left\">");

                if (rootMenu.MenuUrl != "#")
                    buff.Append(string.Format("<a href=\"{0}\">", ResolveUrl("~/" + rootMenu.MenuUrl)));
                else
                    buff.Append("<a>");

                var tmpMenuList = from menu in listMenu
                                  where menu.ParentId.Equals(rootMenu.MenuId)
                                  select menu;
                List<GSB.MenuProp> listChildMenu = tmpMenuList.ToList();

                if (0 < listChildMenu.Count)
                {
                    buff.Append(string.Format("{0}<img src=\"{1}\">", rootMenu.MenuName, ResolveUrl("~/Images/Menu/navDown.png"))).Append("</a>");
                    buff.AppendLine("<ul>");
                    BuildChildMenuDynamic(listMenu, ref  buff, listChildMenu);
                    buff.AppendLine("</ul>");
                }
                else buff.Append(string.Format("{0}", rootMenu.MenuName)).Append("</a>");

                buff.AppendLine("</li>");
            }

            ltrMenu.Text = buff.ToString();
            Menu.Controls.Add(ltrMenu);
        }

        private void BuildChildMenuDynamic(List<GSB.MenuProp> categorysAll, ref StringBuilder buff, List<GSB.MenuProp> categorySelect)
        {
            int i = 0;
            foreach (GSB.MenuProp childCategory in categorySelect)
            {
                i++;
                var tmpMenuList = from menu in categorysAll
                                  where menu.ParentId.Equals(childCategory.MenuId)
                                  select menu;

                List<GSB.MenuProp> listChildMenus = tmpMenuList.ToList();

                if (listChildMenus.Count > 0)
                {
                    buff.Append("<li>");

                    if (childCategory.MenuUrl != "#")
                        buff.Append(string.Format("<a href=\"{0}\">", ResolveUrl("~/" + childCategory.MenuUrl)));
                    else
                        buff.Append("<a>");
                    
                    buff.Append(string.Format("{0}<img src=\"{1}\">", childCategory.MenuName, ResolveUrl("~/Images/Menu/navRight.png"))).Append("</a>");
                    buff.AppendLine("<ul>");

                    int j = 0;
                    foreach (GSB.MenuProp listChildMenu in listChildMenus)
                    {
                        j++;
                        if (j == 1)
                            buff.Append("<li class=\"first_menu\">");
                        else
                            buff.Append("<li>");

                        if (listChildMenu.MenuUrl != "#")
                            buff.Append(string.Format("<a href=\"{0}\">", ResolveUrl("~/" + listChildMenu.MenuUrl)));
                        else
                            buff.Append("<a>");

                        buff.Append(string.Format("{0}", listChildMenu.MenuName)).Append("</a>");
                        buff.AppendLine("</li>");
                    }

                    buff.AppendLine("</ul>");
                    buff.AppendLine("</li>");
                }
                else
                {
                    if (i == categorySelect.Count)
                        buff.Append("<li class=\"last_menu\">");
                    else
                        buff.Append("<li>");

                    if (childCategory.MenuUrl != "#")
                        buff.Append(string.Format("<a href=\"{0}\">", ResolveUrl("~/" + childCategory.MenuUrl)));
                    else
                        buff.Append("<a>");

                    buff.Append(string.Format("{0}", childCategory.MenuName)).Append("</a>");
                    buff.AppendLine("</li>");
                }

                if (childCategory.ChildMenu == null)
                    continue;
            }
        }

        private List<MenuProp> ConvertCategoryListToTree(List<MenuProp> flatList)
        {
            var rootNodes = new List<MenuProp>();
            foreach (var node in flatList)
            {
                //The parent of this node in the flat list (if there is one).
                var parent = flatList.Find(i => i.MenuId == node.ParentId);
                if (parent == null)
                {
                    //Collect the root nodes to return later...
                    rootNodes.Add(node);
                }
                else
                {
                    //Ignore orphans (should never happen, but just in case)...
                    if (!flatList.Exists(i => i.MenuId == node.ParentId))
                        continue;

                    //add this node to the child list of its parent.
                    if (parent.ChildMenu == null)
                        parent.ChildMenu = new List<MenuProp>();

                    parent.ChildMenu.Add(node);
                }
            }
            return rootNodes;
        }

        #endregion Private Method
    }
}