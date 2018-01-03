<%@ WebHandler Language="C#" Class="getOptions" %>

using System;
using System.Web;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
public class getOptions : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        List<DataPackage> ans = new List<DataPackage>();
        string[] firstLevel = { "地区", "生态系统类型", "文献综述", "一级评估", "二级评估", "生态系统服务类型", "方法", "非市场价值类型" };
        try
        {
            foreach(string s in firstLevel)
            {
                DataPackage dp = new DataPackage();
                dp.firstType = s;
                dp.secondType = getSecondObj(s);
                ans.Add(dp);
            }
        }
        catch (Exception ex)
        {

            ans.Clear();
            DataPackage dp = new DataPackage();
            dp.firstType = "error";
            ans.Add(dp);
        }
        finally
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(JsonConvert.SerializeObject(ans));
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }



    private List<DataRet> getSecondObj(string firstLevel)
    {
        List<DataRet> ans = new List<DataRet>();
        try
        {
            using (var db = new PaperDbEntities())
            {
                ans = (from it in db.Category
                       where it.Name == firstLevel
                       join op in db.Option on it.id equals op.CategoryId
                       select new DataRet { id = op.id, name = op.Name })
                       .ToList();

            }
            return ans;
        }
        catch (Exception ex)
        {
            return ans;
        }
    }

    class DataRet
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    class DataPackage
    {
        public string firstType { get; set; }
        public List< DataRet> secondType { get; set; }

    }
}