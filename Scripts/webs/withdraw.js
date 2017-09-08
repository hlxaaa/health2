var isClicked = false;

$(document).ready(function () {
    $('body').delegate('.btn-do', 'click', function () {
        if (!isClicked) {
            $(this).attr('class', 'btn-done');
            $(this).text('已处理')
            var withdrawId = $(this).parent().parent().children('td:first').html();
            var data = {
                method: 'doWithdraw',
                withdrawId: withdrawId
            }
            isClicked = true;
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
        }
    })
    //$('body').delegate('.btn-done', 'click', function () {
    //    $(this).attr('class', 'btn-do');
    //    $(this).text('处理')
    //})
    //getTable(jsonStr);
    changePage(1);
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
    + '<td>' + json[i].id + '</td>'
    + '<td>' + json[i].name + '</td>'
    + '<td>' + json[i].balance + '</td>'
    + '<td class="td-money">' + json[i].applyMoney + '</td>'
    + '<td>' + json[i].applyTime + '</td>'
        if (json[i].applyState == 'False')
            h += '<td><span class="btn-do">处理</span></td><td></td></tr>'
        else
            h += '<td><span class="btn-done">已处理</span></td><td></td></tr>'

        $('tbody').append(h);
    }
    $('#divMains nav').remove();
    var pages = JSON.parse(data).pages;
    var thePage = JSON.parse(data).thePage;
    var h = getPageHtml(pages, thePage);
    $('#divMain3').after(h);
}

function changePage(page) {
    var data = {
        method: 'search',
        thePage: page,
        //id: id
    }

    $.ajax({
        type: 'post',
        data: data,
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