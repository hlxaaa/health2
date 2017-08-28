var jsonStr;
var theClickFood;
var theClickTag;
var pageSize = 12;
var isHasResult = true;
//var types = new Array();
//var keys = new Array();
//var pageIndex;

//webSocket


function moreSelector() {
    $('#divMain2').attr('class', 'row row-max');
}

function getPrePage() {
    var count = $('.pagination li').length;
    //alert(count);
    for (var i = 0; i < count; i++) {
        if ($('.pagination li:eq(' + i + ')').attr('class') == 'active') {
            changePage(i - 1);
            break;
        }

    }
}

function getPage(node) {
        var thePage = node.innerText;
        //alert(thePage);
        changePage(thePage);
}

function getNextPage() {
    var count = $('.pagination li').length;
    //alert(count);
    for (var i = 0; i < count; i++) {
        if ($('.pagination li:eq(' + i + ')').attr('class') == 'active') {
            changePage(i + 1);
            break;
        }
    }
    
}

function clearFoodSelect() {
    var l = $('.row5-right span').length;
    for(var  i =0;i<l;i++){
        if ($('.row5-right span:eq(' + i + ')').text() == theClickFood) {
            $('.row5-right span:eq(' + i + ')').attr('class', 'label label-default');
            theClickFood = '';
            break;
        }
    }
}

function clearTagSelect() {
    var l = $('.row3-right div font').length;
    //alert($('.div-tag:eq(1) font').html())
    //alert($('.row3-right div:eq(0) div').attr('class'))
    for (var i = 0; i < l; i++) {
        if ($('.div-tag:eq('+i+') font').text() == theClickTag) {
            $('.div-tag:eq(' + i + ') div').attr('class', 'icheckbox_minimal');
            theClickTag = '';
            break;
        }
    }
}

function getTable(data) {
    var json = JSON.parse(data).Table1;
    isHasResult = true;
    if (json == null) {
        isHasResult = false;
        //alert('没有结果');
        clearFoodSelect();
        clearTagSelect();
        return;
    }
    var h = '<table class=\"table table-striped table-hover\">'
        + '<thead>'
        + '<tr>'
        + '<th width="1%"><input type="checkbox"></th>'
        //+ '<th width="1%">id</th>'
        + '<th width="5%">名称</th>'
        + '<th width="5%">有没有</th>'
        //+ '<th width="5%">菜品分类</th>'
        + '<th width="20%">菜品</th>'
        + '<th width="5%">餐厅</th>'
        + '<th width="5%">标签</th>'
        //+ '<th width="5%">图片</th>'
        + '<th width="5%">销售量</th>'
        + '<th width="5%">价格</th>'
        + '<th width="10%">操作</th>'
        + '</tr>'
        + '</thead>'
        + '<tbody>';
    for (var i = 0; i < json.length; i++) {
        h += '<tr  style="background-color:white;">';
        h += "<td><input type=\"checkbox\"></td>";
        //h += "<td>" + json[i].id + "</td>";
        h += "<td>" + json[i].name + "</td>";
        //var available = (json[i].available == 'True') ? '有' : '没有';
        h += "<td>" + json[i].available + "</td>";
        //h += "<td>" + json[i].foodtypes + "</td>";
        var foods = json[i].foods;
        if (foods.length > 20)
            foods = json[i].foods.replace(/\|/g, ' ').substring(0, 20) + '...';
        h += "<td>" + foods + "</td>";
        //h += "<td>" +foods[i] + "</td>";
        var rest = json[i].restaurantId;
        if (rest.length > 8)
            rest = json[i].restaurantId.substring(0, 8) + '...';
        h += "<td>" + rest + "</td>";
        var tags = json[i].tags;
        if (tags.length > 11)
            tags = json[i].tags.toString().substring(0,11)+ '...';
        h += "<td>" + tags + "</td>";
        //h += "<td>" + json[i].images + "</td>";
        h += "<td>" + json[i].sales + "</td>";
        h += "<td>" + json[i].price + "元</td>";
        h += '<td id="editDelete">'
            +'<input type="hidden" class="recipeId" value="'+json[i].id+'"/>'
            + '<a id="aRightBorder"><img class="btn-edit" src="/Images/recipe/icon-edit.svg" /><font>编辑</font></a> '
            + '<a class="btn-delete"><img class="btn-edit" src="/Images/recipe/icon-delete.svg" /><font>删除</font></a> '
        + '</td>';
        h += "</tr>";
    }
    h += '</tbody>';
    h += '</table>';
    $('#divMain3 table').remove();
    $('#divMain3').append(h);

    var pages = JSON.parse(data).pages;
    var thePage = JSON.parse(data).thePage;
    //h += '<nav style="text-align:center;display:block">'
    //+ '<ul class="pagination">'
    //if (thePage == 1)
    //    h += '<li class="disabled"><a href="#">&laquo;</a></li>'
    //else
    //    h += '<li onclick="getPrePage()"><a href="#">&laquo;</a></li>'
    ////+ '<li class="active"><a href="#">1</a></li>';

    //for (var i = 1; i < pages + 1; i++) {
    //    if (i == thePage)
    //        h += '<li class="active"><a >' + i + '</a></li>'
    //    else
    //        h += '<li onclick="getPage(this)"><a >' + i + '</a></li>'
    //    if (i == pages && i == thePage)
    //        h += '<li class="disabled" ><a >&raquo;</a></li>'
    //    else if (i == pages)
    //        h += '<li onclick="getNextPage()" ><a>&raquo;</a></li>'
    //}

    //h += '</ul>'
    //+ '</nav>'
    //$('#divMain3 table').remove();
    var h = getPageHtml(pages, thePage);

    $('#divMain3 nav').remove();
    $('#divMain3').append(h);
}

