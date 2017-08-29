<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Customer.aspx.cs" Inherits="WebApplication1.Webs.Customer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>会员管理</title>
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
    <script src="/Scripts/webs/customer.js?ver=<%=ran %>"></script>
    <link href="/Content/base.css" rel="stylesheet" />
    <link href="/Content/webs/customer.css" rel="stylesheet" />
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
                <li class="active">
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
                    <h4>会员管理</h4>
                </div>
                <div id="main1-right">
                    <font class="fl">用户名:</font>
                    <div  class="fl">
                        <input type="text" value="" class="input-search fl"/>
                        <div class="btn-search fl">
                            <img class="img-search" src="/Images/recipe/icon-search.svg">
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" id="divMain3">
                <div class="table-wrap">
                    <table class="table table-striped table-hover">
                        <thead class="thead1">
                            <tr>
                                <th width="2%">编号</th>
                                <th width="3%">用户名</th>
                                <th width="3%">电话</th>
                                <th width="2%">微信号</th>
                                <th width="2%">身高</th>
                                <th width="2%">体重</th>
                                <th width="2%">性别</th>
                                <th width="2%">年龄</th>
                                <th width="2%">劳动强度</th>
                                <th width="2%">体质</th>
                                <th width="2%">健康分数</th>
                                <th width="2%">运动统计</th>
                                <th width="4%">操作</th>
                            </tr>
                        </thead>
                        <tbody>
                           
                            <tr style="background-color: white;">
                                <td >
                                    1
                                </td>
                                <td>
                                   老王
                                </td>
                                <td>
                                  18857120152
                                </td>
                                <td>
                                    tongxiaoyi
                                </td>
                                <td>
                                    32
                                </td>
                                <td>
                                        32
                                </td>
                                <td>
                                 男
                                </td>
                                <td>
                                   18
                                </td>
                                <td>
                                    强度
                                </td>
                                <td>
                                平和质
                                </td>
                                <td>
                                  100
                                </td>
                                <td class="td-sport">
                                    <img class="icon-sport" src="/Images/base/icon-sport.png" alt="运动图片" />
                                </td>
                                <td class="td-edit">
                                    <a style="text-decoration: none;">
                                        <img class="btn-edit" src="/Images/recipe/icon-edit.svg"><font>编辑</font></a>
                                    <input type="hidden" value="" />
                                </td>
                            </tr>
                        </tbody>
                        </table>
                    <nav style="text-align:center;display:block"><ul class="pagination"><li class="disabled"><a href="#" style="text-decoration: none;">«</a></li><li class="active"><a href="#" style="text-decoration: none;">1</a></li><li onclick="getPage(this)"><a style="text-decoration: none;">2</a></li><li onclick="getNextPage()"><a style="text-decoration: none;">»</a></li></ul></nav>
                </div>
            </div>
        </div>
    </div>

     <div class="modal fade" id="modal1" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bold">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;
                    </button>
                    <img class="icon-edit fl" src="/Images/recipe/icon-edit.png" /><font class="modal-title fl" id="myModalLabel">用户资料编辑
                        <div class="fc"></div>
                    </font>
                </div>
                <div class="modal-body">
                    <div class="info">
	<font class="fl">编号:</font>
	<font class="fl font-icon">#</font><font id="info-id" class="fl">1</font>
	<div class="fc"></div>
</div>
<div class="info">
	<font class="fl">用户名:</font>
	<input type="text" id="info-name" class="fr"/>
	<div class="fc"></div>
</div>
<div class="info">
	<font class="fl">电话:</font>
	<input type="text" id="info-phone" class="fr" maxlength="11"/>
	<div class="fc"></div>
</div>
<div class="info">
	<font class="fl">微信号:</font>
	<input type="text" id="info-wechat" class="fr"/>
	<div class="fc"></div>
</div>
<div class="info">
	<font class="fl">密码:</font>
	<input type="text" id="info-password" class="fr"/>
	<div class="fc"></div>
</div>
<div class="info">
	<font class="fl">身高(cm):</font>
	<input type="text" id="info-height" class="fr" maxlength="3"/>
	<div class="fc"></div>
</div>
<div class="info">
	<font class="fl">体重(kg):</font>
	<input type="text" id="info-weight" class="fr" maxlength="3"/>
	<div class="fc"></div>
</div>
<div class="info">
	<font class="fl">性别:</font>
	<select id="info-sex" class="fr">
        <option value=""></option> 
        <option value="True">男</option>  
        <option value="False">女</option>  
	</select>
	<div class="fc"></div>
</div>
<div class="info">
	<font class="fl">生日:</font>
	<input type="text" id="info-age" class="fr"/>
	<div class="fc"></div>
</div>
<div class="info">
	<font class="fl">劳动强度:</font>
	<input type="text" id="info-labour" class="fr"/>
	<div class="fc"></div>
</div>
<div class="info">
	<font class="fl">体质:</font>
	<%--<input type="text" id="info-constitution" class="fr"/>--%>
    <select id="info-constitution" class="fr">
<option value="平和质">平和质</option>
<option value="气郁质">气郁质</option>
<option value="阴虚质">阴虚质</option>
<option value="痰湿质">痰湿质</option>
<option value="阳虚质">阳虚质</option>
<option value="特禀质">特禀质</option>
<option value="湿热质">湿热质</option>
<option value="气虚质">气虚质</option>
<option value="血瘀质">血瘀质</option>
</select>
	<div class="fc"></div>
</div>

<div class="info">
	<font class="fl">健康分数:</font>
	<input type="text" id="info-score" class="fr"/>
	<div class="fc"></div>
</div>
                    <div class="fc"></div>
                </div>
                <div class="footer">
                    <button type="button" class="btn btn-save">
                        确定
                    </button>
                </div>
            </div>
        </div>
    </div>

</body>
</html>
