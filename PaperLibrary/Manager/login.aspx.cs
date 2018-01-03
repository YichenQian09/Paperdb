using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Convert.ToBoolean(Session["user"]))
            Response.Redirect("articleList.aspx");
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text.Trim();
        string verify = txtValidate.Text.Trim();
        if (username.Equals(string.Empty))
            Response.Write(JSHelper.alert("请输入用户名!"));
        else if(password.Equals(string.Empty))
            Response.Write(JSHelper.alert("请输入密码!"));
        else if (verify.Equals(string.Empty) || !verify.Equals(Session["verifyCode"].ToString()))
            Response.Write(JSHelper.alert("请检查验证码!"));
        else if(!username.Equals(WEBCONFIG.ADMIN_USERNAME) || !password.Equals(WEBCONFIG.ADMIN_PASSWORD))
            Response.Write(JSHelper.alert("用户名或密码错误!"));
        else
        {
            //Response.Write(JSHelper.alert("登录成功!", "articleList.aspx"));
            Session["user"] = true;
            Response.Redirect("articleList.aspx");
        }
    }
}