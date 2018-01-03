using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_manager : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Convert.ToBoolean(Session["user"]))
            Response.Write(JSHelper.alert("请先登录!","login.aspx"));
    }

    protected void logout_Click(object sender, EventArgs e)
    {
        Session["user"] = false;
        Response.Write(JSHelper.alert("登出成功!","/"));
    }
}