function changePage(page) {
   
    var timeRange = new Array();
    timeRange[0] = $('#inputDate1').val();
    timeRange[1] = $('#inputDate2').val();
   
    var foods = new Array();
    var spans = $('.row5-right span');
    //alert(spans.length);
    var j = 0;
    for (var i = 0; i < spans.length; i++) {
        if ($('.row5-right span:eq(' + i + ')').attr('class') == 'label label-select') {
            foods[j] = $('.row5-right span:eq(' + i + ')').html();
            j++;
        }
    }

    var tags = new Array();
    var l = $('.row3-right div input').length;
    var j = 0;
    for (var i = 0; i < l; i++) {
        //if ($('.row3-right div input:eq(' + i + ')').is(':checked')) {
        if ($('.div-tag:eq('+i+') div').hasClass('checked')) {
            tags[j] = $('.row3-right div font:eq(' + i + ')').html();
            j++;
        }
    }
    //alert(tags);
    var saleRange = new Array();
    saleRange[0] = $('.row2-mid input:eq(0)').val();
    saleRange[1] = $('.row2-mid input:eq(1)').val();
    var priceRange = new Array();
    priceRange[0] = $('.row2-right input:eq(0)').val();
    priceRange[1] = $('.row2-right input:eq(1)').val();

    var search = $('#inputSearch').val();

    var available = $('input:radio:checked').val();
    //alert(available);
  
    var data = {
        method: 'search',
        thePage: page,
        saleRange: saleRange,
        priceRange: priceRange,
        search: search,
        available: available,
        tags: tags,
        foods: foods,
        timeRange: timeRange,
        pageSize: pageSize
    }
    
  
    $.ajax({
        type: 'post',
        data: data,
        //dataType: 'html',
        url: 'Recipe.aspx',
        cache: false,
        success: function (data) {
            getTable(data);
            $('#divMain3 input').iCheck({
                checkboxClass: 'icheckbox_minimal',
                radioClass: 'iradio_minimal',
                increaseArea: '20%' // optional
            });
            $('a').css('text-decoration', 'none');
            allselectToggle();
            if(!isHasResult)
                alert('没有结果')
        },
        error: function (err) {
            alert('cuole');
        }
    })
    
}

function test() {
    alert(1);
}

