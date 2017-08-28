<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Seller.aspx.cs" Inherits="WebApplication1.Webs.accountSet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>商家账号设置</title>
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
    <script src="/Scripts/webs/seller.js?ver=<%=ran %>"></script>
    <link href="/Content/base.css" rel="stylesheet" />
    <link href="/Content/webs/seller.css" rel="stylesheet" />

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
                <li class="active">
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
                    <h4>商家后台-商家账号设置</h4>
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
                    <div class="row1-right fr">
                        <a class="btn btn-this batchDelete">
                            <img src="/Images/recipe/icon-deleteSome.svg" /><font>批量删除</font></a>
                        <a class="btn btn-this btn-add">
                            <img src="/Images/recipe/icon-add.png" /><font>增加</font></a>
                    </div>
                </div>
            </div>
            <div class="row" id="divMain3">
                <table class="table table-striped table-hover">
                    <thead class="thead1">
                        <tr>
                            <th width="1%">
                                <input type="checkbox"></th>
                            <th width="1%">#</th>
                            <th width="3%">商家</th>
                            <th width="3%">账号</th>
                            <th width="3%">密码</th>
                            <th width="3%"></th>
                            <th width="3%"></th>
                            <th width="3%"></th>
                            <th width="4%">操作</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr style="background-color: white;">
                            <td>
                                <input type="checkbox">
                            </td>
                            <td>1
                            </td>
                            <td>江浙1
                            </td>
                            <td>shangjia1
                            </td>
                            <td>12123123
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td class="edit">
                                <a class="btn-edit-left" style="text-decoration: none;">
                                    <img class="btn-edit" src="/Images/recipe/icon-edit.svg"><font>编辑</font></a>
                                <a style="text-decoration: none;" class="btn-delete">
                                    <img class="btn-edit" src="/Images/recipe/icon-delete.svg"><font>删除</font>
                                </a>
                                <input type="hidden" value="">
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <nav style="text-align: center; display: block"><ul class="pagination"><li class="disabled"><a style="text-decoration: none;">«</a></li><li class="active"><a style="text-decoration: none;">1</a></li><li onclick="getPage(this)"><a style="text-decoration: none;">2</a></li><li onclick="getNextPage()"><a style="text-decoration: none;">»</a></li></ul></nav>
        </div>
        <div style="clear: both"></div>
    </div>

    <%--模态框--%>
    <div class="modal fade" id="modal1" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;
                    </button>
                    <img class="btn-edit" src="/Images/recipe/icon-edit.png" /><font class="modal-title" id="myModalLabel">菜品编辑
                    </font>
                </div>
                <div class="modal-body">
                    <div class="info">
                        <font>商家:</font>
                        <select>
                            <option value="value">text</option>
                        </select>
                    </div>
                    <div class="info">
                        <font>账号:</font>
                        <input id="info-account" type="text" onkeyup="value=value.replace(/[^\w\.\/]/ig,'')" maxlength="16"/>
                    </div>
                    <div class="info">
                        <font>密码:</font>
                        <input id="info-password" type="text" onkeyup="value=value.replace(/[^\w\.\/]/ig,'')" maxlength="16"/>
                    </div>

                </div>
                <div class="footer">
                    <button type="button" class="btn btn-base">
                        确定
                    </button>
                    <input type="hidden" id="sellerId" />
                </div>

            </div>

        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal -->
</body>
</html>
