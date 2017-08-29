var isClicked = false;

$(document).ready(function () {
    $('.btn-withdraw').click(function () {
        $('#modal1').modal();
    })

    $('#number').keydown(function (e) {
        if (e.keyCode == 13)
            $('.btn-save').click();
    })

    getDetailTable(jsonDetail);
    getWithdrawTable(jsonWithdraw);

    $('.btn-save').click(function () {
        if (!isClicked) {
            var number = $('#number').val()
            if (number == '') {
                alert('不能为空');
                return;
            }
            number = parseFloat(number).toFixed(2);
            if (number == 0) {
                alert('不能取0')
                return;
            }
            var r = confirm('确定提取' + number + '元?')
            if (r) {
                //alert(n);
                //return;
                var data = {
                    number: number,
                    sellerId: sellerId,
                    method: 'withdraw'
                }
                isClicked = true;
                $.ajax({
                    type: 'post',
                    data: data,
                    url: 'BalanceSeller.aspx',
                    cache: false,
                    success: function (data) {
                        //getTable(data);
                        if (data == 'no-money') {
                            alert('余额不足')
                            isClicked = false;
                        }
                        else {
                            alert('成功')
                            location.reload();
                        }
                    },
                    error: function (err) {
                        alert('cuole');
                    }
                })
            }
        }
        })
       
})

function getDetailTable(data) {
    var json = JSON.parse(data).Table1;

    if (json == null) {
        //alert('暂无收入记录');
        
        return;
    }

    $('.cash-detail tbody tr').remove();
    var l = json.length;
    for (var i = 0; i < l; i++) {
        var h = '<tr style="background-color: white;">'
    + '<td>'+json[i].CreateTime+'</td>'
    + '<td class="money-color">+' + json[i].Pay + '</td>'
    + '<td>' + json[i].recipe + '</td>'
    + '<td>' + json[i].customer + '</td></tr>'
        $('.cash-detail tbody').append(h);
    }

    var pages = JSON.parse(data).pages;
    var thePage = JSON.parse(data).thePage;
    var h = getPageHtml(pages, thePage);
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
    $('.cash-detail nav').remove();
    $('.cash-detail .table-wrap').after(h);
}

function getWithdrawTable(data) {
    var json = JSON.parse(data).Table1;

    if (json == null) {
        //alert('暂无提现记录');

        return;
    }

    $('.cash-withdraw tbody tr').remove();
    var l = json.length;
    for (var i = 0; i < l; i++) {
        var h = '<tr style="background-color: white;">'
+ '<td>'+json[i].applyTime+'</td>'
+ '<td class="money-color">+' + json[i].applyMoney + '</td>'
        var state = json[i].applyState == 'True' ? '提现成功' : '审核中'
h+= '<td>' + state+ '</td></tr>'
        $('.cash-withdraw tbody').append(h);
    }

    var pages = JSON.parse(data).pages;
    var thePage = JSON.parse(data).thePage;
    var h = getPageHtml2(pages, thePage);
    //h = '<nav style="text-align:center;display:block">'
    //+ '<ul class="pagination">'
    //if (thePage == 1)
    //    h += '<li class="disabled"><a href="#">&laquo;</a></li>'
    //else
    //    h += '<li onclick="getPrePage2()"><a href="#">&laquo;</a></li>'
    ////+ '<li class="active"><a href="#">1</a></li>';

    //for (var i = 1; i < pages + 1; i++) {
    //    if (i == thePage)
    //        h += '<li class="active"><a >' + i + '</a></li>'
    //    else
    //        h += '<li onclick="getPage2(this)"><a >' + i + '</a></li>'
    //    if (i == pages && i == thePage)
    //        h += '<li class="disabled" ><a >&raquo;</a></li>'
    //    else if (i == pages)
    //        h += '<li onclick="getNextPage2()" ><a>&raquo;</a></li>'
    //}

    //h += '</ul>'
    //+ '</nav>'
    $('.cash-withdraw nav').remove();
    $('.cash-withdraw .table-wrap').after(h);
}

function changePage(page) {
    var data = {
        method: 'search',
        thePage: page,
        sellerId: sellerId,
    }
    
    $.ajax({
        type: 'post',
        data: data,
        url: 'BalanceSeller.aspx',
        cache: false,
        success: function (data) {
            getDetailTable(data);
        
        },
        error: function (err) {
            alert('cuole');
        }
    })
}


function changePage2(page) {
    var data = {
        method: 'search2',
        thePage: page,
        sellerId: sellerId,
    }

    $.ajax({
        type: 'post',
        data: data,
        url: 'BalanceSeller.aspx',
        cache: false,
        success: function (data) {
            getWithdrawTable(data);

        },
        error: function (err) {
            alert('cuole');
        }
    })
}

function getPage2(node) {
    var thePage = node.innerText;
    //alert(thePage);
    changePage2(thePage);
}

function getNextPage2() {
    //var count = $('.cash-withdraw .pagination li').length;
    ////alert(count);
    //for (var i = 0; i < count; i++) {
    //    if ($('.cash-withdraw .pagination li:eq(' + i + ')').attr('class') == 'active') {
    //        changePage2(i + 1);
    //        break;
    //    }
    //}
    $('.cash-withdraw .pagination li').each(function () {
        if ($(this).attr('class') == 'active') {
            var a = $(this).children('a').text();
            var next = parseInt(a) + 1;
            changePage2(next);
            return false;
        }
    })
}

function getPrePage2() {
    //var count = $('.cash-withdraw .pagination li').length;
    ////alert(count);
    //for (var i = 0; i < count; i++) {
    //    if ($('.cash-withdraw .pagination li:eq(' + i + ')').attr('class') == 'active') {
    //        changePage2(i - 1);
    //        break;
    //    }
    //}
    $('.cash-withdraw .pagination li').each(function () {
        if ($(this).attr('class') == 'active') {
            var a = $(this).children('a').text();
            var pre = parseInt(a) - 1;
            changePage2(pre);
            return false;
        }
    })

}