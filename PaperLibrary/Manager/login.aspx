<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="Manager_login" %>

<!DOCTYPE html>
<html lang="en">
<head>
	<meta charset="UTF-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge" />
	<title>后台管理</title>
	<link rel="stylesheet" href="css/login-backstage.css">
	<script src="../js/jquery-3.1.1.min.js"></script>
</head>
<body >
	<div class="login-container">
		<div class="blank"></div>
		<img class="title-chinese" src="images/loginTitle-chinese.png" alt="">
		<img class="title-english" src="images/loginTitle-english.png" alt="">
		<form runat ="server" >
        <div class="login-main">
			<div class="username clearfix">
				<img src="images/account.png" alt="">
				<%--<input type="text">--%>
                <asp:TextBox id="txtUsername" runat ="server" MaxLength="20"  placeholder="请输入用户名" ></asp:TextBox>
			</div>
			<div class="password clearfix">
				<img src="images/password.png" alt="">
				<%--<input type="text">--%>
                <asp:TextBox ID="txtPassword" runat ="server" MaxLength="20" placeholder="请输入密码" TextMode="Password"></asp:TextBox>
			</div>
			<div class="yanzheng clearfix">
				<div class="yanzhengInput">
					<img src="images/yanzheng.png" alt="">
					<%--<input type="text">--%>
                    <asp:TextBox ID="txtValidate" runat ="server"  MaxLength="6" placeholder="请输入验证码"></asp:TextBox>
				</div>
				<div class="yanzhengImg"><img src="validate.aspx?" class="verify"></div>
			</div>
			<div class="choice clearfix">
				<%--<button class="log-in">登录</button>--%>
                <asp:Button ID="btnSubmit" runat ="server" Text="登录" CssClass="log-in" OnClick="btnSubmit_Click" />
				<a class="jump-search" href="/">进入首页</a>
			</div>
			</div>
            </form>
	</div>
	<script src="js/login-backstage.js"></script>
</body>
</html>