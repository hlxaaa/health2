﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuestionPro.aspx.cs" Inherits="WebApplication1.Webs.QuestionPro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>问卷管理-专业版</title>
    <script type="text/javascript">

    </script>
    <script src="../Scripts/jquery-2.0.0.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <link href="../Content/bootstrap.css" rel="stylesheet" />

    <script src="../Scripts/datetimepicker/bootstrap-datetimepicker.js"></script>
    <link href="../Scripts/datetimepicker/bootstrap-datetimepicker.min.css" rel="stylesheet" />

    <link href="../Scripts/checkboxStyle/skins/minimal/minimal.css" rel="stylesheet" />
    <script src="../Scripts/checkboxStyle/icheck.js"></script>

    <script src="/Scripts/base.js"></script>
    <script src="/Scripts/webs/questionPro.js"></script>
    <link href="/Content/base.css" rel="stylesheet" />
    <link href="/Content/webs/questionPro.css" rel="stylesheet" />
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
                    <img src="/Images/base/icon-triangle-down.svg" /><a>问卷管理</a>
                </li>
                <li class="li-ques li-ques-open ">
                    <img src="/Images/base/icon-easyQues.svg" /><a href="QuestionEasy.aspx">简易版</a></li>
                <li class="li-ques li-ques-open active">
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
                <li class="">
                    <img src="/Images/base/icon-withdraw-white.svg" style="" /><a href="Withdraw.aspx">提现申请</a>
                </li>

            </ul>

        </div>
        <div class="" id="divMains">
            <div class="row" id="divMain1">
                <div id="main1-left">
                    <h4>问卷管理-专业版</h4>
                </div>
                <div id="main1-right">
                    <a class="btn btn-this" id="batch-delete">
                        <img src="/Images/recipe/icon-deleteSome.svg" /><font>批量删除</font></a>
                    <a onclick="addTemplate()" class="btn btn-this">
                        <img src="/Images/recipe/icon-add.png" /><font>增加</font>
                    </a>
                </div>
            </div>
            <div class="main2">
                <div class="table-wrap">
                    <table class="table table-striped table-hover">
                        <thead class="thead1">
                            <tr>
                                <th width="1%">
                                    <input type="checkbox"></th>
                                <th width="5%">题目</th>
                                <th width="15%">选项</th>
                                <th width="7%">体质</th>
                                <th width="7%">条件</th>
                                <th width="9%">操作</th>
                            </tr>
                        </thead>
                        <tbody>
                            <%for (var i = id.Length - 1; i > -1; i--)
                              { %>
                            <tr style="background-color: white;">
                                <td class="ques-select">
                                    <input type="hidden" value="<%=id[i] %>" />
                                    <input type="checkbox">
                                </td>
                                <td>
                                    <div class="orderNo"><%=i+1 %>. </div>
                                    <input class="ques-title" value="<%=content[i] %>" />
                                    <div class="vertical-line1"></div>
                                </td>
                                <td class="ques-options">
                                    <input value="完全不" readonly="readonly" />
                                    <input value="有一点" readonly="readonly" />
                                    <input value="非常" readonly="readonly" />
                                    <div class="vertical-line2"></div>
                                </td>
                                <td class="ques-constitution">
                                    <select>
                                        <%for (int k = 0; k < constitutions.Length; k++)
                                          {
                                              if (constitution[i] == constitutions[k])
                                              {
                                        %>
                                        <option selected="selected"><%=constitution[i] %></option>
                                        <%}
                                      else
                                      { %>
                                        <option><%=constitutions[k] %></option>
                                        <%}
                                  } %>
                                    </select>
                                    <div class="vertical-line3"></div>
                                </td>
                                <td class="ques-sex">
                                    <select>
                                        <%for (int k = 0; k < sexs.Length; k++)
                                          {
                                              if (sex[i] == sexs[k])
                                              {
                                        %>
                                        <option selected="selected"><%=sex[i] %></option>
                                        <%}
                                      else
                                      { %>
                                        <option><%=sexs[k] %></option>
                                        <%}
                                  } %>
                                    </select>
                                    <div class="vertical-line4"></div>
                                </td>
                                <td class="ques-delete">
                                    <a>
                                        <img class="btn-edit" src="/Images/recipe/icon-delete.svg" /><font>删除</font></a>
                                </td>
                            </tr>
                            <%} %>
                        </tbody>
                    </table>
                </div>

            </div>
            <div class="main3">
                <a class="btn btn-update" id="btn-update">保存修改</a>
            </div>
        </div>
    </div>
</body>
</html>

