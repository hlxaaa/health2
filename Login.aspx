<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication1.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/bootstrap.css" rel="stylesheet" />
    <%--<link href="../Content/seller.css" rel="stylesheet" />--%>
    <script src="../Scripts/jquery-2.0.0.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/login.js"></script>
    <link href="/Content/login.css" rel="stylesheet" />
    <link href="/Content/base.css" rel="stylesheet" />
    <title>登录</title>
</head>
<body>
    <div id="loginPanel">
        <div class="panel-title">
            修身系统管理平台
        </div>
        <div class="panel-account">
            <input type="text" name="account" id="account" value="" placeholder="用户名" />
        </div>
        <div class="panel-password">
            <input type="password" name="password" id="password" value="" placeholder="密码" />
        </div>
        <div class="panel-vcode">
            <input type="text" name="vcode" value="" id="vCode" placeholder="验证码" />
            <img src="/Webs/VerificateCode.aspx" alt="下一张" class="img-vcode" onclick="this.src=this.src" />
        </div>
        <div class="panel-login">
            <a class="btn-login" onclick="login()">登录</a>
        </div>
    </div>
</body>
</html>
