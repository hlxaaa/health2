<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tag.aspx.cs" Inherits="WebApplication1.Webs.Tag" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>标签管理</title>
    <script type="text/javascript">
       
    </script>
    <script src="../Scripts/jquery-2.0.0.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <link href="../Content/bootstrap.css" rel="stylesheet" />

    <script src="../Scripts/datetimepicker/bootstrap-datetimepicker.js"></script>
    <link href="../Scripts/datetimepicker/bootstrap-datetimepicker.min.css" rel="stylesheet" />

    <link href="../Scripts/checkboxStyle/skins/minimal/minimal.css" rel="stylesheet" />
    <script src="../Scripts/checkboxStyle/icheck.js"></script>

    <script src="/Scripts/base.js?ver=<%=ran %>"></script>
    <script src="/Scripts/webs/tag.js?ver=<%=ran %>"></script>
    <link href="/Content/base.css" rel="stylesheet" />
    <link href="/Content/webs/tag.css" rel="stylesheet" />
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
                <li class="active">
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
                <li class="">
                    <img src="/Images/base/icon-withdraw-white.svg" style="" /><a href="Withdraw.aspx">提现申请</a>
                </li>

            </ul>

        </div>
        <div class="" id="divMains">
            <div class="row" id="divMain1">
                <div id="main1-left">
                    <h4>标签管理</h4>
                </div>
                <div id="main1-right">
                    <a class="btn btn-this btn-batchDelete">
                        <img src="/Images/recipe/icon-deleteSome.svg" /><font>批量删除</font></a>
                    <a class="btn btn-this btn-addTemplate">
                        <img src="/Images/recipe/icon-add.png" /><font>增加</font></a>
                </div>
            </div>

            <div class="row" id="divMain3">
                <div class="table-wrap">
                    <table class="table table-striped table-hover">
                        <thead class="thead1">
                            <tr>
                                <th class="th-first" width="1%">
                                    <input type="checkbox"></th>
                                <th width="2%">标签</th>
                                <th width="2%">平和质</th>
                                <th width="2%">气郁质</th>
                                <th width="2%">阴虚质</th>
                                <th width="2%">痰湿质</th>
                                <th width="2%">阳虚质</th>
                                <th width="2%">特禀质</th>
                                <th width="2%">湿热质</th>
                                <th width="2%">气虚质</th>
                                <th width="2%">血瘀质</th>
                                <th width="5%">操作</th>
                            </tr>
                        </thead>
                        <tbody>
                            <%if (jo["Table"] != null)
                              {
                                  for (var i = 0; i < jo["Table"].Count(); i++)
                                  { %>
                            <tr style="background-color: white;">
                                <td class="ques-select">
                                    <input type="hidden" value="<%=jo["Table"][i]["id"]%>" />
                                    <input type="checkbox">
                                </td>
                                <td>
                                    <%=jo["Table"][i]["name"]%>
                                </td>
                                <td>
                                    <%=jo["Table"][i]["pinghescore"]%>
                                </td>
                                <td>
                                    <%=jo["Table"][i]["qiyuscore"]%>
                                </td>
                                <td>
                                    <%=jo["Table"][i]["yinxuscore"]%>
                                </td>
                                <td>
                                    <%=jo["Table"][i]["tanshiscore"]%>
                                </td>
                                <td>
                                    <%=jo["Table"][i]["yangxuscore"]%>
                                </td>
                                <td>
                                    <%=jo["Table"][i]["tebingscore"]%>
                                </td>
                                <td>
                                    <%=jo["Table"][i]["shirescore"]%>
                                </td>
                                <td>
                                    <%=jo["Table"][i]["qixuscore"]%>
                                </td>
                                <td>
                                    <%=jo["Table"][i]["xueyuscore"]%>
                                </td>
                                <td class="td-edit">
                                    <a id="aRightBorder" style="text-decoration: none;">
                                        <img class="btn-edit" src="/Images/recipe/icon-edit.svg"><font>编辑</font></a>
                                    <a style="text-decoration: none;" class="btn-delete">
                                        <img class="btn-edit" src="/Images/recipe/icon-delete.svg"><font>删除</font>
                                    </a>
                                    <input type="hidden" value="<%=jo["Table"][i]["id"]%>" />
                                </td>
                            </tr>
                            <%}
                              } %>
                        </tbody>
                </div>
            </div>
        </div>
    </div>

    <!-- 模态框（Modal） -->
    <div class="modal fade" id="modal1" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;
                    </button>
                    <img class="btn-edit" src="/Images/recipe/icon-edit.png" /><h4 class="modal-title" id="myModalLabel">标签编辑
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="tag-name">
                        <font>名称:</font>
                        <input type="type" name="name" value="" />
                    </div>
                    <div>
                        <font>平和质:</font>
                        <select>
                            <option value="0">0</option>
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
                        </select>
                    </div>
                    <div>
                        <font>气郁质:</font>
                        <select>
                            <option value="0">0</option>
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
                        </select>
                    </div>
                    <div>
                        <font>阴虚质:</font>
                        <select>
                            <option value="0">0</option>
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
                        </select>
                    </div>
                    <div>
                        <font>痰湿质:</font>
                        <select>
                            <option value="0">0</option>
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
                        </select>
                    </div>
                    <div>
                        <font>阳虚质:</font>
                        <select>
                            <option value="0">0</option>
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
                        </select>
                    </div>
                    <div>
                        <font>特禀质:</font>
                        <select>
                            <option value="0">0</option>
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
                        </select>
                    </div>
                    <div>
                        <font>湿热质:</font>
                        <select>
                            <option value="0">0</option>
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
                        </select>
                    </div>
                    <div>
                        <font>气虚质:</font>
                        <select>
                            <option value="0">0</option>
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
                        </select>
                    </div>
                    <div>
                        <font>血瘀质:</font>
                        <select>
                            <option value="0">0</option>
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
                        </select>
                    </div>
                </div>
                <input type="hidden" id="tagId" />
                <div class="footer">
                    <button type="button" class="btn btn-primary">
                        确定
                    </button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal -->
    </div>

</body>
</html>

