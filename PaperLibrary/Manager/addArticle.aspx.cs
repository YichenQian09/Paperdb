using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class Manager_addArticle : System.Web.UI.Page
{
    //各种选项的下拉框控件和一级标题字典
    static Dictionary<DropDownList, string> optionsDict = new Dictionary<DropDownList, string>();
    public string executeJS = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        initOptionDict();
        if (!IsPostBack)
        {

            //绑定各种控件初始值
            bindKeywords();
            bindTimeList();
            bindDplOption(optionsDict);

            //判断是否修改
            int articleId = 0;
            try
            {
                articleId = Convert.ToInt32(Request.QueryString["id"].ToString());
                bindEdit(articleId);
            }
            catch (Exception ex)
            {
                articleId = 0;
            }
        }
    }



    /// <summary>
    /// 初始化各种二级选项的下拉框内容
    /// </summary>
    private void initOptionDict()
    {
        optionsDict.Clear();
        optionsDict.Add(dplDQ, "地区");
        optionsDict.Add(dplSTXTLX, "生态系统类型");
        optionsDict.Add(dplWXZS, "文献综述");
        optionsDict.Add(dplYJPG, "一级评估");
        optionsDict.Add(dplEJPG, "二级评估");
        optionsDict.Add(dplSTXTFWLX, "生态系统服务类型");
        optionsDict.Add(dplMX, "方法");
        optionsDict.Add(dplFSCJZLX, "非市场价值类型");
    }


    /// <summary>
    /// 绑定关键字
    /// </summary>
    void bindKeywords()
    {
        rptKeywords.DataSource = ArticleHelper.getAllKeywords();
        rptKeywords.DataBind();
    }



    /// <summary>
    /// 绑定时间列表
    /// </summary>
    /// <returns></returns>
    protected void bindTimeList()
    {
        List<int> ls = new List<int>();
        int nowYear = DateTime.Now.Year;
        for (int i = nowYear; i >= WEBCONFIG.ARTICLE_YEAR_LOW; i--)
            ls.Add(i);
        dplTime.DataSource = ls;
        dplTime.DataBind();

    }


    /// <summary>
    /// 绑定各种二级选项
    /// </summary>
    /// <param name="dpl">需要绑定的下拉框</param>
    /// <param name="firstLevel">一级选项</param>
    protected void bindDplOption(ref DropDownList dpl, string firstLevel)
    {
        dpl.DataSource = LabelHelper.getSecondOptionName(firstLevel);
        dpl.DataBind();
    }

    protected void bindDplOption(Dictionary<DropDownList, string> dict)
    {
        foreach (KeyValuePair<DropDownList, string> d in dict)
        {
            d.Key.DataSource = LabelHelper.getSecondOptionName(d.Value);
            d.Key.DataBind();
        }
    }



    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtTitle.Text.Trim().Equals(string.Empty))
            Response.Write(JSHelper.alert("请输入文章标题"));
        else if (txtAuthor.Text.Trim().Equals(string.Empty))
            Response.Write(JSHelper.alert("请输入文章作者"));
        else if (txtSummary.Text.Trim().Equals(string.Empty))
            Response.Write(JSHelper.alert("请输入文章摘要"));
        else if (txtLink.Text.Trim().Equals(string.Empty))
            Response.Write(JSHelper.alert("请输入文章链接"));
        else if(txtJournal.Text.Trim().Equals(string.Empty))
            Response.Write(JSHelper.alert("请输入文章所属期刊"));
        else
        {
            //检测LINK正确性
            string pattern = @"(http|https)://[^\s]*";
            Regex r = new Regex(pattern);
            if (r.IsMatch(txtLink.Text.Trim()) == false)
                Response.Write(JSHelper.alert("请输入合法链接,检查链接的完整性！(http(s)://xxx)"));
            else
            {

                if (Request.QueryString["id"]!=null)
                {
                    //更新文章
                    int articleId = Convert.ToInt32(Request.QueryString["id"].ToString());
                    if (updateArticle(articleId))
                        Response.Write(JSHelper.alert("文章更新成功!","articleList.aspx"));
                    else
                        Response.Write(JSHelper.alert("文章更新失败!请检查各项正确性后重试!"));
                }
                else
                {
                    //保存文章
                    if (addNewArticle())
                        Response.Write(JSHelper.alert("文章添加成功!", "articleList.aspx"));
                    else
                        Response.Write(JSHelper.alert("文章添加失败!请检查各项正确性后重试!"));

                }


            }


        }
    }

    protected bool addNewArticle()
    {
        bool addSuccess = true;
        //构造文章对象
        Article ar = new Article();
        ar.Title = txtTitle.Text.Trim();
        ar.Author = txtAuthor.Text.Trim();
        int year = Convert.ToInt32(dplTime.SelectedValue);
        ar.UpateTime = new DateTime(year, 1, 1);
        ar.Summary = txtSummary.Text.Trim();
        ar.Link = txtLink.Text.Trim();
        ar.Journal = txtJournal.Text.Trim();
        int articleId = -1;
        try
        {
            if ((articleId = ArticleHelper.addArticle(ar)) != -1)
            {

                //添加关键字
                string aa = Request.Form["keywords"].ToString();
                string[] keys = Request.Form["keywords"].ToString().Split(',');
                //拿到非重复元素
                string[] keywords = keys.Distinct().ToArray();
                List<int> keywordId = new List<int>();
                for (int i = 0; i < keywords.Length; i++)
                    keywordId.Add(Convert.ToInt32(keywords[i]));
                if (!ArticleHelper.addKeywords(articleId, keywordId))
                    addSuccess = false;
                //添加各种选项
                if (!ArticleHelper.addOptions(articleId, getOptions()))
                    addSuccess = false;
                if (!addSuccess)
                {
                    //删除已经添加的文章对象
                    ArticleHelper.delArticle(articleId);
                    return false;
                }
                else
                    return true;
            }
            else
                return false;

        }
        catch (Exception ex)
        {
            //代码回滚删除构建的文章，防止由于某个函数错误文章信息不完整
            ArticleHelper.delArticle(articleId);
            return false;
        }

    }



    protected bool updateArticle(int articleId)
    {

        bool editSuccess = true;
        //保存旧的文章对象
        Article oldAr = ArticleHelper.getArticleById(articleId);
        //保存旧的文章的关键字记录表ID
        List<KeyWordConnection> oldKeywordConns = ArticleHelper.getArticleKeywordConn(articleId);
        //保存旧的文章的类型记录表ID
        List<TypeConnection> oldTypeConns = ArticleHelper.getArticleTypeConn(articleId);

        //构造文章对象
        Article ar = new Article();
        ar.id = articleId;
        ar.Title = txtTitle.Text.Trim();
        ar.Author = txtAuthor.Text.Trim();
        int year = Convert.ToInt32(dplTime.SelectedValue);
        ar.UpateTime = new DateTime(year, 1, 1);
        ar.Summary = txtSummary.Text.Trim();
        ar.Link = txtLink.Text.Trim();
        ar.Journal = txtJournal.Text.Trim();

        if (ArticleHelper.updateArticle(ar))
        {
            //添加关键字
            string aa = Request.Form["keywords"].ToString();
            string[] keys = Request.Form["keywords"].ToString().Split(',');
            //拿到非重复元素
            string[] keywords = keys.Distinct().ToArray();
            List<int> keywordId = new List<int>();
            for (int i = 0; i < keywords.Length; i++)
                keywordId.Add(Convert.ToInt32(keywords[i]));
            if (!ArticleHelper.addKeywords(articleId, keywordId))
                editSuccess = false;
            //添加各种选项
            if (!ArticleHelper.addOptions(articleId, getOptions()))
                editSuccess = false;
            if (!editSuccess)
            {
                //文章更新失败
                //删除现有所有的关键词和类型选项记录
                var delKeywords = ArticleHelper.getArticleKeywordConn(articleId);
                var delTypeds = ArticleHelper.getArticleTypeConn(articleId);
                List<int> delIds = new List<int>();
                foreach (var del in delKeywords)
                    delIds.Add(del.id);
                ArticleHelper.delKeywordConnecion(delIds);
                delIds.Clear();
                foreach (var del in delTypeds)
                    delIds.Add(del.id);
                ArticleHelper.delTypeConnection(delIds);

                //恢复原来文章 
                ArticleHelper.updateArticle(oldAr);
                //恢复旧文章的关键词和类型选项记录
                List<int> addIds = new List<int>();
                foreach (var t in oldKeywordConns)
                    addIds.Add(t.id);
                ArticleHelper.addKeywords(articleId, addIds);
                addIds.Clear();
                foreach (var t in oldTypeConns)
                    addIds.Add(t.id);
                ArticleHelper.addOptions(articleId, addIds);
                return false;
            }
            else
            {
                //更新成功
                //删除旧的文章关键词记录ID,类型ID
                List<int> delIds = new List<int>();
                foreach (var del in oldKeywordConns)
                    delIds.Add(del.id);
                ArticleHelper.delKeywordConnecion(delIds);
                delIds.Clear();
                foreach (var del in oldTypeConns)
                    delIds.Add(del.id);
                ArticleHelper.delTypeConnection(delIds);
                return true;
            }
        }
        else
            return false;
    }


    /// <summary>
    /// 获取页面中各个选项的值
    /// </summary>
    /// <returns></returns>
    protected Dictionary<string, string> getOptions()
    {
        Dictionary<string, string> a = new Dictionary<string, string>();
        foreach (KeyValuePair<DropDownList, string> d in optionsDict)
            a.Add(d.Value, d.Key.SelectedValue);
        return a;
    }


    protected void bindEdit(int articleId)
    {
        Article ar = ArticleHelper.getArticleById(articleId);
        txtTitle.Text = ar.Title;
        txtAuthor.Text = ar.Author;
        txtSummary.Text = ar.Summary;
        txtLink.Text = ar.Link;
        txtJournal.Text = ar.Journal;
        dplTime.SelectedValue = ar.UpateTime.Year.ToString();
        //绑定各种选项
        changeOptions(articleId);
        //绑定关键字,前台JS实现，调用了 executeJS 变量
        executeJS = bindKeywords(articleId);

    }




    /// <summary>
    /// 后台生成文章关键字HTML的js，供给前台调用,前台直接 eval（Js segment）
    /// </summary>
    /// <param name="articleId">文章ID</param>
    /// <returns>文章关键字HTML</returns>
    public string bindKeywords(int articleId)
    {
        Dictionary<int, string> keywords = ArticleHelper.getKeyowrd(articleId);
        return JsonConvert.SerializeObject(keywords);
    }



    /// <summary>
    /// 更改各种选项为文章的选项
    /// </summary>
    /// <param name="articleId">文章ID</param>
    public void changeOptions(int articleId)
    {
        Dictionary<string, string> allOps = ArticleHelper.getOptions(articleId);
        foreach (KeyValuePair<DropDownList, string> k in optionsDict)
            k.Key.SelectedValue = allOps[k.Value];
    }
}