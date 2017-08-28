<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BalanceSeller.aspx.cs" Inherits="WebApplication1.Webs.BalanceSeller" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>钱包余额</title>
    <script type="text/javascript">
        var jsonDetail = '<%=jsonDetail%>'
        var jsonWithdraw = '<%=jsonWithdraw%>'
        var sellerId = '<%=id%>'
    </script>
    <script src="../Scripts/jquery-2.0.0.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <link href="../Content/bootstrap.css" rel="stylesheet" />

    <script src="../Scripts/datetimepicker/bootstrap-datetimepicker.js"></script>
    <link href="../Scripts/datetimepicker/bootstrap-datetimepicker.min.css" rel="stylesheet" />

    <link href="../Scripts/checkboxStyle/skins/minimal/minimal.css" rel="stylesheet" />
    <script src="../Scripts/checkboxStyle/icheck.js"></script>

    <script src="/Scripts/base.js?ver=<%=ran %>"></script>
    <script src="/Scripts/webs/balanceSeller.js?ver=<%=ran %>"></script>
    <link href="/Content/base.css" rel="stylesheet" />
    <link href="/Content/webs/balanceSeller.css" rel="stylesheet" />
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
                 <li class="li-back li-back-open active">
                    <img src="/Images/base/icon-balance.svg" /><a href="Balance.aspx">钱包余额</a></li>
                <%}else{ %>
                 <li class="li-back li-back-open active">
                    <img src="/Images/base/icon-balance.svg" /><a href="BalanceSeller.aspx?sellerId=<%=id %>">钱包余额</a></li>
                <%} %>
                 <li class="li-back li-back-open">
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
                    <h4>商家后台-钱包余额</h4>
                </div>
            </div>
            <div class="row" id="divMain2">
                <div class="main2-div1 fl">
                    <img src="/Images/base/vertical-line.svg" class="v-line fl" /><font class="bold">余额:</font>
                </div>
                <div class="main2-balance fl">
                    <font><%=balance %></font>
                </div>
                <div class="main2-btn fl">
                    <a class="btn btn-withdraw">提现</a>
                </div>
                <div class="fc"></div>
            </div>
            <div class="row" id="divMain3">
                <div class="cash-detail fl">
                    <div class="detail-title">
                        <img src="/Images/base/icon-income.svg" alt="Alternate Text" class="icon-title fl" /><font class="fl bold">收入明细</font>
                        <div class="fc"></div>
                    </div>
                    <div class="table-wrap">
 <table class="table table-striped table-hover">
                        <thead class="thead1">
                            <tr>
                                <th width="3%">日期</th>
                                <th width="3%">金额</th>
                                <th width="3%">订单</th>
                                <th width="3%">支付方</th>
                            </tr>
                        </thead>
                        <tbody>
                      <%--      <tr style="background-color: white;">
                                <td>
                                    2016-10-01 21:02:59
                                </td>
                                <td class="money-color" >
                                    +500.00
                                </td>
                                <td>
                                   套餐1
                                </td>
                                <td>
                                  老王
                                </td>
                            </tr>--%>
                      
                        </tbody>
                        </table>
                    </div>
                   <%-- <nav style="text-align:center;display:block">
                        <ul class="pagination"><li class="disabled"><a href="#" style="text-decoration: none;">«</a></li><li class="active"><a style="text-decoration: none;">1</a></li><li onclick="getPage(this)"><a style="text-decoration: none;">2</a></li><li onclick="getPage(this)"><a style="text-decoration: none;">3</a></li><li onclick="getNextPage()"><a style="text-decoration: none;">»</a></li></ul>
                    </nav>--%>
                </div>
                <div class="cash-withdraw fl">
                     <div class="detail-title">
                         <img src="/Images/base/icon-withdraw.svg" alt="Alternate Text" class="icon-title fl"/><font class="fl bold">提现申请</font>
                         <div class="fc"></div>
                    </div>
                    <div class="table-wrap">
                         <table class="table table-striped table-hover">
                        <thead class="thead1">
                            <tr>
                                <th width="3%">日期</th>
                                <th width="3%">金额</th>
                                <th width="3%">状态</th>
                            </tr>
                        </thead>
                        <tbody>
                       <%--     <tr style="background-color: white;">
                                <td>
                                    2016-10-01 21:02:59
                                </td>
                                <td class="money-color" >
                                    +500.00
                                </td>
                                <td>
                                   体现成功
                                </td>
                            </tr>--%>
                            
                        </tbody>
                        </table>
                    </div>
                  <%--  <nav style="text-align:center;display:block"><ul class="pagination"><li class="disabled"><a href="#" style="text-decoration: none;">«</a></li><li class="active"><a style="text-decoration: none;">1</a></li><li onclick="getPage(this)"><a style="text-decoration: none;">2</a></li><li onclick="getPage(this)"><a style="text-decoration: none;">3</a></li><li onclick="getNextPage()"><a style="text-decoration: none;">»</a></li></ul></nav>--%>
                </div>
                <div class="fc"></div>
            </div>

        </div>
    </div>

      <%--模态框--%>
    <div class="modal fade" id="modal1" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
              <%--  <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;
                    </button>
                    <img class="btn-edit" src="/Images/recipe/icon-edit.png" /><h4 class="modal-title" id="myModalLabel">菜品编辑
                    </h4>
                </div>--%>
                <div class="modal-body">
                 <font>提现金额:</font>
                    <input type="text" id="number" placeholder="还可提取<%=availableCash %>元" onkeyup="this.value=this.value.replace(/[^\d.]/g,'')" onafterpaste="this.value=this.value.replace(/[^\d.]/g,'')"/>
                     <button type="button" class="btn btn-save">
                        确定
                    </button>
                </div>
         

            </div>

        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal -->

</body>
</html>
