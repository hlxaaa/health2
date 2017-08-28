<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Withdraw.aspx.cs" Inherits="WebApplication1.Webs.Withdraw" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>提现申请</title>
    <script type="text/javascript">
        var jsonStr = '<%=jsonStr%>'
        var id ='<%=sellerId%>'
    </script>
    <script src="../Scripts/jquery-2.0.0.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <link href="../Content/bootstrap.css" rel="stylesheet" />

     <script src="../Scripts/datetimepicker/bootstrap-datetimepicker.js"></script>
    <link href="../Scripts/datetimepicker/bootstrap-datetimepicker.min.css" rel="stylesheet" />

    <link href="../Scripts/checkboxStyle/skins/minimal/minimal.css" rel="stylesheet" />
    <script src="../Scripts/checkboxStyle/icheck.js"></script>

    <script src="/Scripts/base.js?ver=<%=ran %>"></script>
    <script src="/Scripts/webs/withdraw.js?ver=<%=ran %>"></script>
    <link href="/Content/base.css" rel="stylesheet" />
    <link href="/Content/webs/withdraw.css" rel="stylesheet" />
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
                <li class="">
                    <img src="/Images/base/icon-customer.svg" /><a href="Customer.aspx">会员管理</a>
                </li>
                <li class="active">
                    <img src="/Images/base/icon-withdraw-white.svg" style="" /><a href="Withdraw.aspx">提现申请</a>
                </li>

            </ul>

        </div>
        <div class="" id="divMains">
            <div class="row" id="divMain1">
                <div id="main1-left">
                    <h4>提现申请</h4>
                </div>
            </div>

            <div class="row" id="divMain3">
                <div class="table-wrap">
                    <table class="table table-striped table-hover">
                        <thead class="thead1">
                            <tr>
                                <th width="3%">编号</th>
                                <th width="3%">商家</th>
                                <th width="3%">余额</th>
                                <th width="3%">提现金额</th>
                                <th width="3%">时间</th>
                                <th width="1%">操作</th>
                                <th width="8%"></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr style="background-color: white;">
                                <td>3216542
                                </td>
                                <td>商家
                                </td>
                                <td>食谱啊
                                </td>
                                <td class="td-money">
                                    2500.00
                                </td>
                                <td>2016-10-01 21:02:59
                                </td>
                                <td>
                                    <span class="btn-do">处理</span>
                                </td>
                                <td></td>
                            </tr>
                                
                        </tbody>
                    </table>
                </div>
            </div>
            <nav style="text-align:center;display:block">
                        <ul class="pagination"><li class="disabled"><a href="#" style="text-decoration: none;">«</a></li><li class="active"><a style="text-decoration: none;">1</a></li><li onclick="getPage(this)"><a style="text-decoration: none;">2</a></li><li onclick="getPage(this)"><a style="text-decoration: none;">3</a></li><li onclick="getNextPage()"><a style="text-decoration: none;">»</a></li></ul>

                    </nav>
        </div>
    </div>


</body>
</html>
