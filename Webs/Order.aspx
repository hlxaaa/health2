<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Order.aspx.cs" Inherits="WebApplication1.Webs.seller" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script type="text/javascript">
        var isAdmin = '<%=isAdmin%>'
    </script>
    <script src="../Scripts/jquery-2.0.0.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <%--<link href="../Content/bootstrap.min.css" rel="stylesheet" />--%>
    <link href="../Content/bootstrap.css" rel="stylesheet" />

    <script src="../Scripts/datetimepicker/bootstrap-datetimepicker.js"></script>
    <link href="../Scripts/datetimepicker/bootstrap-datetimepicker.min.css" rel="stylesheet" />

    <link href="../Scripts/checkboxStyle/skins/minimal/minimal.css" rel="stylesheet" />
    <script src="../Scripts/checkboxStyle/icheck.js"></script>

    <script src="/Scripts/base.js"></script>
    <script src="../Scripts/webs/order.js"></script>
    <script src="/Scripts/remind.js"></script>
    <link href="/Content/base.css" rel="stylesheet" />
    <link href="../Content/webs/order.css" rel="stylesheet" />

    <title>用户订单</title>
    <script>

    </script>

    <%--     <style>
        @-moz-document url-prefix() {
            select.form-control {
                -moz-appearance: none;
                appearance: none;
                background-image: url("http://221.228.109.114:8083/manage/more/firefox_select_icon.png");
                background-repeat: no-repeat;
                background-position: calc(100% - 7px) 50%;
                background-size: 2% auto;
                border-radius:3px;
                padding:0;
            }
        }
    </style>--%>
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
                    <%-- <font class="tipsNumber">11</font>--%>
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
                <%if (isAdmin == true)
                  { %>
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
                <li class="li-back li-back-open active">
                    <img src="/Images/base/icon-order.svg" /><a href="Order.aspx">用户订单</a></li>
                <%if (isAdmin == true)
                  { %>
                <li class="li-back li-back-open">
                    <img src="/Images/base/icon-balance.svg" /><a href="Balance.aspx">钱包余额</a></li>
                <%}
                  else
                  { %>
                <li class="li-back li-back-open">
                    <img src="/Images/base/icon-balance.svg" /><a href="BalanceSeller.aspx">钱包余额</a></li>
                <%} %>
                <li class="li-back li-back-open">
                    <img src="/Images/base/icon-recipeSet.svg" /><a href="RecipeSet.aspx">食谱缺货设置</a></li>
                <%if (isAdmin == true)
                  { %>
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
                    <h4>商家后台-用户订单</h4>
                </div>
            </div>
            <div class="row row-min" id="divMain2">

                <div class="row4 fl">
                    <font>下单时间:</font>
                    <div>
                        <input type="text" id="inputDate1" />
                    </div>
                    <span>—</span>
                    <div>
                        <input type="text" id="inputDate2" />
                    </div>
                </div>
                <%if (isAdmin == true)
                  { %>
                <div class="select-rest fl">
                    <font>选择餐厅:</font>
                    <select class="selector-rest">
                        <option value="">全部</option>
                        <%foreach (var k in dictRest.Keys)
                          { %>
                        <option value="<%=k %>"><%=dictRest[k] %></option>
                        <%} %>
                    </select>
                </div>
                <%} %>
                <div class="fc"></div>
            </div>
            <div class="row" id="divMain3">
                <div class="table-wrap">
                <table class="table table-striped table-hover">
                    <thead class="thead1">
                        <tr>
                            <th width="1%"></th>
                            <th width="3%">订单号</th>
                            <th width="3%">商家</th>
                            <th width="3%">食谱</th>
                            <th width="3%">价格</th>
                            <th width="3%">客户</th>
                            <th width="3%">到店时间</th>
                            <th width="3%">支付方式</th>
                            <th width="3%">联系方式</th>
                            <th width="3%">下单时间</th>
                        </tr>
                    </thead>
                    <tbody>

                    </tbody>
                </table>
                </div>
                <nav style="text-align: center; display: block">
                    <ul class="pagination">
                        <li class="disabled"><a href="#" style="text-decoration: none;">«</a></li>
                        <li class="active"><a style="text-decoration: none;">1</a></li>
                        <li onclick="getPage(this)"><a style="text-decoration: none;">2</a></li>
                        <li onclick="getPage(this)"><a style="text-decoration: none;">3</a></li>
                        <li onclick="getPage(this)"><a style="text-decoration: none;">4</a></li>
                        <li onclick="getNextPage()"><a style="text-decoration: none;">»</a></li>
                    </ul>
                </nav>
            </div>
        </div>

    </div>

    <div class="modal fade" id="modal1" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;
                    </button>
                    <img class="btn-edit fl" src="/Images/base/icon-info.svg" /><font class="modal-title fl" id="myModalLabel">查看订单
                    </font>
                    <div class="fc"></div>
                </div>
                <div class="modal-body">
                    <div class="info">
                        <font class="info-font1 bold fl">订单号:</font>
                        <font class="fl info-font2">321321</font>
                        <div class="fc"></div>
                    </div>
                    <div class="info">
                        <font class="info-font1 bold fl">商家:</font>
                        <font class="fl info-font2">321321</font>
                        <div class="fc"></div>
                    </div>
                    <div class="info">
                        <font class="info-font1 bold fl">食谱:</font>
                        <font class="fl info-font2">321321</font>
                        <div class="fc"></div>
                    </div>
                    <div class="info">
                        <font class="info-font1 bold fl">价格:</font>
                        <font class="fl info-font2">321321</font>
                        <div class="fc"></div>
                    </div>
                    <div class="info">
                        <font class="info-font1 bold fl">客户:</font>
                        <font class="fl info-font2">321321</font>
                        <div class="fc"></div>
                    </div>
                    <div class="info">
                        <font class="info-font1 bold fl">到店时间:</font>
                        <font class="fl info-font2">321321</font>
                        <div class="fc"></div>
                    </div>
                    <div class="info">
                        <font class="info-font1 bold fl">支付方式:</font>
                        <font class="fl info-font2">321321</font>
                        <div class="fc"></div>
                    </div>
                    <div class="info">
                        <font class="info-font1 bold fl">联系方式:</font>
                        <font class="fl info-font2">321321</font>
                        <div class="fc"></div>
                    </div>
                    <div class="info">
                        <font class="info-font1 bold fl">下单时间:</font>
                        <font class="fl info-font2">321321</font>
                        <div class="fc"></div>
                    </div>

                </div>
                <input type="hidden" />

            </div>
        </div>
    </div>



</body>
</html>
