<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecipeContent.aspx.cs" Inherits="WebApplication1.Webs.RecipeContent" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>食谱管理内页</title>
    <script type="text/javascript">
        var available = '<%=available%>'
        var id = '<%=id%>'
    </script>
    <script src="../Scripts/jquery-2.0.0.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <link href="../Content/bootstrap.css" rel="stylesheet" />

    <script src="../Scripts/datetimepicker/bootstrap-datetimepicker.js"></script>
    <link href="../Scripts/datetimepicker/bootstrap-datetimepicker.min.css" rel="stylesheet" />

    <link href="../Scripts/checkboxStyle/skins/minimal/minimal.css" rel="stylesheet" />
    <script src="../Scripts/checkboxStyle/icheck.js"></script>

    <script src="/Scripts/base.js"></script>
    <script src="/Scripts/webs/recipeContent.js"></script>
    <link href="/Content/base.css" rel="stylesheet" />
    <link href="/Content/webs/recipeContent.css" rel="stylesheet" />
    <script src="../Scripts/jquery-form.js"></script>

    <script src="../Scripts/Jcrop/jquery.Jcrop.js"></script>
    <script type="text/javascript">
        function imgCut() {
            jQuery(function ($) {

                var jcrop_api;

                $('#target').Jcrop({
                    allowSelect: true,
                    onChange: showCoords,
                    onSelect: showCoords,
                    onRelease: clearCoords,
                    boxWidth: 700
                }, function () {
                    jcrop_api = this;
                });

                $('#coords').on('change', 'input', function (e) {
                    var x1 = $('#x1').val(),
                        x2 = $('#x2').val(),
                        y1 = $('#y1').val(),
                        y2 = $('#y2').val();
                    jcrop_api.setSelect([x1, y1, x2, y2]);
                });

            });

            // Simple event handler, called from onChange and onSelect
            // event handlers, as per the Jcrop invocation above
            function showCoords(c) {
                $('#x1').val(c.x);
                $('#y1').val(c.y);
                $('#x2').val(c.x2);
                $('#y2').val(c.y2);
                $('#w').val(c.w);
                $('#h').val(c.h);
            };

            function clearCoords() {
                $('#coords input').val('');
            };
        }
    </script>
    <%--<link href="../Scripts/Jcrop/main.css" rel="stylesheet" />--%>
    <link href="../Scripts/Jcrop/jquery.Jcrop.css" rel="stylesheet" />
    <link href="../Scripts/Jcrop/demos.css" rel="stylesheet" />
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
            <div class="main-header">
                <input type="hidden" id="recipeId" />
                <div class="header-content">
                    <h4>食谱管理</h4>
                </div>
            </div>
            <div class="main-body">
                <div class="mainbody-title">
                    <div class="fl bold">
                        名称:
                    </div>
                    <div class="fl">
                        <input type="text" id="title" value="<%=name %>" maxlength="25" />
                    </div>
                    <div class="fr btn-del">
                        <a style="text-decoration: none;">
                            <img class="btn-edit2" src="/Images/recipe/icon-delete.svg"><font class="bold font-base">删除</font></a>
                    </div>
                    <div class="fc"></div>
                </div>
                <div class="mainbody-info1">
                    <div class="info1-available">
                        <div class="fl">是否有货:</div>
                        <div class="fl">
                            <div class="fl">
                                有<input type="radio" name="available" />
                            </div>
                            <div class="fl right-radio">
                                没有<input type="radio" name="available" value=" " />
                            </div>

                        </div>
                        <div class="fc"></div>
                    </div>

                    <div class="info1-center">
                        <div class="center-rest">
                            <div class="fl">
                                餐厅:
                            </div>
                            <div class="fl">
                                <select id="restName">
                                    <%foreach (var k in dictRest.Keys)
                                      {
                                          if (dictRest[k] == rest)
                                          { %>
                                    <option value="<%=k %>" selected="selected"><%=dictRest[k] %></option>
                                    <%}
                                          else
                                          { %>
                                    <option value="<%=k %>"><%=dictRest[k] %></option>
                                    <%}
                                      }%>
                                </select>
                            </div>
                        </div>
                        <div class="center-price">
                            <div class="fl">
                                价格:
                            </div>
                            <div class="fl">
                                <input type="text" id="price" value="<%=price %>" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" maxlength="8" />
                            </div>
                        </div>
                        <div class="center-sales">
                            <div class="fl">
                                销售量:
                            </div>
                            <div class="fl">
                                <input type="text" id="sales" value="<%=sales %>" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" maxlength="8" />
                            </div>
                        </div>
                        <div class="fc"></div>
                    </div>
                    <div class="info1-tags">
                        <div class="fl">
                            标签:
                        </div>
                        <div>
                            <%foreach (var i in tags)
                              { %>

                            <span class="fl tags-span">
                                <div>
                                    <font class="fl"><%=i %></font>
                                    <button type="button" class="close fl">×</button>
                                </div>
                            </span>


                            <%--  <span class="label-danger fl tags-span">清蒸
                                <button type="button" class="close">×</button>
                            </span>--%>
                            <%} %>
                            <img src="/Images/base/icon-add.png" class="icon-add fl btn-tag-selector">
                        </div>
                        <div class="fc"></div>
                    </div>
                </div>
                <div class="mainbody-info2">
                    <table>
                        <thead class="thead1">
                            <tr>
                                <th width="5%">
                                    <img src="/Images/base/icon-content.svg" class="img-base"></th>
                                <th width="8%">分类</th>
                                <th width="20%">菜品</th>
                                <th width="7%">规格</th>
                                <th width="1%">
                                    <img src="/Images/base/icon-add.png" class="icon-add fl img-base btn-addfood">
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <%if (foods.Length > 0)
                              {
                                  for (var i = 0; i < foods.Length; i++)
                                  { %>
                            <tr style="background-color: white;">
                                <td></td>
                                <td>
                                    <select class="foodtype">
                                        <%foreach (var v in dictType.Keys)
                                          {
                                              if (dictType[v] == foodtypes[i])
                                              {
                                        %>
                                        <option value="<%=v %>" selected="selected"><%=dictType[v] %></option>
                                        <%}
                                              else
                                              { %>
                                        <option value="<%=v %>"><%=dictType[v] %></option>
                                        <%}
                                          }%>
                                    </select>
                                </td>
                                <td>
                                    <select class="food">
                                        <%foreach (var v in dictFood.Keys)
                                          {
                                              if (dictFood[v] == foods[i])
                                              {
                                        %>
                                        <option value="<%=v %>" selected="selected"><%=dictFood[v] %></option>
                                        <%}
                                               else
                                               { %>
                                        <option value="<%=v %>"><%=dictFood[v] %></option>
                                        <%}
                                          }%>
                                    </select>
                                </td>
                                <td>
                                    <input type="text" class="weight" value="<%=weights[i] %>" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" maxlength="8" />g
                                </td>
                                <td>
                                    <img src="/Images/base/icon-content-del.svg" class="img-base btn-del-food" />
                                </td>
                            </tr>
                            <%}
                              }
                              else
                              {%>
                            <tr style="background-color: white;">
                                <td></td>
                                <td>
                                    <select class="foodtype">
                                        <%foreach (var v in dictType.Keys)
                                          { 
                                            
                                        %>

                                        <option value="<%=v %>"><%=dictType[v] %></option>
                                        <%} %>
                                    </select>
                                </td>
                                <td>
                                    <select class="food">
                                        <%foreach (var v in dictFood.Keys)
                                          {
                                            
                                        %>

                                        <option value="<%=v %>"><%=dictFood[v] %></option>
                                        <% }%>
                                    </select>
                                </td>
                                <td>
                                    <input type="text" class="weight" value="" />g
                                </td>
                                <td>
                                    <img src="/Images/base/icon-content-del.svg" class="img-base btn-del-food" />
                                </td>
                            </tr>
                            <%} %>
                        </tbody>
                    </table>
                </div>
                <div class="mainbody-imgs">
                    <div class="font-tips">
                        注意：图片建议尺寸至少为700*900，以达到较好的展示效果！
                    </div>
                    <div>
                        <%for (var i = 0; i < imgs.Length; i++)
                          { %>
                        <div class="div-imgs fl">
                            <img src="/Images/recipe/icon-deleteSome.svg" class="img-del" />
                            <img class="img-show" src="/<%=imgs[i] %>" />

                            <div class="thumb">
                                <%if (i == 0)
                                  { %>
                                <input type="radio" name="name" value=" " checked="checked" /><font>封面图</font>
                                <%}
                                  else
                                  { %>
                                <input type="radio" name="name" value=" " /><font>封面图</font>
                                <%} %>
                            </div>
                        </div>
                        <%} %>

                        <div class="div-upImg fl">
                            <img src="/Images/base/icon-img.svg" alt="上传图片按钮" />
                            <font>上传图片</font>
                        </div>
                        <form id="formid" method='post' enctype="multipart/form-data" action='RecipeContent.aspx' style="visibility: hidden">
                            <input type="file" id="upImg" runat="server" onchange="filechange(event)" multiple="multiple" name="img" />
                            <input id="testSubmit" type="submit" value="上传" />
                        </form>
                        <div class="fc"></div>
                    </div>
                </div>
            </div>
        </div>
        <div class="main-btns">
            <a class="btn-edit fl btn-save">保存修改</a>
            <a class="btn-edit fl btn-back">取消返回</a>

        </div>

    </div>
    <div class="fc"></div>
    <input type="hidden" id="oImg" value="<%=oImgs %>" />

    <%--模态框--%>
    <div class="modal fade" id="modal1" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;
                    </button>
                    <img class="img-base" src="/Images/recipe/icon-edit.png" /><font class="font-base bold" id="myModalLabel">标签编辑
                    </font>
                </div>
                <div class="modal-body">
                    <div class="tags">
                        <%foreach (var v in dictTag.Values)
                          { %>
                        <span class="tag fl label-default"><%=v %></span>
                        <%} %>
                        <div class="fc"></div>
                    </div>
                </div>
                <div class="footer">
                    <button type="button" class="btn btn-base btn-saveTag">
                        确定
                    </button>
                </div>
            </div>
        </div>
    </div>

    <%--模态框--%>
    <div class="modal fade" id="imgCut" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog dialog-cut">
            <div class="modal-content content-cut">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;
                    </button>

                    </font>
                </div>
                <div class="modal-body modal-body-cut">
                    <img src="" id="target" style="width: 700px" />
                    <form id="coords"
                        class="coords">
                        <div class="inline-labels">
                            <label>X1
                                <input type="text" size="4" id="x1" name="x1" /></label>
                            <label>Y1
                                <input type="text" size="4" id="y1" name="y1" /></label>
                            <label>X2
                                <input type="text" size="4" id="x2" name="x2" /></label>
                            <label>Y2
                                <input type="text" size="4" id="y2" name="y2" /></label>
                            <label>W
                                <input type="text" size="4" id="w" name="w" /></label>
                            <label>H
                                <input type="text" size="4" id="h" name="h" /></label>
                        </div>
                    </form>
                </div>
                <div class="footer footer-cut">
                    <button type="button" class="btn btn-cutImg btn-base">
                        确定
                    </button>
                </div>
            </div>
        </div>
    </div>

</body>
</html>

