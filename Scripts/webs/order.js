$(document).ready(function () {
    $('body').delegate('tr', 'click', function () {
        $('#modal1').modal();
        var info = new Array();
        var l = $(this).children('td').length;
        for (var i = 1; i < l; i++) {
            var modalIndex = i - 1;
            $('.info-font2:eq(' + modalIndex + ')').text($(this).children('td:eq(' + i + ')').html());
        }
        if ($(this).hasClass('bold')) {
            var id = $(this).children('td:eq(1)').html();
            var data = {
                method: 'clickOrder',
                id: id
            }
            $.ajax({
                type: 'post',
                data: data,
                url: 'Order.aspx',
                cache: false,
                success: function (data) {

                },
                error: function (err) {
                    alert('cuole');
                }
            })
        }
        $(this).children('td:eq(0)').html('');
        $(this).removeClass('bold');
    })
    //getTable(jsonStr);
    changePage(1)

    $('.selector-rest').change(function () {
        changePage(1);
    })

    $('#inputDate1').change(function () {
        changePage(1);
    })
    $('#inputDate2').change(function () {
        changePage(1);
    })

})

function getTable(data) {
    if (data == 'no-login') {
        location.href = '/error.aspx';
        return;
    }
    var json = JSON.parse(data).Table1;

    if (json == null) {
        //alert('没有结果');
        //clearFoodSelect();
        //clearTagSelect();
        $('#divMain3 tbody tr').remove();
        $('#divMain3 nav').remove();
        return;
    }
    var l = json.length;
    var h = '<table class="table table-striped table-hover">'
+ '<thead class="thead1">'
+ '<tr>'
+ '<th width="1%"></th>'
+ '<th width="3%">订单号</th>'
+ '<th width="3%">商家</th>'
+ '<th width="3%">食谱</th>'
+ '<th width="3%">价格</th>'
+ '<th width="3%">客户</th>'
+ '<th width="3%">到店时间</th>'
+ '<th width="3%">支付方式</th>'
+ '<th width="3%">联系方式</th>'
+ '<th width="3%">下单时间</th>'
+ '</tr>'
+ '</thead>'
+ '<tbody>'
    for (var i = 0; i < l; i++) {
        if (json[i].isNew == 'True') {
            h += '<tr style="background-color: white;" class="bold">'
            + '<td><img src="/Images/base/icon-new.svg" class="icon-new" /></td>'
        }
        else {
            h += '<tr style="background-color: white;">'
            + '<td></td>'
        }
        //h+= '<td><img src="/Images/base/icon-new.svg" class="icon-new" /></td>'
        h += '<td>' + json[i].id + '</td>'
        rest = json[i].rest;
        if (rest.length > 10)
            rest = json[i].rest.substring(0, 10) + '...';
        h += '<td>' + rest + '</td>'
        + '<td>' + json[i].recipe + '</td>'
        + '<td>' + json[i].Pay + '</td>'
        + '<td>' + json[i].customer + '</td>'
        + '<td>' + json[i].ShopTime + '</td>'
        + '<td>' + json[i].PayType + '</td>'
        + '<td>' + json[i].phone + '</td>'
        + '<td>' + json[i].CreateTime + '</td>'
    }

    h += '</tr></tbody></table>'
    $('.table-wrap table').remove();
    $('.table-wrap').prepend(h)

    var pages = JSON.parse(data).pages;
    var thePage = JSON.parse(data).thePage;
    //h = '<nav style="text-align:center;display:block">'
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
    var h = getPageHtml(pages, thePage);
    $('#divMain3 nav').remove();
    $('#divMain3').append(h);
}

function changePage(page) {
    var startTime = $('#inputDate1').val();
    var endTime = $('#inputDate2').val();
    var restId = $('.selector-rest').val();
    var data = {
        startTime: startTime,
        endTime: endTime,
        id: restId,
        thePage: page,
        method: "search"
    }
    $.ajax({
        type: 'post',
        data: data,
        url: 'Order.aspx',
        cache: false,
        success: function (data) {
            getTable(data);
        },
        error: function (err) {
            alert('cuole');
        }
    })
}