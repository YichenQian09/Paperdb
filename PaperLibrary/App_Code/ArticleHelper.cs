using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

/// <summary>
/// 文章操作类
/// </summary>
public class ArticleHelper
{
    public ArticleHelper()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// 添加新文章
    /// </summary>
    /// <param name="ar">待添加文章对象</param>
    /// <returns>添加成功返回文章ID,否则返回-1</returns>
    public static int addArticle(Article ar)
    {
        try
        {
            using (var db = new PaperDbEntities())
            {
                db.Article.Add(ar);
                db.SaveChanges();
                Article tmp = db.Article.Single(a => a.Title == ar.Title && a.Author == ar.Author);
                return tmp.id;
            }
        }
        catch (Exception ex)
        {
            return -1;
        }
    }

    /// <summary>
    /// 更新文章
    /// </summary>
    /// <param name="newAr">待更新文章对象</param>
    /// <returns>是否添加成功</returns>
    public static bool updateArticle(Article newAr)
    {
        try
        {
            using (var db = new PaperDbEntities())
            {
                Article oldAr = db.Article.Single(a => a.id == newAr.id);
                db.Entry(oldAr).CurrentValues.SetValues(newAr);
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
    /// 删除文章
    /// </summary>
    /// <param name="articleID">文章ID</param>
    /// <returns></returns>
    public static bool delArticle(int articleID)
    {
        try
        {
            using (var db = new PaperDbEntities())
            {
                Article ar = db.Article.SingleOrDefault(a => a.id == articleID);
                db.Article.Remove(ar);
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
    /// 根据ID返回一片文章
    /// </summary>
    /// <param name="articleId">文章ID</param>
    /// <returns>文章对象</returns>
    public static Article getArticleById(int articleId)
    {
        Article ar = new Article();
        try
        {
            using (var db = new PaperDbEntities())
            {
                ar = db.Article.Single(a => a.id == articleId);
                return ar;
            }
        }
        catch (Exception ex)
        {
            return ar;
        }

    }

    /// <summary>
    /// 添加关键字
    /// </summary>
    /// <param name="articleId">文章id</param>
    /// <param name="keywordId">关键字id 列表</param>
    /// <returns>是否成功</returns>
    public static bool addKeywords(int articleId, List<int> keywordId)
    {
        try
        {

            using (var db = new PaperDbEntities())
            {
                //增加所有关键字
                foreach (int id in keywordId)
                {
                    KeyWordConnection kc = new KeyWordConnection();
                    kc.ArticleId = articleId;
                    kc.KeyWordId = id;
                    db.KeyWordConnection.Add(kc);
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
    /// 获取所有关键字
    /// </summary>
    /// <returns>关键字对象列表</returns>
    public static List<KeyWords> getAllKeywords()
    {
        List<KeyWords> keywordList = new List<KeyWords>();
        try
        {
            using (var db = new PaperDbEntities())
            {
                keywordList = (from it in db.KeyWords orderby it.Name ascending select it ).ToList();
            }
            return keywordList;
        }
        catch (Exception ex)
        {
            return null;
        }

    }

    /// <summary>
    /// 获取文章的所有关键字
    /// </summary>
    /// <param name="articleId">文章ID</param>
    /// <returns>关键字对象字典</returns>
    public static Dictionary<int, string> getKeyowrd(int articleId)
    {
        Dictionary<int, string> keywordDict = new Dictionary<int, string>();
        try
        {
            List<KeyWords> keywordLs = new List<KeyWords>();
            using (var db = new PaperDbEntities())
            {
                keywordLs = (from it in db.KeyWordConnection
                             where it.ArticleId == articleId
                             join key in db.KeyWords on it.KeyWordId equals key.id
                             select key).ToList();
            }
            //转换成字典
            keywordDict.Clear();
            foreach (KeyWords k in keywordLs)
                keywordDict.Add(k.id, k.Name);
            return keywordDict;
        }
        catch (Exception ex)
        {
            return null;

        }

    }


    /// <summary>
    /// 删除文章关键字记录
    /// </summary>
    /// <param name="keywordConnId"></param>
    /// <returns></returns>
    public static bool delKeywordConnecion(List<int> keywordConnId)
    {
        try
        {
            using (var db = new PaperDbEntities())
            {
                foreach (int id in keywordConnId)
                {
                    KeyWordConnection kc = db.KeyWordConnection.Single(a => a.id == id);
                    db.KeyWordConnection.Remove(kc);
                }
                db.SaveChanges();
            }
            return true;
        }
        catch (Exception ex)
        {

            return false;
        }

    }


    /// <summary>
    /// 删除文章类型选项记录
    /// </summary>
    /// <param name="typeConnId"></param>
    /// <returns></returns>
    public static bool delTypeConnection(List<int> typeConnId)
    {
        try
        {
            using (var db = new PaperDbEntities())
            {
                foreach (int id in typeConnId)
                {
                    TypeConnection kc = db.TypeConnection.Single(a => a.id == id);
                    db.TypeConnection.Remove(kc);
                }
                db.SaveChanges();
            }
            return true;
        }
        catch (Exception ex)
        {

            return false;
        }
    }


    /// <summary>
    /// 添加各种选项
    /// </summary>
    /// <param name="articleId">文章ID</param>
    /// <param name="ops">一级选项名字，二级选项名字</param>
    /// <returns></returns>
    public static bool addOptions(int articleId, Dictionary<string, string> ops)
    {
        List<int> opsls = new List<int>();
        foreach (KeyValuePair<string, string> s in ops)
            opsls.Add(LabelHelper.getOptionId(s.Key, s.Value));
        try
        {

            using (var db = new PaperDbEntities())
            {
                //增加所有选项
                foreach (int id in opsls)
                {
                    TypeConnection tc = new TypeConnection();
                    tc.ArticleId = articleId;
                    tc.OptionId = id;
                    db.TypeConnection.Add(tc);
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
    /// 添加各种选项，通过给出optionId
    /// </summary>
    /// <param name="articleId"></param>
    /// <param name="opsId"></param>
    /// <returns></returns>
    public static bool addOptions(int articleId, List<int> opsId)
    {
        try
        {
            using (var db = new PaperDbEntities())
            {
                foreach (int i in opsId)
                {
                    TypeConnection tc = new TypeConnection();
                    tc.ArticleId = articleId;
                    tc.OptionId = i;
                    db.TypeConnection.Add(tc);
                }
                db.SaveChanges();
            }
            return true;
        }
        catch (Exception ex)
        {

            return false;
        }

    }


    /// <summary>
    /// 获取文章的各种选项
    /// </summary>
    /// <param name="articleId">文章ID</param>
    /// <returns>一级选项名，二级选项名的键值对</returns>
    public static Dictionary<string, string> getOptions(int articleId)
    {
        Dictionary<string, string> opsDict = new Dictionary<string, string>();
        try
        {
            using (var db = new PaperDbEntities())
            {
                var query = from it in db.TypeConnection
                            where it.ArticleId == articleId
                            join op in db.Option on it.OptionId equals op.id
                            join ca in db.Category on op.CategoryId equals ca.id
                            select new { firstLevel = ca.Name, secondLevel = op.Name };
                //转换到字典中
                opsDict.Clear();
                foreach (var k in query)
                    opsDict.Add(k.firstLevel, k.secondLevel);
            }
            return opsDict;
        }
        catch (Exception)
        {

            throw;
        }

    }


    /// <summary>
    /// 获取文章的所有关键字记录
    /// </summary>
    /// <param name="articleId"></param>
    /// <returns></returns>
    public static List<KeyWordConnection> getArticleKeywordConn(int articleId)
    {
        try
        {
            List<KeyWordConnection> kcLs = new List<KeyWordConnection>();
            using (var db = new PaperDbEntities())
            {
                kcLs = (from it in db.KeyWordConnection where it.ArticleId == articleId select it).ToList();
            }
            return kcLs;
        }
        catch (Exception ex)
        {

            return null;
        }
    }

    /// <summary>
    /// 获取文章的所有选项记录
    /// </summary>
    /// <param name="articleId"></param>
    /// <returns></returns>
    public static List<TypeConnection> getArticleTypeConn(int articleId)
    {
        try
        {
            List<TypeConnection> kcLs = new List<TypeConnection>();
            using (var db = new PaperDbEntities())
            {
                kcLs = (from it in db.TypeConnection where it.ArticleId == articleId select it).ToList();
            }
            return kcLs;
        }
        catch (Exception ex)
        {
            return null;
        }

    }

    /// <summary>
    /// 返回所有文章
    /// </summary>
    /// <returns></returns>
    public static List<Article> getAllArticle()
    {
        List<Article> arLs = new List<Article>();
        try
        {
            using (var db = new PaperDbEntities())
            {
                arLs = (from it in db.Article orderby it.Title ascending select it).ToList();
                return arLs;
            }
        }
        catch (Exception ex)
        {
            return arLs;
        }

    }


    /// <summary>
    /// 通过标题返回文章
    /// </summary>
    /// <param name="title">标题，模糊搜索</param>
    /// <returns></returns>
    public static List<Article> getArticleByTitle(string title)
    {
        List<Article> arLs = new List<Article>();
        try
        {
            using (var db = new PaperDbEntities())
            {
                arLs = (from it in db.Article
                        where it.Title.Contains(title)
                        orderby it.Title ascending
                        select it).Distinct().ToList();
                return arLs;
            }
        }
        catch (Exception ex)
        {
            return arLs;
        }

    }

    /// <summary>
    /// 通过作者返回文章
    /// </summary>
    /// <param name="author">作者，模糊搜索</param>
    /// <returns></returns>
    public static List<Article> getArticleByAuthor(string author)
    {
        List<Article> arLs = new List<Article>();
        try
        {
            using (var db = new PaperDbEntities())
            {
                arLs = (from it in db.Article where it.Author.Contains(author) orderby it.Title ascending select it).Distinct().ToList();
                return arLs;
            }
        }
        catch (Exception ex)
        {
            return arLs;
        }

    }


    /// <summary>
    /// 通过关键词返回文章
    /// </summary>
    /// <param name="keywordIds">关键词ID列表，模糊搜索</param>
    /// <returns></returns>
    public static List<Article> getArticleByKeyword(List<int> keywordIds)
    {
        List<int> keyConn = new List<int>();
        List<Article> arLs = new List<Article>();
        try
        {
            using (var db = new PaperDbEntities())
            {
                keyConn = db.KeyWordConnection.Where(a => keywordIds.Contains(a.KeyWordId)).OrderBy(a => a.ArticleId).Select(a => a.ArticleId).ToList();
                arLs = db.Article.Where(a => keyConn.Contains(a.id)).OrderBy(a => a.Title).Distinct().ToList();
                return arLs;
            }
        }
        catch (Exception ex)
        {
            return arLs;
        }

    }



    /// <summary>
    /// 混合条件查询中，从但条件查询中过滤出符合各种选项的文章
    /// </summary>
    /// <param name="ll"></param>
    /// <param name="opIds"></param>
    /// <returns></returns>
    public static List<Article> filterArticleByOptions(ref List<Article> ll, ref List<int> opIds)
    {
        try
        {
            using (var db = new PaperDbEntities())
            {

                List<int> arids = new List<int>();
                arids.Clear();
                foreach (Article ar in ll)
                    arids.Add(ar.id);
                //获取待过滤文章的所有 TypeConnection
                List<TypeConnection> tyls = db.TypeConnection.Where(a => arids.Contains(a.ArticleId)).ToList();
                List < Article > arls = new List<Article>();

                int opid0 = opIds[0];
                int opidNow = 0;
                HashSet<int> setId = new HashSet<int>(tyls.Where(a => a.OptionId == opid0).Select(a => a.ArticleId));// new HashSet<int>();
                HashSet<int> setTmp = null;
                for (int i = 1; i < opIds.Count; i++)
                {
                    //linq 不可以直接使用数组下标访问，需要用变量存一下
                    opidNow = opIds[i];
                    setTmp = new HashSet<int>(tyls.Where(a => a.OptionId == opidNow).Select(a => a.ArticleId));
                    setId.IntersectWith(setTmp);
                }

                arls = ll.Where(a => setId.Contains(a.id)).ToList();
                return arls ;
            }
        }


        catch (Exception ex)
        {
            return null;
        }
    }


    /// <summary>
    /// 通过高级选项返回文章
    /// </summary>
    /// <param name="opIds">高级选项的ID</param>
    /// <returns></returns>
    public static List<Article> getArticleByOptions(List<int> opIds)
    {
        try
        {
            List<Article> arls = new List<Article>();
            using (var db = new PaperDbEntities())
            {

                /***
                 * 
                 * 这个是是要包含就能查出来，不太好
                 * */
                /*
                 * List<int> arIds = db.TypeConnection.Where(a => opIds.Contains(a.OptionId)).Select(a => a.ArticleId).Distinct().ToList();
                arls = db.Article.Where(a => arIds.Contains(a.id)).ToList();
                */

                //查询每一个 optionID 可以获得一个 set
                //求所有 set 的广义交，结果就是多选项查询
                //set 访问速度O(1),但是求交的时候也需要遍历，速度取决于 optionId 的个数
                //目前没想到什么好的办法改进，可能程序可以优化，也可能数据库结构不好，导致现在这种查询尴尬的境地
                int opid0 = opIds[0];
                int opidNow = 0;
                HashSet<int> setId = new HashSet<int>(db.TypeConnection.Where(a=>a.OptionId==opid0).Select(a=>a.ArticleId));// new HashSet<int>();
                HashSet<int> setTmp = null;
                for (int i = 1; i < opIds.Count; i++)
                {
                    //linq 不可以直接使用数组下标访问，需要用变量存一下
                    opidNow = opIds[i];
                    setTmp = new HashSet<int>(db.TypeConnection.Where(a => a.OptionId == opidNow).Select(a => a.ArticleId));
                    setId.IntersectWith(setTmp);
                }

                arls = db.Article.Where(a => setId.Contains(a.id)).ToList();
            }
            return arls;
        }
        catch (Exception ex)
        {
            return null;
        }

    }


    /// <summary>
    /// 通过时间返回文章
    /// </summary>
    /// <param name="low">时间下限</param>
    /// <param name="high">时间上限</param>
    /// <returns></returns>
    public static List<Article> getArticleByTime(int low, int high)
    {
        DateTime lowYear = new DateTime(low, 1, 1);
        DateTime highYear = new DateTime(high, 1, 1);
        List<Article> arls = new List<Article>();
        try
        {
            using (var db = new PaperDbEntities())
            {
                arls = (from it in db.Article
                        where it.UpateTime <= highYear && it.UpateTime >= lowYear
                        select it).Distinct().ToList();
            }
            return arls;
        }
        catch (Exception ex)
        {
            return null;
        }

    }


    /// <summary>
    /// 返回给前台使用的文章数据
    /// </summary>
    public class Passage
    {
        public string title { get; set; }
        public string author { get; set; }
        public string keyword { get; set; }
        public string summary { get; set; }
        public string link { get; set; }
        public string time { get; set; }
        public string journal { get; set; }

    }

    /// <summary>
    /// 返回给前台使用的文章数据
    /// </summary>
    public class DataPackage
    {
        public int totalCnt { get; set; }
        public int nowPage { get; set; }
        public List<Passage> info { get; set; }
    }

    /// <summary>
    /// 这里可能严重影响性能，由于是查出所有的数据然后再提取多少条
    /// </summary>
    /// <param name="arls"></param>
    /// <param name="currentPage"></param>
    /// <returns></returns>
    public static DataPackage generateDataPackage(ref List<Article> arls, int currentPage)
    {
        DataPackage dp = new DataPackage();
        List<Passage> psLs = new List<Passage>();
        dp.totalCnt = arls.Count;
        arls = arls.Skip(currentPage * WEBCONFIG.ARTICLE_EVERYPAGE_COUNT).Take(WEBCONFIG.ARTICLE_EVERYPAGE_COUNT).ToList();
        foreach (Article ar in arls)
        {
            Passage ps = new Passage();
            ps.title = ar.Title;
            ps.author = ar.Author;
            ps.summary = ar.Summary;
            ps.link = ar.Link;
            ps.time = ar.UpateTime.Date.Year.ToString();
            ps.journal = ar.Journal;
            string keyword = string.Empty;//;连接
            foreach (KeyValuePair<int, string> kp in ArticleHelper.getKeyowrd(ar.id))
            {
                keyword += ";" + kp.Value;
            }
            ps.keyword = keyword.Substring(1);
            psLs.Add(ps);
        }
        
        dp.nowPage = currentPage;
        dp.info = psLs;
        return dp;
    }
}