//document.ready
//document.ready
//document.ready
var isAdd = false;
$(document).ready(function () {
    //日期选择控件初始化
    $('#inputDate1').datetimepicker({
        format: 'yyyy-mm-dd',
        language: 'ch',
        autoclose: true,
        minView: (0, 'month'),
    });
    $('#inputDate2').datetimepicker({
        format: 'yyyy-mm-dd',
        language: 'ch',
        autoclose: true,
        minView: (0, 'month'),
    });
    
    $('#inputDate1').css('border-radius', '0.3vw');
    $('#inputDate2').css('border-radius', '0.3vw');


    //$('#modalAdd').keydown(function (e) {
    //    if (e.keyCode == 13) {
    //        if (isAdd)
    //            $('#btnAdd').click();
    //        else
    //            $('#btnUpdate').click();
    //    }
    //})

    //导航高亮
    var li = $('.list-group li');
    for (var i = 0; i < li.length; i++) {
        if (li[i].className == 'active') {
            $('.list-group li:eq(' + i + ')').css('background', 'rgb(138,148,199)');
            $('.list-group li:eq(' + i + ')').append('<span><img id="icon-triangle" src="/Images/recipe/icon-triangle.png" /></span>');
            //$('#divNav').attr('overflow-x', 'hidden')
            break;
        }
    }

    $('.batchDelete').click(function () {
        var l = $('tr div[class="icheckbox_minimal checked"]').length;
        if (l < 1) {
            alert('请先选择');
            return;
        }
        var r = confirm('确定删除这些吗?');
        if (r == true) {
            var ids = new Array();
            $('tr div[class="icheckbox_minimal checked"]').each(function () {
                ids.push($(this).parent().parent().children('#editDelete').children('input').val());
            })
            var data = {
                method: 'batchDelete',
                ids: ids
            }
            $.ajax({
                type: 'post',
                data: data,
                url: 'Recipe.aspx',
                cache: false,
                success: function (res) {
                    location.reload();
                },
                error: function (res) {
                    alert(res);
                }
            })
        }
    })


    //日期判断
    $('#inputDate2').change(function () {
        var start = $('#inputDate1').val();
        var end = $('#inputDate2').val();
        if (end < start&&start!=''&&end!='') {
            alert('结束时间应晚于开始时间');
            $('#inputDate2').val('');
        }
    })

    $('#inputDate1').change(function () {
        var start = $('#inputDate1').val();
        var end = $('#inputDate2').val();
        if (end < start && start != '' && end != '') {
            alert('结束时间应晚于开始时间');
            $('#inputDate1').val('');
        }
    })

    //getTable(jsonStr);
    changePage(1);

    $('.btnSearch').click(function () {
        changePage(1);
    })

    getInputStyle();

    //$('body').delegate('.row2-left .iCheck-helper', 'click', function () {
    //    //changePage(1);
    //    var available = $('input:radio:checked').val();
    //    alert(available);
    //})

    $('.row2-left .iCheck-helper').click(function () {
        //if($(this).parent().attr('class')=='iradio_minimal hover checked')
        changePage(1);
        //alert(1);
        //test();
        //var available = $('input:radio:checked').val();
        //alert(available);

    })

    $('.row3-right ins').click(function () {
        theClickTag = $(this).parent().next().text();
        
        changePage(1);
    })
    
    $('.row5-right span').click(function () {
        theClickFood = $(this).text();
        changePage(1);
    })

    //切换高级搜索
    $('#btnMoreSelect').click(function () {
        if (this.className == 'dropdown') {
            $('#divMain2').attr('class', 'row-max');
            this.innerHTML = '收起<span class="caret">';
            this.className = 'dropup';
            pageSize = 7;
            changePage(1);
            $('.div-selector').attr('class', 'div-selector-h ')
        }
        else {
            $('#divMain2').attr('class', 'row-min');
            this.innerHTML = '高级<span class="caret">';
            this.className = 'dropdown';
            pageSize = 12;
            changePage(1);
            $('.div-selector-h').attr('class', 'div-selector')
        }
    })

    $('.row2-mid input').change(function(){
        var start =  $('.row2-mid input:eq(0)').val().trim();
        var end = $('.row2-mid input:eq(1)').val().trim();
        if ((start != '') && (end != '')) {
            if (start > end)
                alert('销售量区间错误');
        }
    })

    $('.row2-right input').change(function () {
        //alert(1);
        var start = parseInt($('.row2-right input:eq(0)').val());
        var end =  parseInt($('.row2-right input:eq(1)').val());
        if (start != '' && end != '') {
            if (start > end)
                alert('价格区间错误');
        }
    })

    //全选
    allselectToggle();

    $('body').delegate('#aRightBorder', 'click', function () {
        var id =$(this).prev().val();
        window.location.href = '/webs/RecipeContent.aspx?id='+id;
    })

    $('body').delegate('.btn-delete', 'click', function () {
        var page = $('.pagination li[class="active"] a').text();
        var id = $(this).prev().prev().val();
        var r = confirm('确定删除吗？')
        if (r == true) {
            var data = {
                method: 'deleteRecipe',
                id:id
            }
            $.ajax({
                type: 'post',
                data: data,
                url: 'Recipe.aspx',
                cache: false,
                success: function (res) {
                    changePage(page);
                },
                error: function (res) {

                }
            })
        }
    })
})

