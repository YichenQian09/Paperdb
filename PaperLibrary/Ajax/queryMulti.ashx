<%@ WebHandler Language="C#" Class="queryMulti" %>

using System;
using System.Web;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
public class queryMulti : IHttpHandler {
    private string searchKey = string.Empty;
    private string time = string.Empty;
    private int currentPage = 0;
    public void ProcessRequest (HttpContext context) {
        ArticleHelper.DataPackage dp = new ArticleHelper.DataPackage();
        //提取表单数据
        try
        {
            searchKey = context.Request.Form["searchKey"].ToString();
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
            List<int> opIds = new List<int>();
            foreach(string s in searchKey.Split(';'))
            {
                opIds.Add( Convert.ToInt32(s));
            }
            List<Article> arls = null;
            //如果opIds有 -1，则返回全部内容
            if (opIds.Contains(-1))
                arls = ArticleHelper.getAllArticle();
            else
                arls = ArticleHelper.getArticleByOptions(opIds);

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

    public bool IsReusable {
        get {
            return false;
        }
    }

}