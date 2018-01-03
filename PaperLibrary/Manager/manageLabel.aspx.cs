using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_manageLabel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            LabelHelper.bindLables(ref gdvKeywords, "keyword");
            gdvKeywords.DataBind();
        }
    }
    protected void btnKeyword_Click(object sender, EventArgs e)
    {
        string keywordvalu = txtKeyword.Text.Trim();
        if (keywordvalu.Equals(string.Empty))
            Response.Write(JSHelper.alert("关键词不能为空，请重新输入！"));
        else
        {
            try
            {
                using (var db = new PaperDbEntities())
                {
                    KeyWords tmp = db.KeyWords.SingleOrDefault(a => a.Name == keywordvalu);
                    if (tmp == null)
                    {
                        KeyWords keyword = new KeyWords();
                        keyword.Name = keywordvalu;
                        db.KeyWords.Add(keyword);
                        db.SaveChanges();
                        Response.Write(JSHelper.alert("添加成功!", "manageLabel.aspx"));

                    }
                    else
                    {
                        Response.Write(JSHelper.alert("关键词重复，请检查！"));
                    }

                }

            }
            catch (Exception ex)
            {
                //ex.Message;
                Response.Write(JSHelper.alert("添加失败，请重新尝试!"));
            }
        }
    }

    protected void dplManageLabel_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dplManageLabel.SelectedValue.Equals("关键字管理"))
        {
            
            panel_keywords.Visible = true;
            LabelHelper.bindLables(ref gdvKeywords, "keyword");
            gdvKeywords.DataBind();
            panel_firstLevel.Visible = panel_secondLevel.Visible = false;

        }else if(dplManageLabel.SelectedValue.Equals("一级分类管理"))
        {
            panel_firstLevel.Visible = true;
            LabelHelper.bindLables(ref gdvFirstLevel, "firstLevel");
            gdvFirstLevel.DataBind();
            panel_keywords.Visible = panel_secondLevel.Visible = false;
        }else
        {
            try
            {
                //绑定一级分类
                using(var db=new PaperDbEntities())
                {
                    List<string> caLi = (from it in db.Category select it.Name).ToList();
                    dplFirstLevel.DataSource = caLi;
                    dplFirstLevel.DataBind();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            panel_secondLevel.Visible = true;
            LabelHelper.bindLables(ref gdvSecondLevel, "secondLevel",dplFirstLevel.SelectedValue);
            gdvSecondLevel.DataBind();
            panel_keywords.Visible = panel_firstLevel.Visible = false;
        }
    }

    protected void btnFirstLevel_Click(object sender, EventArgs e)
    {
        string firstLevelVal = txtFirstLevel.Text.Trim();
        if (firstLevelVal.Equals(string.Empty))
            Response.Write(JSHelper.alert("一级分类不能为空，请重新输入！"));
        else
        {
            try
            {
                using (var db = new PaperDbEntities())
                {
                    Category tmp = db.Category.SingleOrDefault(a => a.Name == firstLevelVal);
                    if (tmp == null)
                    {
                        Category firstLevel = new Category();
                        firstLevel.Name = firstLevelVal;
                        db.Category.Add(firstLevel);
                        db.SaveChanges();
                        Response.Write(JSHelper.alert("添加成功!", "manageLabel.aspx"));

                    }
                    else
                    {
                        Response.Write(JSHelper.alert("一级分类重复，请检查！"));
                    }

                }

            }
            catch (Exception ex)
            {
                //ex.Message;
                Response.Write(JSHelper.alert("添加失败，请重新尝试!"));
            }
        }
    }

    
    protected void btnSecondLevel_Click(object sender, EventArgs e)
    {
        
        string secondLevelVal = txtSecondLevel.Text.Trim();
        if (secondLevelVal.Equals(string.Empty))
            Response.Write(JSHelper.alert("二级分类不能为空，请重新输入！"));
        else
        {
            try
            {
                using (var db = new PaperDbEntities())
                {
                    Category c = db.Category.Single(a => a.Name == dplFirstLevel.SelectedValue);
                    Option tmp = db.Option.SingleOrDefault(a => a.Name == secondLevelVal && a.CategoryId==c.id);
                    if (tmp == null)
                    {
                        Option secondLevel = new Option();
                        secondLevel.Name = secondLevelVal;
                        secondLevel.CategoryId = LabelHelper.getCategoryIdByName(dplFirstLevel.SelectedValue);
                        db.Option.Add(secondLevel);
                        db.SaveChanges();
                        Response.Write(JSHelper.alert("添加成功!", "manageLabel.aspx"));

                    }
                    else
                    {
                        Response.Write(JSHelper.alert("二级分类重复，请检查！"));
                    }

                }

            }
            catch (Exception ex)
            {
                //ex.Message;
                Response.Write(JSHelper.alert("添加失败，请重新尝试!"));
            }
        }

    }

    protected void dplFirstLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
       gdvSecondLevel.DataSource= LabelHelper.bindSecondLevel(dplFirstLevel.SelectedValue);
        gdvSecondLevel.DataBind();
    }




    protected void gdvKeywords_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("delKeyword"))
        {
            int id = Convert.ToInt32(e.CommandArgument);
            if (LabelHelper.removeItem(id, "keyword"))
                Response.Write(JSHelper.alert("删除成功!","manageLabel.aspx"));
            else
                Response.Write(JSHelper.alert("删除失败，请重试!"));

        }
    }

    protected void gdvKeywords_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        LabelHelper.bindLables(ref gdvKeywords, "keyword");
        gdvKeywords.PageIndex = e.NewPageIndex;
        gdvKeywords.DataBind();
    }




    protected void gdvFirstLevel_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("delFirstLevel"))
        {
            int id = Convert.ToInt32(e.CommandArgument);
            if (LabelHelper.removeItem(id, "firstLevel"))
                Response.Write(JSHelper.alert("删除成功!", "manageLabel.aspx"));
            else
                Response.Write(JSHelper.alert("删除失败，请重试!"));

        }
    }

    protected void gdvFirstLevel_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        LabelHelper.bindLables(ref gdvFirstLevel, "firstLevel");
        gdvFirstLevel.PageIndex = e.NewPageIndex;
        gdvFirstLevel.DataBind();
    }

    protected void gdvSecondLevel_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        
            if (e.CommandName.Equals("delSecondLevel"))
        {
            int id = Convert.ToInt32(e.CommandArgument);
            if (LabelHelper.removeItem(id, "secondLevel"))
                Response.Write(JSHelper.alert("删除成功!", "manageLabel.aspx"));
            else
                Response.Write(JSHelper.alert("删除失败，请重试!"));

        }
    }

    protected void gdvSecondLevel_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        LabelHelper.bindLables(ref gdvSecondLevel, "firstLevel");
        gdvSecondLevel.PageIndex = e.NewPageIndex;
        gdvSecondLevel.DataBind();
    }
}