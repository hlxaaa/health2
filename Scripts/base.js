//document.ready
//document.ready
//document.ready
var isAdd = false;
$(document).ready(function () {

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

    $('#inputDate1').css('border-radius', '8px');
    $('#inputDate2').css('border-radius', '8px');

    //$('#modalAdd').keydown(function (e) {
    //    if (e.keyCode == 13) {
    //        if (isAdd)
    //            $('#btnAdd').click();
    //        else
    //            $('#btnUpdate').click();
    //    }
    //})

    var li = $('.list-group li');
    for (var i = 0; i < li.length; i++) {
        if($('.list-group li:eq('+i+')').hasClass('active')){
        //if (li[i].className == 'active') {
            $('.list-group li:eq(' + i + ')').css('background', 'rgb(138,148,199)');
            $('.list-group li:eq(' + i + ')').append('<span><img id="icon-triangle" src="/Images/recipe/icon-triangle.png" /></span>');
            //$('#divNav').attr('overflow-x', 'hidden')
            break;
        }
    }

    $('#inputDate2').change(function () {
        var start = $('#inputDate1').val();
        var end = $('#inputDate2').val();
        if (end < start && start != '' && end != '') {
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

   



    $('a').css('text-decoration', 'none');

    $('input').iCheck({
        checkboxClass: 'icheckbox_minimal',
        radioClass: 'iradio_minimal',
        increaseArea: '20%',
    });

    $('.btnSearch').click(function () {
        //if ($(this).prev().val().trim()=='') {
        //    alert('请输入关键词');
        //}
        //else
            changePage(1);
    })

    $('body').delegate('#ulTop li:eq(1)', 'click', function () {
        location.href = '/Webs/Order.aspx';
    })

    //点击图片也能转到页面
    $('.list-group li').click(function () {
        if ($(this).attr('class') != 'li-allques' && $(this).attr('class') != 'li-allbacks') {
            var url = $(this).children('a').attr('href');
            window.location.href = url;
        }
    })

    $('.li-allques').click(function () {
        if ($('.li-allques img').attr('src') == '/Images/base/icon-triangle-right.svg') {
            $('.li-allques img').attr('src', '/Images/base/icon-triangle-down.svg');
            $('.li-ques').css('display', 'block');
        }
        else {
            $('.li-allques img').attr('src', '/Images/base/icon-triangle-right.svg');
            $('.li-ques').css('display', 'none');
        }
    })

    $('.li-allbacks').click(function () {
        if ($('.li-allbacks img').attr('src') == '/Images/base/icon-triangle-right.svg') {
            $('.li-allbacks img').attr('src', '/Images/base/icon-triangle-down.svg');
            $('.li-back').css('display', 'block');
        }
        else {
            $('.li-allbacks img').attr('src', '/Images/base/icon-triangle-right.svg');
            $('.li-back').css('display', 'none');
        }
    })


    //退出
    $('.iconTop2:eq(1)').click(function () {
        var r = confirm('确认退出吗？')
        if (r) {
            $.ajax({
                type: 'post',
                //data: data,
                url: '/Login.aspx?method=loginOut',
                cache: false,
                success: function (data) {
                    location.href = '/error.aspx';
                },
                error: function (err) {
                    alert('cuole');
                }
            })
        }
    })

    pushTips(8)
})

function getPrePage() {
    $('.pagination li').each(function () {
        if ($(this).attr('class') == 'active') {
            var a = $(this).children('a').text();
            var pre = parseInt(a) - 1;
            changePage(pre);
            return false;
        }
    })
}

function getPage(node) {
    var thePage = node.innerText;
    changePage(thePage);
}

function getNextPage() {
    $('.pagination li').each(function () {
        if ($(this).attr('class') == 'active') {
            var a = $(this).children('a').text();
            var next = parseInt(a) + 1;
            changePage(next);
            return false;
        }
    })
}

function getInputStyle() {
    $('input').iCheck({
        checkboxClass: 'icheckbox_minimal',
        radioClass: 'iradio_minimal',
        increaseArea: '20%', // optional
    });
    $('a').css('text-decoration', 'none');
    $('.allselect .iCheck-helper').click(function () {
        allselectToggle();
    })
    //$('.icheckbox_minimal').css('background-color', 'rgb(234,237,242)');
    //$('.allselect .icheckbox_minimal').css('background-color', 'white');
}

function getPageHtml(pages, thePage) {
    if (pages <= 5) {
        h = '<nav style="text-align:center;display:block">'
        + '<ul class="pagination">'
        if (thePage == 1)
            h += '<li class="disabled"><a href="#">&laquo;</a></li>'
        else
            h += '<li onclick="getPrePage()"><a href="#">&laquo;</a></li>'
        //+ '<li class="active"><a href="#">1</a></li>';

        for (var i = 1; i < pages + 1; i++) {
            if (i == thePage)
                h += '<li class="active"><a >' + i + '</a></li>'
            else
                h += '<li onclick="getPage(this)"><a >' + i + '</a></li>'
        }
        if (pages == thePage)
            h += '<li class="disabled" ><a >&raquo;</a></li>'
        else
            h += '<li onclick="getNextPage()" ><a>&raquo;</a></li>'
        h += '</ul>'
        + '</nav>'
    } else {
        if (thePage <= 2) {
            h = '<nav style="text-align:center;display:block"><ul class="pagination">'
            if (thePage == 1)
                h += '<li class="disabled"><a href="#">&laquo;</a></li>'
            else
                h += '<li onclick="getPrePage()"><a href="#">&laquo;</a></li>'

            for (var i = 1; i < pages + 1; i++) {
                if (i == pages) {
                    h += '<li><a>...</a></li>'
                }
                if (i <= thePage + 2 || i == pages) {
                    if (i == thePage)
                        h += '<li class="active"><a >' + i + '</a></li>'
                    else
                        h += '<li onclick="getPage(this)"><a >' + i + '</a></li>'
                }
            }

            if (pages == thePage) {

                h += '<li class="disabled" ><a >&raquo;</a></li>'
            }
            else {

                h += '<li onclick="getNextPage()" ><a>&raquo;</a></li>'
            }
            h += '</ul></nav>'
        } else {
            if (thePage < 4) {
                h = '<nav style="text-align:center;display:block"><ul class="pagination">'
                if (thePage == 1)
                    h += '<li class="disabled"><a href="#">&laquo;</a></li>'
                else
                    h += '<li onclick="getPrePage()"><a href="#">&laquo;</a></li>'

                for (var i = 1; i < pages + 1; i++) {
                    if (i == pages && thePage < pages - 3) {
                        h += '<li><a>...</a></li>'
                    }
                    if (i <= thePage + 2 && i >= thePage - 2 || i == pages) {
                        if (i == thePage)
                            h += '<li class="active"><a >' + i + '</a></li>'
                        else
                            h += '<li onclick="getPage(this)"><a >' + i + '</a></li>'
                    }
                }

                if (pages == thePage) {

                    h += '<li class="disabled" ><a >&raquo;</a></li>'
                }
                else {

                    h += '<li onclick="getNextPage()" ><a>&raquo;</a></li>'
                }
                h += '</ul></nav>'
            } else {
                h = '<nav style="text-align:center;display:block"><ul class="pagination">'
                if (thePage == 1)
                    h += '<li class="disabled"><a href="#">&laquo;</a></li>'
                else
                    h += '<li onclick="getPrePage()"><a href="#">&laquo;</a></li>'

                for (var i = 1; i < pages + 1; i++) {
                    if (i == pages && thePage < pages - 3) {
                        h += '<li><a>...</a></li>'
                    }
                    if (i <= thePage + 2 && i >= thePage - 2 || i == pages || i == 1) {
                        if (i == thePage)
                            h += '<li class="active"><a >' + i + '</a></li>'
                        else
                            h += '<li onclick="getPage(this)"><a >' + i + '</a></li>'
                    }
                    if (i == 1 && thePage > 4) {
                        h += '<li><a>...</a></li>'
                    }
                }

                if (pages == thePage) {

                    h += '<li class="disabled" ><a >&raquo;</a></li>'
                }
                else {

                    h += '<li onclick="getNextPage()" ><a>&raquo;</a></li>'
                }
                h += '</ul></nav>'
            }
        }
    }
    return h;
}

function getPageHtml2(pages, thePage) {//一个页面多个表的情况
    if (pages <= 5) {
        h = '<nav style="text-align:center;display:block">'
        + '<ul class="pagination">'
        if (thePage == 1)
            h += '<li class="disabled"><a href="#">&laquo;</a></li>'
        else
            h += '<li onclick="getPrePage2()"><a href="#">&laquo;</a></li>'
        //+ '<li class="active"><a href="#">1</a></li>';

        for (var i = 1; i < pages + 1; i++) {
            if (i == thePage)
                h += '<li class="active"><a >' + i + '</a></li>'
            else
                h += '<li onclick="getPage2(this)"><a >' + i + '</a></li>'
        }
        if (pages == thePage)
            h += '<li class="disabled" ><a >&raquo;</a></li>'
        else
            h += '<li onclick="getNextPage2()" ><a>&raquo;</a></li>'
        h += '</ul>'
        + '</nav>'
    } else {
        if (thePage <= 2) {
            h = '<nav style="text-align:center;display:block"><ul class="pagination">'
            if (thePage == 1)
                h += '<li class="disabled"><a href="#">&laquo;</a></li>'
            else
                h += '<li onclick="getPrePage2()"><a href="#">&laquo;</a></li>'

            for (var i = 1; i < pages + 1; i++) {
                if (i == pages) {
                    h += '<li><a>...</a></li>'
                }
                if (i <= thePage + 2 || i == pages) {
                    if (i == thePage)
                        h += '<li class="active"><a >' + i + '</a></li>'
                    else
                        h += '<li onclick="getPage2(this)"><a >' + i + '</a></li>'
                }
            }

            if (pages == thePage) {

                h += '<li class="disabled" ><a >&raquo;</a></li>'
            }
            else {

                h += '<li onclick="getNextPage2()" ><a>&raquo;</a></li>'
            }
            h += '</ul></nav>'
        } else {
            if (thePage < 4) {
                h = '<nav style="text-align:center;display:block"><ul class="pagination">'
                if (thePage == 1)
                    h += '<li class="disabled"><a href="#">&laquo;</a></li>'
                else
                    h += '<li onclick="getPrePage2()"><a href="#">&laquo;</a></li>'

                for (var i = 1; i < pages + 1; i++) {
                    if (i == pages && thePage < pages - 3) {
                        h += '<li><a>...</a></li>'
                    }
                    if (i <= thePage + 2 && i >= thePage - 2 || i == pages) {
                        if (i == thePage)
                            h += '<li class="active"><a >' + i + '</a></li>'
                        else
                            h += '<li onclick="getPage2(this)"><a >' + i + '</a></li>'
                    }
                }

                if (pages == thePage) {

                    h += '<li class="disabled" ><a >&raquo;</a></li>'
                }
                else {

                    h += '<li onclick="getNextPage2()" ><a>&raquo;</a></li>'
                }
                h += '</ul></nav>'
            } else {
                h = '<nav style="text-align:center;display:block"><ul class="pagination">'
                if (thePage == 1)
                    h += '<li class="disabled"><a href="#">&laquo;</a></li>'
                else
                    h += '<li onclick="getPrePage2()"><a href="#">&laquo;</a></li>'

                for (var i = 1; i < pages + 1; i++) {
                    if (i == pages && thePage < pages - 3) {
                        h += '<li><a>...</a></li>'
                    }
                    if (i <= thePage + 2 && i >= thePage - 2 || i == pages || i == 1) {
                        if (i == thePage)
                            h += '<li class="active"><a >' + i + '</a></li>'
                        else
                            h += '<li onclick="getPage2(this)"><a >' + i + '</a></li>'
                    }
                    if (i == 1 && thePage > 4) {
                        h += '<li><a>...</a></li>'
                    }
                }

                if (pages == thePage) {

                    h += '<li class="disabled" ><a >&raquo;</a></li>'
                }
                else {

                    h += '<li onclick="getNextPage2()" ><a>&raquo;</a></li>'
                }
                h += '</ul></nav>'
            }
        }
    }
    return h;
}

function pushTips(num) {
    $('.tipsNumber').remove();
    $('.iconTop2:first').after('<font class="tipsNumber">'+num+'</font>');
}

function omit(str, length) {
    if (str.length > length)
        str = str.substring(0, length)+'...';
    return str;
}