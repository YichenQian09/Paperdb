using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// 工具类，操作关键字，一级分类，二级分类
/// </summary>
public class LabelHelper
{
    public LabelHelper()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    

    /// <summary>
    /// 根据一级分类返回本类下面的二级分类
    /// </summary>
    /// <param name="firstLevel">一级分了</param>
    /// <returns>本类下面的二级分类</returns>
    public static List<Option> bindSecondLevel(string firstLevel)
    {
        try
        {
            using (var db = new PaperDbEntities())
            {
                Category caID = db.Category.Single(a => a.Name == firstLevel);
                List<Option> opLi = (from it in db.Option where it.CategoryId == caID.id select it).ToList();
                return opLi;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }



    
    /// <summary>
    /// 得到二级选项的id，建立文章和二级选项的关系
    /// </summary>
    /// <param name="firstLevel">一级选项名字</param>
    /// <param name="secondLevel">二级选项名字</param>
    /// <returns>二级选项ID</returns>
    public static int getOptionId(string firstLevel,string secondLevel)
    {
        try
        {
            using(PaperDbEntities db =new PaperDbEntities())
            {
                Category c = db.Category.Single(a => a.Name == firstLevel);
                Option op = db.Option.Single(a => a.CategoryId == c.id && a.Name == secondLevel);
                return op.id;
            }
        }
        catch (Exception ex)
        {
            return -1;
        }

    }

    /// <summary>
    /// 返回一级选项下面的二级选项
    /// </summary>
    /// <param name="firstLevel">一级选名字</param>
    /// <returns>二级选项名字</returns>
    public static List<string> getSecondOptionName(string firstLevel)
    {
        List<string> ans = new List<string>();
        foreach (Option op in LabelHelper.bindSecondLevel(firstLevel))
        {
            ans.Add(op.Name);
        }
        return ans;
    }


    

    /// <summary>
    /// 根据一级分类的名字获取一级分类ID
    /// </summary>
    /// <param name="name">一级分类名字</param>
    /// <returns>一级分类ID,失败返回 -1</returns>
    public static int getCategoryIdByName(string name)
    {

        try
        {
            using (var db = new PaperDbEntities())
            {
                Category caID = db.Category.Single(a => a.Name == name);
                return caID.id;
            }

        }
        catch (Exception ex)
        {
            return -1;
        }
    }

 

    /// <summary>
    ///  查询需要绑定的数据，并且绑定数据源到给出的下拉列表框中(Enum("keyword","firstLevel","secondLevel"))
    /// </summary>
    /// <param name="rpt">数据容器</param>
    /// <param name="queryString">查询那个表</param>
    /// <param name="ca">为了查二级选项，需要提供一级选项名字</param>

    public static void bindLables(ref GridView rpt, string queryString,string ca=null)
    {
        try
        {
            using (var db = new PaperDbEntities())
            {
                switch (queryString)
                {

                    case "keyword":
                        List<KeyWords> keywordList = (from it in db.KeyWords orderby it.Name ascending select it).ToList();
                        rpt.DataSource = keywordList;
                        break;
                    case "firstLevel":
                        List<Category> categoryList = (from it in db.Category orderby it.Name ascending select it).ToList();
                        rpt.DataSource = categoryList;
                        break;
                    case "secondLevel":
                        Category cid = db.Category.Single(a => a.Name == ca);
                        List<Option> optionList = (from it in db.Option where it.CategoryId==cid.id orderby it.Name ascending select it).ToList();
                        rpt.DataSource = optionList;
                        break;
                }

            }

        }
        catch (Exception ex)
        {
            rpt.DataSource = null;
        }
    }




    /// <summary>
    /// 移除某一个类型(关键字，一级类型，二级类型)
    /// </summary>
    /// <param name="rmId">移除项目的ID</param>
    /// <param name="queryString">移除项目的类型（keyword,firstLevel,secondLevel）</param>
    /// <returns>是否移除成功</returns>
    public static bool removeItem(int rmId, string queryString)
    {
        try
        {
            using (var db = new PaperDbEntities())
            {
                switch (queryString)
                {
                         
                    case "keyword":
                        KeyWords keyword = db.KeyWords.Single(a=>a.id==rmId);
                        db.KeyWords.Remove(keyword);
                        break;
                    case "firstLevel":
                        Category category = db.Category.Single(a => a.id == rmId);
                        db.Category.Remove(category);
                        break;
                    case "secondLevel":
                        Option option = db.Option.Single(a => a.id == rmId);
                        db.Option.Remove(option);
                        break;
                }
                db.SaveChanges();
                return true;

            }
        }
        catch (Exception ex)
        {

            return false;
        }
    }




    /// <summary>
    /// 根据查询多关键字字符串返回关键字ID
    /// </summary>
    /// <param name="queryKeyword">关键字字符串(分隔符;)</param>
    /// <returns>关键词ID列表</returns>
    public static List<int> getKeywordIdByStringKeyword(string queryKeyword)
    {
        List<int> ids = new List<int>();
        try
        {
            using(var db=new PaperDbEntities())
            {
                ids = db.KeyWords.Where(a => queryKeyword.Contains(a.Name)).Select(a => a.id).ToList();
            }
            return ids;
        }
        catch (Exception ex)
        {
            return null;
        }

    }

}