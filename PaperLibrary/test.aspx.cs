using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        List<int> ls = new List<int>();
        ls.Add(1);
        ls.Add(1);
        ls.Add(1);
        ls.Add(1);
        ls.Add(1);
        Response.Write(ls[2]);
        kk(ref ls);
        Response.Write(ls[2]);
    }

    void kk(ref List<int> ll)
    {
        ll[2] = 66666;
    }
}