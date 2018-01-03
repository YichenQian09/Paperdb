<%@ WebHandler Language="C#" Class="querySingle" %>

using System;
using System.Web;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
public class querySingle : IHttpHandler
{
    private string[] queryString = { "title", "author", "keyword" };
    public void ProcessRequest(HttpContext context)
    {
        ArticleHelper.DataPackage dp = new ArticleHelper.DataPackage();
        int currentPage = 0;
        string searchKey = string.Empty;
        string searchValue = string.Empty;

        //获取参数
        try
        {
            searchKey = context.Request.Form["searchKey"].ToString();
            //查询条件出错
            if (!queryString.Contains(searchKey))
                return;
            searchValue = context.Request.Form["searchValue"].ToString();
            currentPage = Convert.ToInt32(context.Request.Form["currntPage"].ToString());
        }
        catch (Exception ex)
        {

            dp.totalCnt = 0;
            context.Response.ContentType = "text/plain";
            context.Response.Write(JsonConvert.SerializeObject(dp));
        }


        try
        {
            //处理从表单处理这些
            List<Article> arls = new List<Article>();
            switch (searchKey)
            {
                case "title":
                    arls = ArticleHelper.getArticleByTitle(searchValue);
                    break;
                case "author":
                    arls = ArticleHelper.getArticleByAuthor(searchValue);
                    break;
                case "keyword":
                    List<int> keywordIds = LabelHelper.getKeywordIdByStringKeyword(searchValue);
                    arls = ArticleHelper.getArticleByKeyword(keywordIds);
                    break;
            }
            arls = arls.Distinct().ToList();
            dp = ArticleHelper.generateDataPackage(ref arls, currentPage);
        }
        catch (Exception ex)
        {
            dp.totalCnt = 0;
        }
        finally
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(JsonConvert.SerializeObject(dp));
        }

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}