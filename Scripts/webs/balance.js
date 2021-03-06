﻿var isHasResult = true;
$(document).ready(function () {
    $('.input-search').keydown(function (e) {
        if (e.keyCode == 13)
            $('.btn-search').click();
    })

    //getTable(jsonStr);
    changePage(1);

    $('.btn-search').click(function () {
        changePage(1);
    })

    $('body').delegate('tbody tr', 'click', function () {

        var id = $(this).children('td:first').html();
        location.href = 'balanceSeller.aspx?sellerId=' + id;
    })
})

function getTable(data) {
    var json = JSON.parse(data).Table1;
    isHasResult = true;
    if (json == null) {
        $('#divMain3 .table-wrap tbody tr').remove();
        $('#divMain3').next().remove();
        isHasResult = false;
        return;
    }
    var count = json.length;
    //alert(count);
    var rows = 14;
    var temp = rows;
    var h = '<div class="fl table-wrap wrap1"><table class="table table-striped table-hover"><thead class="thead1"><tr><th width="1%">编号</th><th width="3%">商家</th><th width="3%">余额</th></tr></thead><tbody>'
    if (count <= rows)
        temp = count;
    for (var i = 0; i < temp; i++) {
        h += '<tr style="background-color: white;">'
    + '<td >' + json[i].id + '</td>'
        var name = omit(json[i].name, 12);
        h += '<td>' + name + '</td>'
    + '<td>' + json[i].balance + '元</td>'
    + '</tr>'
    }
    h += '</tbody></table></div>'
    if (count > rows) {
        h += '<div class="fl table-wrap wrap2"><table class="table table-striped table-hover"><thead class="thead1"><tr><th width="1%">编号</th><th width="3%">商家</th><th width="3%">余额</th></tr></thead><tbody>'
        if (count - rows <= rows)
            temp = count - rows;
        for (var i = rows ; i < temp + rows; i++) {
            h += '<tr style="background-color: white;">'
        + '<td >' + json[i].id + '</td>'
            var name = omit(json[i].name, 12);
            h += '<td>' + name + '</td>'
        + '<td>' + json[i].balance + '元</td>'
        + '</tr>'
        }
        h += '</tbody></table></div>'
        if (count - rows > rows) {
            h += '<div class="fl table-wrap wrap3"><table class="table table-striped table-hover"><thead class="thead1"><tr><th width="1%">编号</th><th width="3%">商家</th><th width="3%">余额</th></tr></thead><tbody>'
            if (count - rows * 2 <= rows)
                temp = count - rows * 2;
            for (var i = rows * 2 ; i < temp + rows * 2; i++) {
                h += '<tr style="background-color: white;">'
            + '<td >' + json[i].id + '</td>'
                var name = omit(json[i].name, 12);
                h += '<td>' + name + '</td>'
                + '<td>' + json[i].balance + '元</td>'
                + '</tr>'
            }
            h += '</tbody></table></div>'
        }
    }
    $('#divMain3 .table-wrap').remove();
    $('#divMain3 .table-bigWrap').prepend(h);

    var pages = JSON.parse(data).pages;
    var thePage = JSON.parse(data).thePage;

    var h = getPageHtml(pages, thePage);
    $('#divMain3').next().remove();
    $('#divMain3').after(h);
    //alert(h);
}


function changePage(page) {
    var search = $('.input-search').val();
    var data = {
        method: 'search',
        thePage: page,
        search: search,
    }

    $.ajax({
        type: 'post',
        data: data,
        //dataType: 'html',
        url: 'Balance.aspx',
        cache: false,
        success: function (data) {
            getTable(data);
            //if(!isHasResult)
            //    alert('没有结果')
        },
        error: function (err) {
            alert('cuole');
        }
    })
}

