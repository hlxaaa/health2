$(document).ready(function () {
    $('body').delegate('.btn-do', 'click', function () {
        $(this).attr('class', 'btn-done');
        $(this).text('已处理')
        var withdrawId = $(this).parent().parent().children('td:first').html();
        var data = {
            method: 'doWithdraw',
            withdrawId:withdrawId
        }
        $.ajax({
            type: 'post',
            data: data,
            url: 'Withdraw.aspx',
            cache: false,
            success: function (data) {
                alert('已处理！')
                location.reload();
            },
            error: function (err) {
                alert('cuole');
            }
        })

    })
    //$('body').delegate('.btn-done', 'click', function () {
    //    $(this).attr('class', 'btn-do');
    //    $(this).text('处理')
    //})
    getTable(jsonStr);
})

function getTable(data) {
    var json = JSON.parse(data).Table1;

    if (json == null) {
        //alert('没有结果');
        //clearFoodSelect();
        //clearTagSelect();
        $('tbody tr').remove();
        $('#divMains nav').remove();
        return;
    }
    var l = json.length;
    $('tbody tr').remove();
    for (var i = 0; i < l; i++) {
        var h = '<tr style="background-color: white;">'
    + '<td>'+json[i].id+'</td>'
    + '<td>' + json[i].name + '</td>'
    + '<td>' + json[i].balance + '</td>'
    + '<td class="td-money">' + json[i].applyMoney + '</td>'
    + '<td>' + json[i].applyTime + '</td>'
    if(json[i].applyState=='False')
        h += '<td><span class="btn-do">处理</span></td><td></td></tr>'
        else
        h += '<td><span class="btn-done">已处理</span></td><td></td></tr>'

        $('tbody').append(h);
    }
    $('#divMains nav').remove();
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
    $('#divMain3').after(h);
}

function changePage(page) {
    var data = {
        method: 'search',
        thePage: page,
        id:id
    }


    $.ajax({
        type: 'post',
        data: data,
        //dataType: 'html',
        url: 'Withdraw.aspx',
        cache: false,
        success: function (data) {
            getTable(data);
           
        },
        error: function (err) {
            alert('cuole');
        }
    })
}