function allselectToggle() {
    $('thead .iCheck-helper').click(function () {
        if ($(this).parent().attr('class') == 'icheckbox_minimal hover checked') {
            //var l = $('.ques-select').length;
            $('tbody .icheckbox_minimal').attr('class', 'icheckbox_minimal checked');
        }
        else {
            $('tbody .icheckbox_minimal').attr('class', 'icheckbox_minimal');
        }
    })
}

//function search() {
//    changePage(1);
//////}

function clickFood(node) {
    if (node.className == 'label label-default')
        node.className = 'label label-select'
    else
        node.className = 'label label-default'
}

















function tipsClear() {
    $('#divName').css('border-color', '#515151');
    $('#divPrice').css('border-color', '#515151');
    $('#selectTags').css('border-color', '#515151');

    var countTr = $('#tbodyFood tr').length;
    //var foodType = new Array(count);
    //for (var i = 0; i < countTr; i++) {
    //    foodType[i] = $('#tbodyFood tr:eq(' + i + ') td select').val();
    //}
    //var food = new Array(countTr);
    var weight = new Array(countTr);
    for (var i = 0; i < countTr; i++) {
        var l = $('#tbodyFood tr:eq(' + i + ') #tdFood div').length - 1;
        //food[i] = new Array(l);
        weight[i] = new Array(l);
        for (var j = 0; j < l; j++) {
            //food[i][j] = $('#tbodyFood tr:eq(' + i + ') #tdFood div:eq(' + j + ') select').val();
            var wInput = $('#tbodyFood tr:eq(' + i + ') #tdFood div:eq(' + j + ') input');
            wInput.css('border-color', '#515151');
        }
    }

}

function addRecipe() {
    tipsClear();
    var name = $('#divName').val();
    if (name == '') {
        $('#divName').css('border-color', 'red');
        return;
    }
    var available = $('#selectAvailable').val();
    var rest = $('#selectRest').val();
    var price = $('#divPrice').val();
    if (price == '') {
        $('#divPrice').css('border-color', 'red');
        return;
    }
    var sales = $('#divSales').val();
    //alert(name+available+rest+price+sales);

    var count = $('#divTags span').length;
    //alert(count);
    if (count == 0) {
        $('#selectTags').css('border-color', 'red');
        return;
    }
    var tags = new Array(count);
    for (var i = 0; i < count; i++) {

        tags[i] = $('#divTags span:eq(' + i + ')  h6').html();
    }

    var countTr = $('#tbodyFood tr').length;
    var foodType = new Array(count);
    for (var i = 0; i < countTr; i++) {
        foodType[i] = $('#tbodyFood tr:eq(' + i + ') td select').val();
    }

    var food = new Array(countTr);
    var weight = new Array(countTr);
    for (var i = 0; i < countTr; i++) {
        var l = $('#tbodyFood tr:eq(' + i + ') #tdFood div').length - 1;
        food[i] = new Array(l);
        weight[i] = new Array(l);
        for (var j = 0; j < l; j++) {
            food[i][j] = $('#tbodyFood tr:eq(' + i + ') #tdFood div:eq(' + j + ') select').val();
            var wInput = $('#tbodyFood tr:eq(' + i + ') #tdFood div:eq(' + j + ') input');
            //alert(wInput.val());
            if (wInput.val() == '') {
                wInput.css('border-color', 'red');
                return;
            }
            weight[i][j] = wInput.val();
            //if (j == l - 1) {
            //    food[i][j] += "|";
            //    weight[i][j] += "|";
            //}
        }
    }
    //alert(food);
    var data = {
        name: name,
        available: available,
        rest: rest,
        price: price,
        sales: sales,
        tags: tags,
        foodType: foodType,
        food: food,
        weight: weight
    }

    $.ajax({
        type: "post",
        data: data,
        url: "Recipe.aspx?method=add",
        //url: "Recipe.aspx?method=add&name=" + name + "&available=" + available + "&rest=" + rest + "&price=" + price + "&sales=" + sales + "&tags=" + tags + "&foodType=" + foodType + "&food=" + food + "&weight=" + weight,
        cache: false,
        success: function (data) {
            if (data == "exist")
                alert("已存在该餐厅")
            else
                location.reload();
        },
        error: function (err) {
            alert("出问题了");
        }
    });
}

