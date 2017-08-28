<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerSport.aspx.cs" Inherits="WebApplication1.Webs.CustomerSport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>运动统计</title>
    <script type="text/javascript">
        var jsonStr = '<%=jsonStr%>'
        var cid = '<%=cid%>';
    </script>
    <script src="../Scripts/jquery-2.0.0.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <link href="../Content/bootstrap.css" rel="stylesheet" />

    <script src="../Scripts/datetimepicker/bootstrap-datetimepicker.js"></script>
    <link href="../Scripts/datetimepicker/bootstrap-datetimepicker.min.css" rel="stylesheet" />

    <link href="../Scripts/checkboxStyle/skins/minimal/minimal.css" rel="stylesheet" />
    <script src="../Scripts/checkboxStyle/icheck.js"></script>

    <script src="../Scripts/ichart/ichart.1.2.min.js"></script>

    <script src="/Scripts/base.js?ver=<%=ran %>"></script>
    <script src="/Scripts/webs/customerSport.js?ver=<%=ran %>"></script>
    <link href="/Content/base.css" rel="stylesheet" />
    <link href="/Content/webs/customerSport.css" rel="stylesheet" />

</head>
<body>
    <div id="divTop" class="row">
        <div id="divTopLeft">
            <font>系统管理平台</font>
        </div>
        <div id="divTopRight">
            <ul id="ulTop">
                <li>
                    <img class="iconTop" src="/Images/recipe/icon-admin.svg" alt="Alternate Text" />
                    <font>admin</font>
                </li>
                <li>
                    <img class="iconTop2" src="/Images/recipe/icon-bell.svg" alt="Alternate Text" />
                </li>
                <li>
                    <img class="iconTop2" src="/Images/recipe/icon-exit.svg" alt="Alternate Text" />
                </li>
            </ul>
        </div>
    </div>

    <div id="divMain" class="row">
        <div class="" id="divNav">

            <ul class="list-group">
                <li class="">
                    <img src="/Images/recipe/icon-food.svg" /><a href="Food.aspx">菜品管理</a>
                </li>
                <li class="">
                    <img src="/Images/recipe/icon-foodtype.svg" /><a href="FoodType.aspx">菜品分类</a>
                </li>
                <li class="">
                    <img src="/Images/recipe/icon-tag.svg" /><a href="Tag.aspx">标签管理</a>
                </li>
                <li class="">
                    <img src="/Images/recipe/icon-rest.svg" /><a href="Restaurant.aspx">餐厅管理</a>
                </li>
                <li class="">
                    <img src="/Images/recipe/icon-recipe.svg" /><a href="Recipe.aspx">食谱管理</a>
                </li>
                <li class="">
                    <img src="/Images/recipe/icon-article.svg" /><a href="Article.aspx">文章管理</a>
                </li>

                <li class="li-allques">
                    <img src="/Images/base/icon-triangle-right.svg" /><a>问卷管理</a>
                </li>
                <li class="li-ques">
                    <img src="/Images/base/icon-easyQues.svg" /><a href="QuestionEasy.aspx">简易版</a></li>
                <li class="li-ques">
                    <img src="/Images/base/icon-proQues.svg" /><a href="QuestionPro.aspx">专业版</a></li>
                <li class="li-allbacks">
                    <img src="/Images/base/icon-triangle-right.svg" /><a>商家后台</a>
                </li>
                <li class="li-back">
                    <img src="/Images/base/icon-order.svg" /><a href="Order.aspx">用户订单</a></li>
                <li class="li-back">
                    <img src="/Images/base/icon-balance.svg" /><a href="Balance.aspx">钱包余额</a></li>
                <li class="li-back">
                    <img src="/Images/base/icon-recipeSet.svg" /><a href="RecipeSet.aspx">食谱缺货设置</a></li>
                <li class="">
                    <img src="/Images/base/icon-account.svg" /><a href="Seller.aspx">商家账号设置</a>
                </li>
                <li class="active">
                    <img src="/Images/base/icon-customer.svg" /><a href="Customer.aspx">会员管理</a>
                </li>
                <li class="">
                    <img src="/Images/base/icon-withdraw-white.svg" style="" /><a href="Withdraw.aspx">提现申请</a>
                </li>

            </ul>

        </div>
        <div class="" id="divMains">
            <div class="row" id="divMain1">
                <div id="main1-left">
                    <h4>运动统计</h4>
                </div>
                <div id="main1-right">
                    <a class="btn btn-base">
                        <img src="/Images/base/icon-left-arrow.svg" alt="Alternate Text" />
                        <font>返回</font>
                    </a>
                </div>
            </div>
         

            <div class="row" id="divMain3">
                <div class="main3-div1">
                    <div class="div1-left fl">
                        <font class="fl div1-font1" >会员:</font><font class="fl div1-font2"><%=name %></font>
                        <div class="fc"></div>
                    </div>
                    <div class="div1-right fr">
                        <a class="datetime dtDay fl" style="color:#49425b"><font>日</font>
                            <img class="icon-uptriangle" src="/Images/base/icon-up-triangle.svg" alt="Alternate Text" />
                        </a>
                        <a class="datetime dtMonth fl" ><font>月</font>
                               <img class="icon-uptriangle" src="/Images/base/icon-up-triangle.svg" alt="Alternate Text" style="display:none"/>
                        </a>
                        <a class="datetime dtYear fl" ><font>年</font>
                               <img class="icon-uptriangle" src="/Images/base/icon-up-triangle.svg" alt="Alternate Text" style="display:none" />
                        </a>
                        <div class="fc"></div>
                    </div>
                    <div class="fc"></div>
                </div>
                <div class="main3-div2">
                    <div class="div-year fl">
                    <font>时间:</font>
                    <select class="select-year">
                        <%--<option value="value">2017</option>--%>
                    </select>
                    <font>年</font>
                        </div>
                    <div class="div-month fl">
                    <select class="select-month">
                        <option value="1">1</option>
                         <option value="2">2</option>
                         <option value="3">3</option>
                         <option value="4">4</option>
                         <option value="5">5</option>
                         <option value="6">6</option>
                         <option value="7">7</option>
                         <option value="8">8</option>
                         <option value="9">9</option>
                         <option value="10">10</option>
                         <option value="11">11</option>
                         <option value="12">12</option>
                    </select>
                    <font>月</font>
                        </div>
                    <div class="fc"></div>
                </div>
                <div  class="main3-div3">
                     <div id="canvasDiv"></div>
                </div>
               
            </div>

    
        </div>
        <div style="clear: both"></div>
    </div>

</body>
</html>
