<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecipeSet.aspx.cs" Inherits="WebApplication1.Webs.RecipeSet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>食谱缺货设置</title>
    <script type="text/javascript">
        var jsonStr = '<%=jsonStr%>'
    </script>
    <script src="../Scripts/jquery-2.0.0.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <link href="../Content/bootstrap.css" rel="stylesheet" />

    <script src="../Scripts/datetimepicker/bootstrap-datetimepicker.js"></script>
    <link href="../Scripts/datetimepicker/bootstrap-datetimepicker.min.css" rel="stylesheet" />

    <link href="../Scripts/checkboxStyle/skins/minimal/minimal.css" rel="stylesheet" />
    <script src="../Scripts/checkboxStyle/icheck.js"></script>

    <script src="/Scripts/base.js?ver=<%=ran %>"></script>
    <script src="/Scripts/webs/recipeSet.js?ver=<%=ran %>"></script>
    <link href="/Content/base.css" rel="stylesheet" />
    <link href="/Content/webs/recipeSet.css" rel="stylesheet" />

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
                    <font><%=loginName %></font>
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
                  <%if(isAdmin==true){ %>
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
                <%} %>
                <li class="li-allbacks">
                    <img src="/Images/base/icon-triangle-down.svg" /><a>商家后台</a>
                </li>
                <li class="li-back li-back-open">
                    <img src="/Images/base/icon-order.svg" /><a href="Order.aspx">用户订单</a></li>
              <%if(isAdmin==true){ %>
                 <li class="li-back li-back-open ">
                    <img src="/Images/base/icon-balance.svg" /><a href="Balance.aspx">钱包余额</a></li>
                <%}else{ %>
                 <li class="li-back li-back-open ">
                    <img src="/Images/base/icon-balance.svg" /><a href="BalanceSeller.aspx?sellerId=<%=id %>">钱包余额</a></li>
                <%} %>
                <li class="li-back li-back-open active">
                    <img src="/Images/base/icon-recipeSet.svg" /><a href="RecipeSet.aspx">食谱缺货设置</a></li>
                  <%if(isAdmin==true){ %>
                <li class="">
                    <img src="/Images/base/icon-account.svg" /><a href="Seller.aspx">商家账号设置</a>
                </li>
                <li class="">
                    <img src="/Images/base/icon-customer.svg" /><a href="Customer.aspx">会员管理</a>
                </li>
                <li class="">
                    <img src="/Images/base/icon-withdraw-white.svg" style="" /><a href="Withdraw.aspx">提现申请</a>
                </li>
                <%} %>
            </ul>

        </div>
        <div class="" id="divMains">
            <div class="row" id="divMain1">
                <div id="main1-left">
                    <h4>商家后台-食谱缺货设置</h4>
                </div>

            </div>
            <div class="row row-min" id="divMain2">
                <%--改这里 --%>
                <div class="row">
                    <%if(isAdmin==true){ %>
                    <div class="row1-left">
                        <font>选择餐厅:</font>
                    </div>
                    
                    <div class="row1-mid">
                        <select>
                            <option value="" >全部</option>
                            <%foreach(var k in dictRest.Keys){ 
                                  if(k==id){
                                  %>
                            <option value="<%=k %>" selected="selected"><%=dictRest[k] %></option>
                            <%}else{ %>
                            <option value="<%=k %>" ><%=dictRest[k] %></option>
                            <%} }%>
                        </select>
                    </div>
                    <%} %>
                    <div class="row1-right fr">
                        <font class="fl">关键词:</font>
                        <input class="fl" id="inputSearch" />
                        <div class="btnSearch fl">
                            <img class="btn-search" src="/Images/recipe/icon-search.svg" />
                        </div>
                        <div class="fc"></div>
                    </div>
                </div>
            </div>

            <div class="row" id="divMain3">
                <div class="partline line1"></div>
                 <div class="partline line2"></div>
                 <div class="partline line3"></div>
                
<%--                <div class="recipe fl">
                    <input type="hidden"/>
                    <font class="fl bold">食谱1</font>
                    <div class="fr">
                        <font>有货</font>
                        <input type="radio" name="recipe" checked="checked" />
                        <font>缺货</font>
                        <input type="radio" name="recipe" />
                    </div>
                </div>--%>
       
                <div class="fc"></div>
            </div>

        <%--    <nav style="text-align: center; display: block"><ul class="pagination"><li class="disabled"><a style="text-decoration: none;">«</a></li><li class="active"><a style="text-decoration: none;">1</a></li><li onclick="getPage(this)"><a style="text-decoration: none;">2</a></li><li onclick="getNextPage()"><a style="text-decoration: none;">»</a></li></ul></nav>--%>
        </div>
        <div style="clear: both"></div>
    </div>

</body>
</html>
