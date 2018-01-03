<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/manager.master" AutoEventWireup="true" CodeFile="manageLabel.aspx.cs" Inherits="Manager_manageLabel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>标签管理页面</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>管理关键字</h1>

    功能：<asp:DropDownList ID="dplManageLabel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dplManageLabel_SelectedIndexChanged">
        <asp:ListItem>关键字管理</asp:ListItem>
        <asp:ListItem>一级分类管理</asp:ListItem>
        <asp:ListItem>二级分类管理</asp:ListItem>
    </asp:DropDownList>

    <asp:Panel ID="panel_keywords" runat="server" Visible="true">
        <p>
            添加关键字：<asp:TextBox ID="txtKeyword" runat="server" MaxLength="20"></asp:TextBox>
            <asp:Button ID="btnKeyword" runat="server" Text="添加" OnClick="btnKeyword_Click" />
        </p>
        <div>
         

            <asp:GridView ID="gdvKeywords" runat ="server" AllowPaging="True"  PageSize="20"  AutoGenerateColumns="False"   OnRowCommand="gdvKeywords_RowCommand"  OnPageIndexChanging="gdvKeywords_PageIndexChanging" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical" ItemType="KeyWords" >
        <AlternatingRowStyle BackColor="#CCCCCC" />
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemTemplate>
                    <span><%# this.gdvKeywords.PageIndex * this.gdvKeywords.PageSize + this.gdvKeywords.Rows.Count + 1%></span>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="name" HeaderText="关键字" ReadOnly="True" SortExpression="name" />
            <asp:TemplateField HeaderText="删除">
                <ItemTemplate>
                    <asp:LinkButton  runat ="server" OnClientClick='return confirm("此操作会删除该关键字，确定要删除此关键字吗？")' CommandName="delKeyword" CommandArgument='<%# Item.id%>'>删除</asp:LinkButton>
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
    </asp:Panel>

    <asp:Panel ID="panel_firstLevel" runat="server" Visible="false">

        <p>
            添加一级选项：<asp:TextBox ID="txtFirstLevel" runat="server" MaxLength="60"></asp:TextBox>
            <asp:Button ID="btnFirstLevel" runat="server" Text="添加" OnClick="btnFirstLevel_Click" />
        </p>
        <div>
                  <asp:GridView ID="gdvFirstLevel" runat ="server" AllowPaging="True"  PageSize="20"  AutoGenerateColumns="False"   OnRowCommand="gdvFirstLevel_RowCommand" OnPageIndexChanging="gdvFirstLevel_PageIndexChanging" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical" ItemType="Category" >
        <AlternatingRowStyle BackColor="#CCCCCC" />
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemTemplate>
                    <span><%# this.gdvFirstLevel.PageIndex * this.gdvFirstLevel.PageSize + this.gdvFirstLevel.Rows.Count + 1%></span>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="name" HeaderText="一级选项" ReadOnly="True" SortExpression="name" />
            <asp:TemplateField HeaderText="删除">
                <ItemTemplate>
                    <asp:LinkButton  runat ="server" OnClientClick='return confirm("此操作会删除该一级选项以及属于本选项下的所有二级选项，确定要删除此一级选项吗？")' CommandName="delFirstLevel" CommandArgument='<%# Item.id%>'>删除</asp:LinkButton>
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
    </asp:Panel>

    <asp:Panel ID="panel_secondLevel" runat="server" Visible="false">
<p>
    一级选项：<asp:DropDownList ID="dplFirstLevel" runat ="server" AutoPostBack="true" OnSelectedIndexChanged="dplFirstLevel_SelectedIndexChanged"></asp:DropDownList>
</p>
        <p>
            本一级选项下添加二级选项：<asp:TextBox ID="txtSecondLevel" runat="server" MaxLength="60"></asp:TextBox>
            <asp:Button ID="btnSecondLevel" runat="server" Text="添加" OnClick="btnSecondLevel_Click" />
        </p>
        <div>
                        <asp:GridView ID="gdvSecondLevel" runat ="server" AllowPaging="True"  PageSize="20"  AutoGenerateColumns="False"   OnRowCommand="gdvSecondLevel_RowCommand" OnPageIndexChanging="gdvSecondLevel_PageIndexChanging" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical" ItemType="Option" >
        <AlternatingRowStyle BackColor="#CCCCCC" />
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemTemplate>
                    <span><%# this.gdvSecondLevel.PageIndex * this.gdvSecondLevel.PageSize + this.gdvSecondLevel.Rows.Count + 1%></span>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="name" HeaderText="二级选项" ReadOnly="True" SortExpression="name" />
            <asp:TemplateField HeaderText="删除">
                <ItemTemplate>
                    <asp:LinkButton  runat ="server" OnClientClick='return confirm("此操作会删除该二级选项，确定要删除此二级选项吗？")' CommandName="delSecondLevel" CommandArgument='<%# Item.id%>'>删除</asp:LinkButton>
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
    </asp:Panel>


</asp:Content>

