<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/manager.master" AutoEventWireup="true" CodeFile="articleList.aspx.cs" Inherits="Manager_articleList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <h1>管理文章</h1>
    <p>
        检索文章：<asp:TextBox ID ="txtTitle" runat ="server" MaxLength="60" placeholder="请输入文章标题"></asp:TextBox>
        <asp:Button ID="btnSearch" runat ="server" Text="检索" OnClick="btnSearch_Click" />
    </p>
    <div>
   

        <asp:GridView ID="gdvArticle" runat ="server" AllowPaging="True"  PageSize="20"  AutoGenerateColumns="False"    OnRowCommand="gdvArticle_RowCommand" OnPageIndexChanging="gdvArticle_PageIndexChanging" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical" ItemType="Article" >
        <AlternatingRowStyle BackColor="#CCCCCC" />
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemTemplate>
                    <span><%# this.gdvArticle.PageIndex * this.gdvArticle.PageSize + this.gdvArticle.Rows.Count + 1%></span>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="title" HeaderText="文章标题" ReadOnly="True" SortExpression="name" />
            <asp:BoundField DataField="author" HeaderText="作者" ReadOnly="True" />
            <asp:TemplateField HeaderText="修改">
               <ItemTemplate>
                  <a href="<%# "addArticle.aspx?id=" +Item.id %>">修改</a>
               </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="删除">
                <ItemTemplate>
                    <asp:LinkButton runat ="server" OnClientClick='return confirm("此操作会删除该文章，确定要删除此文章吗？")' CommandName="delAritcle" CommandArgument='<%# Item.id%>'>删除</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <FooterStyle BackColor="#CCCCCC" />
        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#F1F1F1" />
        <SortedAscendingHeaderStyle BackColor="#808080" />
        <SortedDescendingCellStyle BackColor="#CAC9C9" />
        <SortedDescendingHeaderStyle BackColor="#383838" />
    </asp:GridView>
       


    </div>
</asp:Content>