function updateRecipe() {
    tipsClear();
    var id = $('#inputId').val();
    var name = $('#divName').val();
    var available = $('#selectAvailable').val();
    var rest = $('#selectRest').val();
    var price = $('#divPrice').val();
    var sales = $('#divSales').val();

    var count = $('#divTags span').length;
    var tags = new Array(count);
    for (var i = 0; i < count; i++) {
        tags[i] = $('#divTags span:eq(' + i + ')  h6').html();
    }

    var countTr = $('#tbodyFood tr').length;
    var foodType = new Array(count);
    for (var i = 0; i < countTr; i++) {
        foodType[i] = $('#tbodyFood tr:eq(' + i + ') td select').val();
    }

    var food = new Array(countTr);
    var weight = new Array(countTr);
    for (var i = 0; i < countTr; i++) {
        var l = $('#tbodyFood tr:eq(' + i + ') #tdFood div').length - 1;
        food[i] = new Array(l);
        weight[i] = new Array(l);
        for (var j = 0; j < l; j++) {
            food[i][j] = $('#tbodyFood tr:eq(' + i + ') #tdFood div:eq(' + j + ') select').val();
            weight[i][j] = $('#tbodyFood tr:eq(' + i + ') #tdFood div:eq(' + j + ') input').val();
            //if (j == l - 1) {
            //    food[i][j] += "|";
            //    weight[i][j] += "|";
            //}
        }
    }

    var data = {
        name: name,
        available: available,
        rest: rest,
        price: price,
        sales: sales,
        tags: tags,
        foodType: foodType,
        food: food,
        weight: weight,
        id: id
    }

    $.ajax({
        type: "post",
        data: data,
        url: "Recipe.aspx?method=update",
        //url: "Recipe.aspx?method=update&name=" + name + "&available=" + available + "&rest=" + rest + "&price=" + price + "&sales=" + sales + "&tags=" + tags + "&foodType=" + foodType + "&food=" + food + "&weight=" + weight + "&id=" + id,
        cache: false,
        success: function (data) {
            if (data == "exist")
                alert("已存在该食谱")
            else
                location.reload();
        },
        error: function (err) {
            alert("出问题了");
        }
    });
}

//$(document).ready(function () {
//    //$('#trModel').hide();
//});

function deleteTags(node) {
    var a = node.parentNode;
    a.parentNode.removeChild(a);
}

function modalAdd() {
    tipsClear();
    isAdd = true;
    document.getElementById("btnAdd").style.display = '';
    document.getElementById("btnUpdate").style.display = 'none';
    $('#modalAdd').modal();
    $('#divName').val('');
    $('#selectAvailable').prop('selectedIndex', 0);
    $('#selectRest').prop('selectedIndex', 0);
    $('#divPrice').val('');
    $('#divSales').val('');
    $('#divTags span').remove();

    $('#tbodyFood tr:first').nextAll().remove();
    $('#tbodyFood tr:eq(0) td:eq(1) #divFood').first().nextAll('#divFood').remove();
}

function getOption() {
    var a = $('#selectTags option:selected').val();
    //alert(a);
    $('#divTags').prepend("<span class='label label-default' style='float:left'><h6>" + a + "</h6><a  class='btn btn-info' onclick='deleteTags(this)'>删除</a></span>");
}

function addFoodType() {
    var a = document.getElementById("trModel");
    var b = a.cloneNode(true);

    $('#tbodyFood').append(b);
}

function deleteFoodType(node) {
    var a = node.parentNode.parentNode;
    if (a.parentNode.children.length > 1)
        a.parentNode.removeChild(a);
}

function deleteFood(node) {
    var a = node.parentNode;
    if (a.parentNode.children.length > 2)
        a.parentNode.removeChild(a);
}

function addFood(node) {
    var a = document.getElementById("divFood");
    var b = a.cloneNode(true);
    //$('#tdFood').prepend(b);
    node.parentNode.parentNode.prepend(b);
}

function deleteRecipe(id) {
    var r = confirm("确认删除吗？")
    if (r == true) {
        var div = document.getElementById("div" + id);
        div.parentNode.removeChild(div);
        $.ajax({
            type: "post",
            url: "Recipe.aspx?method=delete&id=" + id,
            cache: false,
            success: function (data) {
                //alert("ok");
            },
            error: function (err) {
                alert("出问题了");
            }
        });
    }
}

