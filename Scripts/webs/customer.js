var isHasResult = true;
var isClicked = false;

$(document).ready(function () {
    $('.input-search').keydown(function (e) {
        if (e.keyCode == 13)
            $('.btn-search').click();
    })


    $('body').delegate('.td-edit', 'click', function () {
        $('#modal1').modal();
        isClicked = false;
        //var info = new Array();
        var id = $(this).parent().children('td:eq(0)').html();
        var name = $(this).parent().children('td:eq(1)').html();
        var phone = $(this).parent().children('td:eq(2)').html();
        var wechat = $(this).parent().children('td:eq(3)').html();
        var height = $(this).parent().children('td:eq(4)').html();
        var weight = $(this).parent().children('td:eq(5)').html();
        var sex = $(this).parent().children('td:eq(6)').html();
        var age = $(this).parent().children('td:eq(7)').html();
        var labour = $(this).parent().children('td:eq(8)').html();
        var constitution = $(this).parent().children('td:eq(9)').html();
        var score = $(this).parent().children('td:eq(10)').html();

        $('#info-id').text(id)
        $('#info-name').val(name);
        $('#info-phone').val(phone);
        $('#info-wechat').val(wechat);
        $('#info-password').val('')
        $('#info-height').val(height)
        $('#info-weight').val(weight);
        if (sex == '男') 
            $('#info-sex').val('True')
        else if(sex=='女')
            $('#info-sex').val('False')
        else
            $('#info-sex').val('null')
        $('#info-age').val('')
        $('#info-labour').val(labour)
        $('#info-constitution').val(constitution)
        $('#info-score').val(score)
        $('.info').removeClass('changed')
    })
    getTable(jsonStr);

    $('#info-age').datetimepicker({
        format: 'yyyy-mm-dd',
        language: 'ch',
        autoclose: true,
        minView: (0, 'month'),
    });

    $('.btn-search').click(function () {
        changePage(1);
    })

    $('.footer button').click(function () {
        if (!isClicked) {
            var page = $('.pagination li[class="active"] a').text();
            var id, name, phone, wechat, password, height, weight, sex, age, labour, constitution, score;
            id = $('#info-id').text()
            if ($('.info:eq(1)').hasClass('changed')) {
                name = $('#info-name').val();
            }
            phone = $('#info-phone').val();

            if ($('.info:eq(3)').hasClass('changed')) {
                wechat = $('#info-wechat').val();
            }
            if ($('.info:eq(4)').hasClass('changed')) {
                password = $('#info-password').val()
            }
            if ($('.info:eq(5)').hasClass('changed')) {
                height = $('#info-height').val()
            }
            if ($('.info:eq(6)').hasClass('changed')) {
                weight = $('#info-weight').val();
            }
            if ($('.info:eq(7)').hasClass('changed')) {
                sex = $('#info-sex').val()
            }
            if ($('.info:eq(8)').hasClass('changed')) {
                age = $('#info-age').val();
            }
            if ($('.info:eq(9)').hasClass('changed')) {
                labour = $('#info-labour').val()
            }
            if ($('.info:eq(10)').hasClass('changed')) {
                constitution = $('#info-constitution').val()
            }
            if ($('.info:eq(11)').hasClass('changed')) {
                score = $('#info-score').val()
            }
            isClicked = true;
            var data = {
                id: id,
                name: name,
                phone: phone,
                wechat: wechat,
                password: password,
                height: height,
                weight: weight,
                sex: sex,
                age: age,
                labour: labour,
                constitution: constitution,
                score: score,
                method: 'updateCustomer'
            }
            $.ajax({
                type: 'post',
                data: data,
                url: 'Customer.aspx',
                cache: false,
                success: function (data) {
                    alert('更新成功');
                    changePage(page);
                    $('#modal1').modal('hide')
                },
                error: function (err) {
                    alert('cuole');
                }
            })
        }
    })

    $('.info').change(function () {
        $(this).addClass('changed');
    })

    $('body').delegate('.td-sport', 'click', function () {
        var id = $(this).parent().children('.customerId').html();
        location.href = 'CustomerSport.aspx?id='+id;
    })
})


function getTable(data) {
    var json = JSON.parse(data).Table1;
    isHasResult = true;
    if (json == null) {
        //alert('没有结果');
        //clearFoodSelect();
        //clearTagSelect();
        isHasResult = false;
        $('tbody tr').remove();
        $('.table-wrap table').next().remove();
        return;
    }
    var l = json.length;
    $('tbody tr').remove();
    for (var i = 0; i < l; i++) {
        var h = '<tr style="background-color: white;">'
    + '<td class="customerId">'+json[i].id+'</td>'
    + '<td>' + json[i].name + '</td>'
    + '<td>' + json[i].phone + '</td>'
    + '<td>' + json[i].wechat + '</td>'
    + '<td>' + json[i].height + '</td>'
    + '<td>' + json[i].weight + '</td>'
    + '<td>' + json[i].sex + '</td>'
    + '<td>' + json[i].birthday + '</td>'
    + '<td>' + json[i].labourIntensity + '</td>'
    + '<td>' + json[i].constitution + '</td>'
    + '<td>' + json[i].UserScore + '</td>'
    + '<td class="td-sport"><img class="icon-sport" src="/Images/base/icon-sport.png" alt="运动图片"></td><td class="td-edit"><a style="text-decoration: none;"><img class="btn-edit" src="/Images/recipe/icon-edit.svg"><font>编辑</font></a></td></tr>'
        $('tbody').append(h);
    }

    var pages = JSON.parse(data).pages;
    var thePage = JSON.parse(data).thePage;
    //h = '<nav style="text-align:center;display:block">'
    //+ '<ul class="pagination">'
    //if (thePage == 1)
    //    h += '<li class="disabled"><a href="#">&laquo;</a></li>'
    //else
    //    h += '<li onclick="getPrePage()"><a href="#">&laquo;</a></li>'

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
    $('.table-wrap table').next().remove();
    $('.table-wrap table').after(h);
}

function changePage(page) {
    var data = {
        method: 'search',
        thePage: page,
        search: $('.input-search').val(),
    }

    $.ajax({
        type: 'post',
        data: data,
        url: 'Customer.aspx',
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













































//function GetInfo(id)
//{
//    $('#modalInfo').modal();
//}

//function GetSport(id)
//{
//    $('#divModalSport').modal();
//}