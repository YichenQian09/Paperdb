using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_validate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        VerifyCode vf = new VerifyCode();
        string verifyCode= vf.CreateVerifyCode(4);
        Session["verifyCode"] = verifyCode;
        vf.CreateImageOnPage(verifyCode, HttpContext.Current);
    }
}