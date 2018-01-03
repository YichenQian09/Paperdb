using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// JSHelper 的摘要说明
/// </summary>
public class JSHelper
{
    public JSHelper()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// JS弹窗提示
    /// </summary>
    /// <param name="content">提示内容</param>
    /// <returns>JS 字符串</returns>
    public static string alert(string content)
    {
        return "<script>alert('" + content + "');</script>";
    }

    /// <summary>
    /// JS 弹窗并且跳转到指定网址
    /// </summary>
    /// <param name="content">提示内容</param>
    /// <param name="url">跳转目标网址</param>
    /// <returns>JS 字符串</returns>
    public static string alert(string content, string url)
    {
        return "<script>alert('" + content + "');location.href='"+url+ "';</script>";
    }
}