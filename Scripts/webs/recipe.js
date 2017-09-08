var jsonStr;
var theClickFood;
var theClickTag;
var pageSize = 14;
var isHasResult = true;

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
    for (var i = 0; i < l; i++) {
        //if ($('.row5-right span:eq(' + i + ')').text() == theClickFood) {
        $('.row5-right span:eq(' + i + ')').attr('class', 'label label-default');
        //theClickFood = '';
        //break;
    }
}

function clearTagSelect() {
    var l = $('.row3-right div font').length;
    for (var i = 0; i < l; i++) {
        //if ($('.div-tag:eq('+i+') font').text() == theClickTag) {
        $('.div-tag:eq(' + i + ') div').attr('class', 'icheckbox_minimal');
        //theClickTag = '';
        //break;
    }
}

function getTable(data) {
    var json = JSON.parse(data).Table1;
    isHasResult = true;
    if (json == null) {
        isHasResult = false;
        //alert('没有结果');
        //clearFoodSelect();
        //clearTagSelect();
        $('tbody tr').remove();
        $('nav').remove();
        return;
    }
    var h = '<table class=\"table table-striped table-hover\">'
        + '<thead>'
        + '<tr>'
        + '<th width="1%"><input type="checkbox"></th>'
        + '<th width="5%">名称</th>'
        + '<th width="5%">餐厅</th>'
        + '<th width="5%">有没有</th>'
          + '<th width="5%">标签</th>'
             + '<th width="5%">销售量</th>'
        + '<th width="5%">价格</th>'
        + '<th width="20%">菜品</th>'
        + '<th width="10%">操作</th>'
        + '</tr>'
        + '</thead>'
        + '<tbody>';
    for (var i = 0; i < json.length; i++) {
        h += '<tr  style="background-color:white;">';
        h += "<td><input type=\"checkbox\"></td>";
        var name = omit(json[i].name, 12);
        h += "<td>" + name + "</td>";
        var rest = omit(json[i].restaurantId, 8);
        h += "<td>" + rest + "</td>";
        h += "<td>" + json[i].available + "</td>";
        var tags = omit(json[i].tags, 11);
        h += "<td>" + tags + "</td>";
        h += "<td>" + json[i].sales + "</td>";
        h += "<td>" + json[i].price + "元</td>";
        var foods = omit(json[i].foods, 20);
        h += "<td>" + foods + "</td>";
        //h += "<td>" + json[i].images + "</td>";

        h += '<td id="editDelete">'
            + '<input type="hidden" class="recipeId" value="' + json[i].id + '"/>'
            + '<a id="aRightBorder"><img class="btn-edit" src="/Images/recipe/icon-edit.svg" /><font>编辑</font></a> '
            + '<a class="btn-delete"><img class="btn-edit" src="/Images/recipe/icon-delete.svg" /><font>删除</font></a> '
        + '</td>';
        h += "</tr>";
    }
    h += '</tbody>';
    h += '</table>';
    $('.table-wrap table').remove();
    $('.table-wrap').append(h);

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
        if ($('.div-tag:eq(' + i + ') div').hasClass('checked')) {
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
    var data;
    if (pageSize == 14) {
        data = {
            method: 'search',
            thePage: page,
            search: search,
            pageSize: pageSize
        }
    } else {

        data = {
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
            //if(!isHasResult)
            //    alert('没有结果')
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
        var page = $('.pagination li[class="active"] a').text();
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
                    //location.reload();
                    changePage(page);
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
        if (end < start && start != '' && end != '') {
            alert('结束时间应晚于开始时间');
            $('#inputDate2').val('');
        } else {
            changePage(1);
        }
    })

    $('#inputDate1').change(function () {
        var start = $('#inputDate1').val();
        var end = $('#inputDate2').val();
        if (end < start && start != '' && end != '') {
            alert('结束时间应晚于开始时间');
            $('#inputDate1').val('');
        } else {
            changePage(1);
        }
    })

    //getTable(jsonStr);
    changePage(1);

    //$('.btnSearch').click(function () {
    //    changePage(1);
    //})

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
            pageSize = 9;
            changePage(1);
            $('.div-selector').attr('class', 'div-selector-h ')
            $('.table-wrap').css('height', '458px');
        }
        else {
            $('#divMain2').attr('class', 'row-min');
            this.innerHTML = '高级<span class="caret">';
            this.className = 'dropdown';
            pageSize = 14;
            test();
            changePage(1);
            $('.div-selector-h').attr('class', 'div-selector')
            $('.table-wrap').css('height', '683px');
        }
    })

    $('.row2-mid input:eq(0)').change(function () {
        var start = $('.row2-mid input:eq(0)').val().trim();
        var end = $('.row2-mid input:eq(1)').val().trim();
        if (start == '')
            start = 0;
        if (end == '')
            end = 9999999;
        if (parseInt(start) > parseInt(end)) {
            alert('销售量区间错误');
            $(this).val('')
        }
        else
            changePage(1);
    })

    $('.row2-mid input:eq(1)').change(function () {
        var start = $('.row2-mid input:eq(0)').val().trim();
        var end = $('.row2-mid input:eq(1)').val().trim();
        if (start == '')
            start = 0;
        if (end == '')
            end = 9999999;
        if (parseInt(start) > parseInt(end)) {
            alert('销售量区间错误');
            $(this).val('')
        }
        else
            changePage(1);
    })

    $('.row2-right input:eq(0)').change(function () {
        var start = parseInt($('.row2-right input:eq(0)').val());
        var end = parseInt($('.row2-right input:eq(1)').val());
        if (start == '')
            start = 0;
        if (end == '')
            end = 999999;
        if (parseInt(start) > parseInt(end)) {
            alert('价格区间错误');
            $(this).val('')
        } else
            changePage(1);
    })

    $('.row2-right input:eq(1)').change(function () {
        var start = parseInt($('.row2-right input:eq(0)').val());
        var end = parseInt($('.row2-right input:eq(1)').val());
        if (start == '')
            start = 0;
        if (end == '')
            end = 999999;
        if (parseInt(start) > parseInt(end)) {
            alert('价格区间错误');
            $(this).val('')
        } else
            changePage(1);
    })

    //全选
    allselectToggle();

    $('body').delegate('#aRightBorder', 'click', function () {
        var id = $(this).prev().val();
        window.location.href = '/webs/RecipeContent.aspx?id=' + id;
    })

    $('body').delegate('.btn-delete', 'click', function () {
        var page = $('.pagination li[class="active"] a').text();
        var id = $(this).prev().prev().val();
        var r = confirm('确定删除吗？')
        if (r == true) {
            var data = {
                method: 'deleteRecipe',
                id: id
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

    $('#inputSearch').keydown(function (e) {
        if (e.keyCode == 13)
            $('.btnSearch').click();
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

function clickFood(node) {
    if (node.className == 'label label-default')
        node.className = 'label label-select'
    else
        node.className = 'label label-default'
}

function test() {
    $('input:radio:checked').attr('checked', false);
    $('.row2-left div').removeClass('checked');
    $('.row2-mid input').val('');
    $('.row2-right input').val('');
    clearTagSelect();
    clearFoodSelect();
    //clearTagSelect();
    $('.row4 input').val('')
    //changePage(1);

}