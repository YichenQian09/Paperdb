<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/manager.master" AutoEventWireup="true" CodeFile="addArticle.aspx.cs" Inherits="Manager_addArticle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        li {
            float:left;
            list-style:none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
    <h1>添加文章</h1>
     
    标题：<asp:TextBox ID="txtTitle" runat ="server" MaxLength="60" placeholder="请输入不超过60字内容"></asp:TextBox><br />
    作者：<asp:TextBox ID="txtAuthor" runat ="server" MaxLength="40" placeholder="请输入不超过40字内容"></asp:TextBox><br />
    时间：<asp:DropDownList ID="dplTime" runat ="server" ></asp:DropDownList><br />
    摘要：<asp:TextBox ID ="txtSummary" runat ="server" TextMode="MultiLine"  Height="120px"></asp:TextBox><br />
    <div class="keywordContainer">关键字：<span style="color:red">必选项</span><ul class="addKeyword clearfix"></ul>
         <select class="keyword">
        <asp:Repeater ID="rptKeywords" runat ="server" >
            <ItemTemplate>
            <option id='<%# Eval("id") %>' > <%# Eval("name") %></option>
            </ItemTemplate>
        </asp:Repeater>
        </select>
        <input type="button"  value="添加关键字" onclick="addKeyWord()"/>
    </div>
    链接：<asp:TextBox ID="txtLink" runat ="server" placeholder="请给出完整链接（E.G. http(s)://xxx）"  MaxLength="2048"></asp:TextBox><br />
    期刊：<asp:TextBox ID="txtJournal" runat ="server" placeholder="请填写文章期刊" MaxLength="2048"></asp:TextBox><br />

    <div>
        地区：<asp:DropDownList ID="dplDQ" runat ="server" ></asp:DropDownList><br />
        生态系统类型：<asp:DropDownList ID="dplSTXTLX" runat ="server" ></asp:DropDownList><br />
        <p>
            <p class="firstType">文献类型：</p>
            文献综述：<asp:DropDownList ID ="dplWXZS" runat ="server" ></asp:DropDownList><br />
            一级评估：<asp:DropDownList ID="dplYJPG" runat ="server" ></asp:DropDownList><br />
            一级评估：<asp:DropDownList ID="dplEJPG" runat ="server" ></asp:DropDownList><br />
        </p>
        生态系统服务类型：<asp:DropDownList ID="dplSTXTFWLX" runat ="server" ></asp:DropDownList><br />
        方法：<asp:DropDownList ID="dplMX" runat ="server" ></asp:DropDownList><br />
        <p>
            非市场价值类型：
            <asp:DropDownList ID="dplFSCJZLX" runat ="server" ></asp:DropDownList><br />
        </p>
        <asp:Button ID="btnSubmit" runat ="server" Text="提交"  OnClick="btnSubmit_Click"/>
    </div>

            <script>
                $(document).ready(function () {
                    //如果页面是修改页面，这段 JS 用来绑定文章关键字
                    function bindArticleKeywords() {
                        var articleID = window.location.href.split('=');
                        if (articleID.length > 1) {
                            var keywordJson ='<%=executeJS%>';
                            var pairs = keywordJson.replace(/\"/g, "").replace("{", '').replace("}", '').split(',');
                            for (var i = 0; i < pairs.length; i++) {
                                var aPair = pairs[i].split(':');
                                $(".addKeyword").append("<li><input type='checkbox' checked='checked' name='keywords' value='" + aPair[0] + "'/>" + aPair[1] + "</li>")
                            }
                        }
                    }

                    bindArticleKeywords();
                });
        </script>
</asp:Content>

