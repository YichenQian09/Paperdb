using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// WEBCONFIG 服务器静态变量
/// </summary>
public class WEBCONFIG
{
    public WEBCONFIG()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// 文章时间列表里面最小的年份
    /// </summary>
    public static int ARTICLE_YEAR_LOW = 2000;


    /// <summary>
    /// 文章结果集每页显示数量
    /// </summary>
    public static int ARTICLE_EVERYPAGE_COUNT = 12;


    /// <summary>
    /// 管理员账号密码
    /// </summary>
    public static string ADMIN_USERNAME = "MarketAdmin";
    public static string ADMIN_PASSWORD = "MarketManager";
    //public static string ADMIN_USERNAME = "1";
    //public static string ADMIN_PASSWORD = "1";


    /// <summary>
    /// 多数据分割符
    /// </summary>
    public static string DATA_SEPERATOR = ";";

    
}