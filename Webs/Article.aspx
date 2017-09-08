<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Article.aspx.cs" Inherits="WebApplication1.Webs.Article" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>文章管理</title>
    <script type="text/javascript">
       
    </script>
    <%--    <script src="../Scripts/jquery-2.0.0.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>--%>
    <link href="../Content/bootstrap.css" rel="stylesheet" />
    <link href="../Scripts/dist/css/wangEditor.min.css" rel="stylesheet" />


    <link href="/Content/base.css" rel="stylesheet" />
    <link href="/Content/webs/article.css" rel="stylesheet" />
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
                <li class="active">
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
                    <h4>文章管理</h4>
                </div>

            </div>
            <div class="row row-search-rest" id="divMain2">
                <%--改这里 --%>
                <div class="row row-search-rest">
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
                        <a class="btn btn-this batch-delete">
                            <img src="/Images/recipe/icon-deleteSome.svg" /><font>批量删除</font></a>
                        <a class="btn btn-this btn-add">
                            <img src="/Images/recipe/icon-add.png" /><font>增加</font></a>
                    </div>
                </div>
            </div>
            <div class="row" id="divMain3">
                <div class="table-wrap">
                <table class="table table-striped table-hover">
                    <thead>
                        <tr>
                            <th width="2%">
                                <input type="checkbox"/></th>
                            <th width="2%">标题</th>
                            <th width="5%">标签</th>
                            <th width="5%">发表时间</th>
                            <th width="5%">浏览量</th>
                            <th width="20%">点赞数</th>
                            <th width="5%">主要内容</th>
                            <th width="12%">操作</th>
                        </tr>
                    </thead>
                    <tbody>
                     
                    </tbody>
                </table>
                    </div>
            </div>

        </div>
    </div>


    <%--编辑模态框--%>
    <div class="modal fade" id="modal1" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">

        <div class="modal-dialog" style="height: 1000px">
            <div class="modal-content">
                <%--   <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true" onclick="reload()">&times;</button>
                    <h4 class="modal-title" id="myModalLabel">文章管理</h4>
                </div>--%>
                <div class="modal-body">
                    <div class="control-group">
                        <div class="row">
                            <font>标题:</font>
                            <input id="divTitle" name="name"  maxlength="18"/><font class="inputTips">还剩18字可以输入</font>
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        </div>

                        <div id="divTags" class="row">
                            <font class="font-tag">标签:</font>
                            <span class="label label-select">
                                <font>红烧</font>
                                <button type="button" class="close">&times;</button>

                            </span>
                            <span class="label label-select">
                                <font>清蒸</font>
                                <button type="button" class="close">&times;</button>
                            </span>
                            <span class="label label-select">
                                <font>乱炖</font>
                                <button type="button" class="close">&times;</button>
                            </span>
                            <div class="add-selector">
                                <img src="/Images/base/icon-add.png" class="icon-add"/>
                                <div class="tag-selector">
                                    <div class="row selector-head">
                                        <img class="btn-edit" src="/Images/recipe/icon-edit.svg" alt="Alternate Text" />
                                        <font>添加标签</font>
                                         <button type="button" class="close" >&times;</button>
                                    </div>
                                    <hr class="row" />
                                    <div class="row selector-body">
                                        <%foreach (var i in dictTags.Values)
                                          { %>
                                        <span class="label-default"><%=i %></span>
                                       
                                        <%} %>
                                    </div>
                                    <hr class="row" />
                                    <div class="row selector-foot">
                                        <button type="button" class="btn btn-tag">
                                            确定
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" id="divEditor" style="height: 600px">
                        </div>


                        <div class="modal-footer">
                            <input type="hidden" id="url" />
                            <input type="hidden" id="inputId" />
                            <a id="btn-addArticle" class="btn btn-success">完成</a>
                        </div>
                        <div class="row" style="visibility: hidden">
                        </div>
                    </div>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal -->
    </div>
    <%--编辑模态框结束--%>
    <input type="hidden" id="originalImg" />


    <script src="../Scripts/jquery-2.0.0.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>

    <script src="../Scripts/datetimepicker/bootstrap-datetimepicker.js"></script>
    <link href="../Scripts/datetimepicker/bootstrap-datetimepicker.min.css" rel="stylesheet" />

    <link href="../Scripts/checkboxStyle/skins/minimal/minimal.css" rel="stylesheet" />
    <script src="../Scripts/checkboxStyle/icheck.js"></script>

    <script src="../Scripts/dist/js/wangEditor.min.js"></script>
    <script src="/Scripts/base.js"></script>
    <script src="/Scripts/webs/article.js"></script>
    <script type="text/javascript">
     
       
    </script>
</body>
</html>

