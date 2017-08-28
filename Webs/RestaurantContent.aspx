<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RestaurantContent.aspx.cs" Inherits="WebApplication1.Webs.RestaurantContent" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>餐厅管理内页</title>

    <script src="../Scripts/jquery-2.0.0.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <link href="../Content/bootstrap.css" rel="stylesheet" />

    <script src="../Scripts/datetimepicker/bootstrap-datetimepicker.js"></script>
    <link href="../Scripts/datetimepicker/bootstrap-datetimepicker.min.css" rel="stylesheet" />

    <link href="../Scripts/checkboxStyle/skins/minimal/minimal.css" rel="stylesheet" />
    <script src="../Scripts/checkboxStyle/icheck.js"></script>

    <script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=N3VxQPm4CRqm7odgrMuopqIWoMRPqesL"></script>

    <script src="/Scripts/base.js?ver=<%=ran %>"></script>
    <script src="/Scripts/webs/restContent.js?ver=<%=ran %>"></script>
    <link href="/Content/base.css" rel="stylesheet" />
    <link href="/Content/webs/restContent.css" rel="stylesheet" />
     <script src="../Scripts/jquery-form.js"></script>

    <script type="text/javascript">
     var id = '<%=id%>'
    </script>
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
                <li class="active">
                    <img src="/Images/recipe/icon-rest.svg" /><a href="Restaurant.aspx">餐厅管理</a>
                </li>
                <li class="">
                    <img src="/Images/recipe/icon-recipe.svg" /><a href="Recipe.aspx">食谱管理</a>
                </li>
                <li class="">
                    <img src="/Images/recipe/icon-article.svg" /><a href="Article.aspx">文章管理</a>
                </li>
                <li class="">
                    <img src="/Images/recipe/icon-bell.svg" /><a href="Customer.aspx">会员查看</a>
                </li>
                <li class="li-allques">
                    <img src="/Images/base/icon-triangle-right.svg" /><a>问卷管理</a>
                </li>
                <li class="li-ques">
                    <img src="/Images/base/icon-easyQues.svg" /><a href="QuestionEasy.aspx">简易版</a></li>
                <li class="li-ques">
                    <img src="/Images/base/icon-proQues.svg" /><a href="QuestionPro.aspx">专业版</a></li>
                <li class="">
                    <img src="/Images/recipe/icon-bell.svg" /><a href="Seller.aspx">后台商家</a>
                </li>
                <li class="">
                    <img src="/Images/recipe/icon-bell.svg" /><a>待定管理</a>
                </li>

            </ul>

        </div>
        <div class="" id="divMains">
            <div class="main-header">
                <input type="hidden" id="recipeId" />
                <div class="header-content">
                    <h4>餐厅管理</h4>
                    <input type="hidden" id="restId" value="<%=id %>" />
                </div>
            </div>
            <div class="main-body">
                <div class="mainbody-title">
                    <div class="fl bold">
                        名称:
                    </div>
                    <div class="fl ">
                        <input type="text" id="title" value="<%=name %>" maxlength="25" />
                    </div>
                    <div class="fr btn-del">
                        <a  style="text-decoration: none;">
                            <img class="btn-edit" src="/Images/recipe/icon-delete.svg"><font class="bold font-base">删除</font></a>
                    </div>
                    <div class="fc"></div>
                </div>

                <div class="mainbody-info">
                    <div class="info1">
                        <font>
                            地址：
                        </font>
                        <font id="address">  
                            <%=address %>
                        </font>
                        <img src="/Images/base/icon-map.svg" alt="Alternate Text" class="img-base openMap" />
                        <font>
                             坐标：
                        </font>
                        <font id="coordinate">  
                            <%=coordinate %>
                        </font>
                    </div>
                    <div class="info2">
                        <div class="fl">
                            <font>
                                餐厅类别：
                            </font>
                            <select id="category">
                                <%foreach (var k in dictCate.Keys)
                                  {
                                      if (dictCate[k] == category)
                                      {%>
                                <option value="<%=k %>" selected="selected"><%=dictCate[k]%></option>
                                <%}
                                      else
                                      { %>
                                <option value="<%=k %>"><%=dictCate[k]%></option>
                                <%}
                                  }%>
                            </select>
                        </div>
                        <div class="fl">
                            <font>
                                联系电话：
                            </font>
                            <input type="text" id="phone" value="<%=phone %>" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" maxlength="11"/>
                        </div>
                        <div class="fl">
                            <font>
                                月销售量：
                            </font>
                            <input type="text" id="sales" value="<%=sales %>"  onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"/ maxlength="9">
                        </div>
                        <div class="fl">
                            <font>
                                人均消费：
                            </font>
                            <input type="text" id="consumption" value="<%=consumption %>" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" maxlength="9"/>
                        </div>
                        <div class="fl">
                            <font>
                                营业时间：
                            </font>
                            <input type="text" id="startTime" />

                            <span>-</span>
                            <input type="text" id="endTime" />

                        </div>
                        <div class="fc"></div>
                    </div>
                    <div class="info3">
                        <font class="fl">
                            优惠活动
                        </font>
                        <%for (var i = 0; i < discounts.Length; i++)
                          {
                              if (discounts[i] != "")
                              { %>
                        <span class="discount fl">
                            <input type="text" class="fl discount-input discount-input2" value="<%=discounts[i]%>" />
                            <img src="/Images/base/icon-deleteX.svg" alt="Alternate Text" class="fl img-base discount-del" />
                        </span>
                        <%}
                          } %>
                        <span class="discount fl">
                            <input type="text" class="fl discount-input" placeholder="输入新优惠活动" />
                            <font class="fl img-base discount-del2 add-discount">添加</font>
                        </span>
                        <div class="fc"></div>
                    </div>
                </div>

                <div class="mainbody-imgs">
                    <div class="font-tips">
                        注意：图片建议尺寸至少为400*300，以达到较好的展示效果！
                    </div>
                    <div>
                        <%for (var i = 0; i < imgs.Length; i++)
                          { %>
                        <div class="div-imgs fl">
                            <img src="/Images/recipe/icon-deleteSome.svg" class="img-del" />
                            <img class="img-show" src="/<%=imgs[i]%>" alt="img" />
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
                            <p>
                                上传图片
                            </p>
                        </div>
                        <form id="formid" method = 'post' enctype="multipart/form-data" action = 'RestaurantContent.aspx' style="visibility:hidden"  >
                            <input type="file" id="upImg" runat="server" onchange="filechange(event)" multiple="multiple" name="img" />
                            <input id="testSubmit" type="submit" value="上传"/>
                       </form>
                        <div class="fc"></div>
                    </div>
                </div>
            </div>
            <div class="show-recipe">
                <div>
                    <p class="bold">本餐厅食谱：</p>
                </div>
                <div>
                    <%foreach (var k in dictRecipe.Keys)
                      { %>
                    <a class="font-base col-md-2 recipeName" href="/webs/RecipeContent.aspx?id=<%=k %>"><%=dictRecipe[k] %></a>
                    <%} %>
                </div>
                <div class="fc"></div>
            </div>
            <div class="main-btns">
                <a class="btn-edit fl btn-save">保存修改</a>
                <a class="btn-edit fl btn-back">取消返回</a>
            </div>
        </div>
        <div class="fc"></div>
    </div>

    <%--模态框--%>
    <div class="modal fade" id="modal1" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="tag-selector" style="display: block;">
                    <div class="row selector-head">
                        <img class="btn-edit2" src="/Images/recipe/icon-edit.svg" alt="Alternate Text">
                        <font>添加标签</font>
                        <button type="button" class="close">×</button>
                    </div>
                    <hr class="row">
                    <div class="row selector-body">
                        <span class="label-default">测试3</span>
                    </div>
                    <hr class="row">
                    <div class="row selector-foot">
                        <button type="button" class="btn btn-tag">
                            确定
                        </button>
                    </div>
                </div>
            </div>

        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal -->

    <input type="hidden" id="oImg" value="<%=oImgs %>" />

    <div class="modal fade" id="div1" role="dialog" >
        <div class="modal-dialog m-dialog2" style="">
            <div class="modal-content ">
                <button type="button" class="close" data-dismiss="modal">×</button>
                <div class="fl map-left">
                    <div id="r-result">请输入:<input type="text" id="suggestId" size="20" value="百度" style="width: 150px; z-index: 9999" /></div>
                    <div id="searchResultPanel" style="border: 1px solid #C0C0C0; width: 150px; height: auto; display: none; z-index: 9999"></div>
                </div>
                <div class="fl map-right">
                    <div id="l-map">
                    </div>
                    <div class="map-coord">
                        <font>坐标</font>
                        <font>X</font>
                        <input type="text" id="coord-x" />
                        <font>Y</font>
                        <input type="text" id="coord-y" />
                    </div>
                    <a class="btn btn-base btn-getMap">确定</a>
                </div>
                <%--<div class="fc"></div>--%>
                <%--<div id="Div2" style="display: none"></div>--%>


                <%--<input readonly="true" id="shopcoord" type="text" style="width: 300px" />--%>


            </div>
        </div>

    </div>
    <script>
        var dt1 = '<%=startTime%>'
        var dt2 = '<%=endTime%>'
        //var t = date.toJSON();
        function G(id) {
            return document.getElementById(id);
        }

        var map = new BMap.Map("l-map");
        map.centerAndZoom("北京", 12);                   // 初始化地图,设置城市和地图级别。

        var ac = new BMap.Autocomplete(    //建立一个自动完成的对象
            {
                "input": "suggestId"
            , "location": map
            });

        ac.addEventListener("onhighlight", function (e) {  //鼠标放在下拉列表上的事件
            var str = "";
            var _value = e.fromitem.value;
            var value = "";
            if (e.fromitem.index > -1) {
                value = _value.province + _value.city + _value.district + _value.street + _value.business;
            }
            str = "FromItem<br />index = " + e.fromitem.index + "<br />value = " + value;

            value = "";
            if (e.toitem.index > -1) {
                _value = e.toitem.value;
                value = _value.province + _value.city + _value.district + _value.street + _value.business;
            }
            str += "<br />ToItem<br />index = " + e.toitem.index + "<br />value = " + value;
            G("searchResultPanel").innerHTML = str;
        });

        var myValue;
        ac.addEventListener("onconfirm", function (e) {    //鼠标点击下拉列表后的事件
            var _value = e.item.value;
            myValue = _value.province + _value.city + _value.district + _value.street + _value.business;
            getPoint(myValue);
            G("searchResultPanel").innerHTML = "onconfirm<br />index = " + e.item.index + "<br />myValue = " + myValue;

            setPlace();
        });

        function setPlace() {
            map.clearOverlays();    //清除地图上所有覆盖物
            function myFun() {
                var pp = local.getResults().getPoi(0).point;    //获取第一个智能搜索的结果
                map.centerAndZoom(pp, 18);
                map.addOverlay(new BMap.Marker(pp));    //添加标注
            }
            var local = new BMap.LocalSearch(map, { //智能搜索
                onSearchComplete: myFun
            });
            local.search(myValue);
        }
    </script>
</body>
</html>



