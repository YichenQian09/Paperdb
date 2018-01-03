using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_articleList : System.Web.UI.Page
{
    List<Article> arLis = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            arLis = ArticleHelper.getAllArticle();
            gdvArticle.DataSource = arLis;
            gdvArticle.DataBind();
        }
    }

    protected void gdvArticle_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("delAritcle"))
        {
            int id = Convert.ToInt32(e.CommandArgument);
            if (ArticleHelper.delArticle(id))
                Response.Write(JSHelper.alert("删除成功！", "articleList.aspx"));
            else
                Response.Write(JSHelper.alert("删除失败，请重试！"));

        }
    }

    protected void gdvArticle_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvArticle.DataSource = arLis;
        gdvArticle.PageIndex = e.NewPageIndex;
        gdvArticle.DataBind();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string title = txtTitle.Text.Trim();
        if (title.Equals(string.Empty))
            arLis = ArticleHelper.getAllArticle();
        else
            arLis = ArticleHelper.getArticleByTitle(title);
        gdvArticle.DataSource = arLis;
        gdvArticle.DataBind();

    }
}