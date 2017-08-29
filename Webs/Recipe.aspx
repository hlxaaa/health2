<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Recipe.aspx.cs" Inherits="WebApplication1.Webs.Recipe" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script type="text/javascript">
        var jsonStr = '<%=jsonStr%>'
    </script>
    <script src="../Scripts/jquery-2.0.0.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <%--<link href="../Content/bootstrap.min.css" rel="stylesheet" />--%>
    <link href="../Content/bootstrap.css" rel="stylesheet" />

    <script src="../Scripts/datetimepicker/bootstrap-datetimepicker.js"></script>
    <link href="../Scripts/datetimepicker/bootstrap-datetimepicker.min.css" rel="stylesheet" />

    <link href="../Scripts/checkboxStyle/skins/minimal/minimal.css" rel="stylesheet" />
    <script src="../Scripts/checkboxStyle/icheck.js"></script>

    <script src="/Scripts/base.js?ver=<%=ran %>"></script>
    <script src="../Scripts/webs/recipe.js?ver=<%=ran %>"></script>

    <link href="/Content/base.css" rel="stylesheet" />
        <link href="../Content/webs/recipe.css" rel="stylesheet" />

    <title>食谱管理</title>
    <script>

    </script>
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
                    <font>管理员</font>
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
                <li class="active">
                    <img src="/Images/recipe/icon-recipe.svg" /><a href="Recipe.aspx">食谱管理</a>
                </li>
                <li class="">
                    <img src="/Images/recipe/icon-article.svg" /><a href="Article.aspx">文章管理</a>
                </li>
               
               <li class="li-allques">
                    <img src="/Images/base/icon-triangle-right.svg" /><a>问卷管理</a>
                </li>
               <li class="li-ques"><img src="/Images/base/icon-easyQues.svg" /><a href="QuestionEasy.aspx">简易版</a></li>
                <li class="li-ques"><img src="/Images/base/icon-proQues.svg" /><a href="QuestionPro.aspx">专业版</a></li>
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
                <li class="">
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
                    <h4>食谱管理</h4>
                </div>
                <div id="main1-right">
                    <a  class="btn btn-this batchDelete">
                        <img src="/Images/recipe/icon-deleteSome.svg" /><font>批量删除</font></a>
                    <a  class="btn btn-this" href="RecipeContent.aspx">
                        <img src="/Images/recipe/icon-add.png" /><font>增加</font></a>
                </div>
            </div>
            <div class="row row-min" id="divMain2">
                <%--改这里 --%>
                <div class="row">
                    <div class="row1-left">
                        <font>关键词:</font>
                    </div>
                    <div class="row1-mid">
                        <input id="inputSearch" />
                        <div class="btnSearch">
                            <img class="btn-search" src="/Images/recipe/icon-search.svg" />
                        </div>
                    </div>
                    <div class="row1-right">
                        <a id="btnMoreSelect" class="dropdown" href="#">高级<span class="caret"></span></a>
                    </div>
                </div>
                <div class="div-selector">
                <div class="row">
                    <div class="row2-left">
                        <font>是否有货:</font>
                        <label>
                            有
                            <input type="radio" name="iCheck" id="radioHas" value="True">
                            <%--<input type="radio" name="optionsRadios" id="radioHas" value="True">--%>
                        </label>
                        <label>
                            没有
                            <input type="radio" name="iCheck" id="radioNo" value="False">
                        </label>
                    </div>
                    <div class="row2-mid">
                        <font>销售量:</font>
                        <input onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" maxlength="8"/>
                        <%--<img src="/Images/line.svg" alt="Alternate Text" />--%>
                        <%--<span>—</span>--%>
                        <img src="/Images/base/icon-line.svg" alt="Alternate Text" class="img-line"/>
                        <input onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" maxlength="8"/>
                    </div>
                    <div class="row2-right">
                        <font>价格:</font>
                        <input onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" maxlength="8"/>
                        <img src="/Images/base/icon-line.svg" alt="Alternate Text" class="img-line"/>
                        <input onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" maxlength="8"/>
                    </div>
                </div>
                <div class="row3">
                    <div class="row3-left">
                        <font>标签:</font>
                    </div>
                    <div class="row3-right">
                        <%foreach (var v in dictTags.Values)
                          { %>
                        <div class="div-tag">
                            <input type="checkbox" class="tagcheckbox" />
                            <font><%=v %></font>
                        </div>
                        <%} %>
                    </div>
                </div>
                <div class="row4">
                    <font>创建时间:</font>
                    <div>
                        <input type="text" id="inputDate1" />
                    </div>
                    <span>—</span>
                    <div>
                        <input type="text" id="inputDate2" />
                    </div>
                </div>
                <div class="row5">
                    <div class="row5-left ">
                        <font>菜品:</font>
                    </div>
                    <div class="row5-right ">
                        <%foreach (var v in dictFood.Values)
                          { %>
                        <span onclick="clickFood(this)" class="label label-default"><%=v %></span>
                        <%} %>
                        <%--       <%foreach(var v in dictFood.Values){ %>
                        <span onclick="clickFood(this)" class="label label-default">测试</span>
                        <%} %>--%>
                    </div>
                </div>
                    </div>
            </div>
            <div class="row" id="divMain3">
            </div>

        </div>
    </div>


</body>
</html>