function modalUpdate(id) {
    tipsClear();
    isAdd = false;
    document.getElementById("btnAdd").style.display = 'none';
    document.getElementById("btnUpdate").style.display = '';
    $('#inputId').val(id);
    $('#modalAdd').modal();

    $('#divName').val(document.getElementById("pName" + id).innerText);
    $('#selectAvailable').val($('#info' + id + ' #available').html());
    $('#selectRest').val($('#info' + id + ' #restId').html());
    $('#divPrice').val($('#info' + id + ' #price').html());
    $('#divSales').val($('#info' + id + ' #sales').html());
    $('#divTags span').remove();
    var a = new Array();
    a = $('#info' + id + ' #tags').html().split(',');
    for (var i = a.length - 1; i >= 0; i--) {
        $('#divTags').prepend("<span class='label label-default' style='float:left'><h6>" + a[i] + "</h6><a class='btn btn-info' onclick='deleteTags(this)'>删除</a></span>");
    }

    $('#tbodyFood tr:first').nextAll().remove();
    var countTr = $('#tbodyFood' + id + ' tr').length;
    for (var i = 0; i < countTr - 1; i++) {
        addFoodType();
    }
    for (var i = 0; i < countTr; i++) {
        var s = $('#tbodyFood' + id + ' tr:eq(' + i + ') td:eq(0)').html();
        $('#tbodyFood tr:eq(' + i + ') td:eq(0) select').val(s);

        var td2 = $('#tbodyFood' + id + ' tr:eq(' + i + ') td:eq(1)').html();
        var arr = new Array();
        arr = td2.split(';');
        $('#tbodyFood tr:eq(' + i + ') td:eq(1) #divFood').first().nextAll('#divFood').remove();
        for (var j = 0; j < arr.length - 1; j++) {
            var td = $('#tbodyFood tr:eq(' + i + ') td:eq(1)');
            var a = document.getElementById("divFood");
            var b = a.cloneNode(true);
            //$('#tdFood').prepend(b);
            td.prepend(b);
        }
        for (var j = 0; j < arr.length; j++) {
            var food = arr[j].split(',')[0];
            var weight = arr[j].replace(/[^0-9]+/g, '');
            $('#tbodyFood tr:eq(' + i + ') td:eq(1) div:eq(' + j + ') select').val(food);
            $('#tbodyFood tr:eq(' + i + ') td:eq(1) div:eq(' + j + ') input').val(weight);
        }
    }

}

function modalImg(id) {

    $('#imgModal').modal();
    $('#modalImgs').children().remove();
    $('#idImg').val(id);
    var imgs = document.getElementById('divImg' + id).getElementsByTagName('img');
    $('#modalImgs').children().remove();
    for (var i = 0; i < imgs.length; i++) {
        var imgSrc = imgs[i].src;

        var img = "<img src=\'" + imgSrc + "\' style=\'width:70px\'/>"
        $('#modalImgs').append("<span>" + img + "<button onclick=\'deleteImg(this," + i + "," + id + ")\'>删除</button></span>");
    }
}

function deleteImg(node, i, id) {
    var parent = node.parentElement.parentElement;
    var deleteNode = node.parentElement;
    var l = parent.children.length;
    for (var j = 0; j < l; j++) {
        if (parent.children[j] == node.parentElement) {
            //alert(parent.children[j].innerHTML);
            //alert(node.parentElement.innerHTML);
            i = j;
            break;
        }
    }
    deleteNode.parentElement.removeChild(deleteNode);
    $.ajax({
        type: 'post',
        url: 'Recipe.aspx?method=deleteImg&index=' + i + '&id=' + id,
        cache: false,
        success: function (data) {
            //alert('success');
        },
        error: function (err) {
            alert(2);
        }
    })
}

//不用的
//function test() {
//    var type = new Array(2);
//    type[0] = 'name';
//    type[1] = 'available';
//    var key = new Array(2);
//    key[0] = '1';
//    key[1] = '有';
//    var data = {
//        method: 'search',
//        type: type,
//        key: key
//    }
//    $.ajax({
//        type: 'post',
//        data: data,
//        dataType: 'html',
//        url: 'Recipe.aspx',
//        cache: false,
//        success: function (data) {
//            getTable(data);

//        },
//        error: function (err) {
//            alert('cuole');
//        }
//    })
//}
