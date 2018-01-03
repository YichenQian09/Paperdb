<%@ WebHandler Language="C#" Class="queryMix" %>

using System;
using System.Web;
    using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
public class queryMix : IHttpHandler
{
    private string searchSingleKey = string.Empty;
    private string searchSingleVal = string.Empty;
    private string searchMultiKey = string.Empty;
    private string time = string.Empty;
    private int currentPage = 0;
    public void ProcessRequest(HttpContext context)
    {

        ArticleHelper.DataPackage dp = new ArticleHelper.DataPackage();
        //提取表单数据
        try
        {
            searchSingleKey = context.Request.Form["searchSingleKey"].ToString();
            searchSingleVal=context.Request.Form["searchSingleValue"].ToString();
            searchMultiKey=context.Request.Form["searchMultiKey"].ToString(); ;
            time = context.Request.Form["time"].ToString();
            currentPage = Convert.ToInt32(context.Request.Form["currntPage"].ToString());
        }
        catch (Exception ex)
        {
            dp.totalCnt = 0;
            context.Response.ContentType = "text/plain";
            context.Response.Write(JsonConvert.SerializeObject(dp));
        }

        //处理表单数据
        try
        {
                //首先通过简单搜索确定范围

                      List<Article> arls = new List<Article>();
            switch (searchSingleKey)
            {
                case "title":
                    arls = ArticleHelper.getArticleByTitle(searchSingleVal);
                    break;
                case "author":
                    arls = ArticleHelper.getArticleByAuthor(searchSingleVal);
                    break;
                case "keyword":
                    List<int> keywordIds = LabelHelper.getKeywordIdByStringKeyword(searchSingleVal);
                    arls = ArticleHelper.getArticleByKeyword(keywordIds);
                    break;
            }
            arls = arls.Distinct().ToList();

            //找出所有 option 的id

            List<int> opIds = new List<int>();
            foreach(string s in searchMultiKey.Split(';'))
            {
                opIds.Add( Convert.ToInt32(s));
            }
            arls=ArticleHelper.filterArticleByOptions(ref arls,ref opIds);

            string[] times = time.Split(';');
            if (times.Length > 1)
            {
                //筛选符合时间条件的文章
                DateTime lowYear = new DateTime(Convert.ToInt32( times[0]), 1, 1);
                DateTime highYear = new DateTime( Convert.ToInt32(times[1]), 1, 1);
                arls = arls.Where(a => a.UpateTime <= highYear && a.UpateTime >= lowYear).ToList();
            }
            arls = arls.Distinct().ToList();
            dp = ArticleHelper.generateDataPackage(ref arls, currentPage);
        }
        catch(Exception ex)
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