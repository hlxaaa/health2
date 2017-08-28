<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="base.aspx.cs" Inherits="WebApplication1._base" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="../Scripts/jquery-2.0.0.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <link href="../Content/bootstrap.css" rel="stylesheet" />

     <script src="../Scripts/datetimepicker/bootstrap-datetimepicker.js"></script>
    <link href="../Scripts/datetimepicker/bootstrap-datetimepicker.min.css" rel="stylesheet" />

    <link href="../Scripts/checkboxStyle/skins/minimal/minimal.css" rel="stylesheet" />
    <script src="../Scripts/checkboxStyle/icheck.js"></script>

    <script src="/Scripts/base.js?ver=<%=ran %>"></script>
    <link href="/Content/base.css" rel="stylesheet" />

   

</head>
<body>
     <div id="divTop" class="row">
        <div id="divTopLeft" >
            <font>系统管理平台</font>
        </div>
         <div id="divTopRight" >
             <ul id="ulTop">
                 <li>
                     <img class="iconTop" src="/Images/recipe/icon-admin.svg" alt="Alternate Text" />
                     <font>admin</font>
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
                    <img src="/Images/recipe/icon-food.svg" /><a>菜品管理</a>
                </li>
                 <li class="">
                    <img src="/Images/recipe/icon-foodtype.svg" /><a>菜品分类</a>
                </li>
                 <li class="">
                    <img src="/Images/recipe/icon-tag.svg" /><a>标签管理</a>
                </li>
                 <li class="">
                    <img src="/Images/recipe/icon-rest.svg" /><a>餐厅管理</a>
                </li>
                 <li class="active">
                    <img src="/Images/recipe/icon-recipe.svg" /><a>食谱管理</a>
                </li>
                  <li class="">
                    <img src="/Images/recipe/icon-article.svg" /><a>文章管理</a>
                </li>
                 <li class="">
                    <img src="/Images/recipe/icon-bell.svg" /><a>会员管理</a>
                </li>
                 <li class="">
                    <img src="/Images/recipe/icon-naire.svg" /><a>问卷管理</a>
                </li>
                 <li class="">
                    <img src="/Images/recipe/icon-bell.svg" /><a>后台商家</a>
                </li>
                 <li class="">
                    <img src="/Images/recipe/icon-bell.svg" /><a>待定管理</a>
                </li>

            </ul>

        </div>
        <div class="" id="divMains">
            <div class="row" id="divMain1">
                <div id="main1-left">
                    <h4>食谱管理</h4>
                </div>
                <div id="main1-right">
                   <a href="#" class="btn btn-this"><img src="/Images/recipe/icon-deleteSome.svg" /><font>批量删除</font></a>
                    <a href="#" class="btn btn-this"><img src="/Images/recipe/icon-add.png" /><font>增加</font></a>
                </div>
            </div>
            <div class="row row-min" id="divMain2"><%--改这里 --%>
                <div class="row">
                    <div class="row1-left">
                        <font>关键词:</font>
                    </div>
                     <div class="row1-mid">
                         <input id="inputSearch"/>
                         <div class="btnSearch">
                             <img class="btn-search" src="/Images/recipe/icon-search.svg" />
                         </div>
                     </div>
                     <div class="row1-right">
                         <a id="btnMoreSelect" class="dropdown" href="#">高级<span class="caret"></span></a>
                     </div>
                </div>
                <div class="row">
                    <div class="row2-left">
                        <font>是否有货:</font>
                        <label>
                            有
                            <input type="radio" name="iCheck"  id="radioHas" value="True">
                            <%--<input type="radio" name="optionsRadios" id="radioHas" value="True">--%>
                        </label>
                        <label>
                            没有
                            <input type="radio" name="iCheck" id="radioNo" value="False">
                        </label>
                    </div>
                    <div class="row2-mid">
                        <font>销售量:</font>
                        <input onkeyup="value=value.replace(/[^\d]/g,'') "   
onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g,''))"  />
                        <span>—</span>
                        <input onkeyup="value=value.replace(/[^\d]/g,'') "   
onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g,''))"  />
                    </div>
                    <div class="row2-right">
                        <font>价格:</font>
                        <input onkeyup="value=value.replace(/[^\d]/g,'') "   
onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g,''))"  />
                        <span>—</span>
                        <input onkeyup="value=value.replace(/[^\d]/g,'') "   
onbeforepaste="clipboardData.setData('text',clipboardData.getData('text').replace(/[^\d]/g,''))"  />
                    </div>
                </div>
                <div class="row3">
                    <div class="row3-left">
                        <font>标签:</font>
                    </div>
                    <div class="row3-right">
                        
                            <%for (var i = 0; i < 10;i++ )
                              { %>
                        <div>
                            <input type="checkbox" class="tagcheckbox" />
                            <font>测试</font>
                        </div>
                         <%} %>
                      
                      

                    </div>
                </div>
                <div class="row4">
                    <font>创建时间:</font>
                    <div>
                        <input type="text" id="inputDate1" />
                    </div>
                    <span>—</span>
                    <div>
                        <input type="text" id="inputDate2" />
                    </div>
                </div>
                <div class="row5">
                    <div class="row5-left ">
                        <font>菜品:</font>
                    </div>
                    <div class="row5-right ">
                        <%for (var i = 0; i < 10;i++ )
                          { %>
                        <span onclick="clickFood(this)" class="label label-default">测试</span>
                        <%} %>
                    </div>
                </div>
            </div>
            <div class="row" id="divMain3">
               
            </div>

        </div>
    </div>
     <script>
         $('#divTop').click(function () {
             var a = '<%=test()[1]%>'
            alert(a);
        })

    </script>

</body>
</html>
