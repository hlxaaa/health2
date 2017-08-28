var isAdd = false;
var jsonStr;
//var isHasResult = true;
var isClicked = false;

$(document).ready(function () {

    //回车触发
    $('.row1-mid').keydown(function (e) {
        if (e.keyCode == 13)
            $('.btnSearch').click();
    })

    $('.modal-body').keydown(function (e) {
        if (e.keyCode == 13)
            $('.modal-body button').click();
    })

    //getTable(jsonStr);
    changePage(1);

    //$('input').iCheck({
    //    checkboxClass: 'icheckbox_minimal',
    //    radioClass: 'iradio_minimal',
    //    increaseArea: '20%' // optional
    //});
    //$('a').css('text-decoration', 'none');
    //$('.allselect .iCheck-helper').click(function () {
    //    allselectToggle();
    //})
    getInputStyle();

    $('.btn-add').click(function () {
        isAdd = true;
        isClicked = false;
        $('#modal1').modal();
        $('.modal-body input:eq(0)').val('');
        clearCss();
    })

    $('body').delegate('#aRightBorder', 'click', function () {
        isAdd = false;
        isClicked = false;
        var id = $(this).prev().val();
        var name = $(this).parent().prev().children('font').text();
        $('#foodId').val(id);
        $('.modal-body input:eq(0)').val(name);
        $('#modal1').modal();
        clearCss();
        //$('.modal-body input:eq(0)').focus();
    })

    $('body').delegate('.btn-delete', 'click', function () {
        var r = confirm('确认删除吗？');
        if (r == true) {
            var id = $(this).prevAll('input').val();
            var search = $('#inputSearch').val();
            var page = $('.pagination li[class="active"] a').text();
            var data = {
                method: 'deleteFood',
                thePage: page,
                search: search,
                id: id
            }
            $.ajax({
                type: 'post',
                data: data,
                url: 'Food.aspx',
                cache: false,
                success: function (res) {
                    //location.reload();
                    getTable(res);
                },
                error: function (res) {

                }
            })
        }
    })

    //add或者update
    $('.btn-primary').click(function () {
        if (!isClicked) {
            var name = $('.modal-body input:eq(0)').val().trim();
            if (name == '') {
                $('.modal-body input').css('border-color', 'red');
                return;
            }
            var search = $('#inputSearch').val();
            var page = $('.pagination li[class="active"] a').text();


            //return;
            if (isAdd) {
                var data = {
                    method: 'addFood',
                    search: search,
                    thePage: page,
                    name: name
                }
                isClicked = true;
                $.ajax({
                    type: 'post',
                    data: data,
                    url: 'Food.aspx',
                    cache: false,
                    success: function (res) {

                        if (res == 'exist')
                            alert('已存在该菜品！');
                        else {
                            getTable(res);
                          
                            $('#modal1').modal('hide');
                           
                        }
                        //location.reload();
                    },
                    error: function (res) {

                    }
                })
            }
            else {
                var id = $('#foodId').val();
                var data = {
                    id: id,
                    method: 'updateFood',
                    search: search,
                    thePage: page,
                    name: name
                }
                isClicked = true;
                $.ajax({
                    type: 'post',
                    data: data,
                    url: 'Food.aspx',
                    cache: false,
                    success: function (res) {
                        if (res == 'exist')
                            alert('已存在该菜品！');
                        else
                            getTable(res);
                       
                        $('#modal1').modal('hide');
                       
                        //location.reload();
                    },
                    error: function (res) {

                    }
                })
            }
        }
    })

    //全选js

    $('.batchDelete').click(function () {
        var l = $('.div-name div[class="icheckbox_minimal checked"]').length;
        if (l < 1) {
            alert('请先选择');
            return;
        }
        var r = confirm('确定删除这些吗?');
        if (r == true) {
            //var l = $('.div-name div[class="icheckbox_minimal checked"]').length;
            var ids = new Array();
            for (var i = 0; i < l; i++) {
                ids[i] = $('.div-name div[class="icheckbox_minimal checked"]:eq(' + i + ') ').parent().next().children('input').val();
            }
            var data = {
                method: 'batchDelete',
                ids: ids
            }
            $.ajax({
                type: 'post',
                data: data,
                url: 'Food.aspx',
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
})

function clearCss() {
    $('.modal-body input').css('border', '1px solid rgb(153,153,153)');
}

function allselectToggle() {
    if ($('.allselect div').attr('class') == 'icheckbox_minimal hover checked') {
        //var l = $('.ques-select').length;
        $('.div-food .icheckbox_minimal').attr('class', 'icheckbox_minimal checked');
    }
    else {
        $('.div-food .icheckbox_minimal').attr('class', 'icheckbox_minimal');
    }
}

function getTable(jsonStr) {
    var json = JSON.parse(jsonStr).Table1;
    //isHasResult = true;
    if (json == null) {
        //isHasResult = false;
        //alert('没有结果');
        $('#divMain3 .div-food').remove();
        $('#divMain3').nextAll().remove();
        return;
    }
    $('#divMain3 .div-food').remove();

    for (var i = 0; i < json.length; i++) {
        var name = json[i].name

        var h = '<div class="div-food">'
+ '<div class="name-wrap">'
+ '<div class="div-name">'
+ '<input type="checkbox">'
+ '<font>' + name + '</font>'
+ '</div>'
+ '<div class="div-edit">'
+ '<input type="hidden"  value="' + json[i].id + '"/>'
+ '<a id="aRightBorder" style="text-decoration: none;">'
+ '<img class="btn-edit" src="/Images/recipe/icon-edit.svg"><font>编辑</font></a>'
+ '<div class="partline3">'
+ '</div>'
+ '<a style="text-decoration: none;" class="btn-delete">'
+ '<img class="btn-edit" src="/Images/recipe/icon-delete.svg"><font>删除</font>'
+ '</a>'
+ '</div>'
+ ' <div class="fc"></div>'
+ '</div>'
+ '</div>'
        $('#divMain3 .partline2').before(h);

    }

    var pages = JSON.parse(jsonStr).pages;
    var thePage = JSON.parse(jsonStr).thePage;
    //    var p = '<nav style="text-align: center; display: block">'
    //+ '<ul class="pagination">'
    //    if(thePage==1)
    //        p += '<li class="disabled"><a style="text-decoration: none;">&laquo;</a></li>'
    //    else
    //        p += '<li class="" onclick="getPrePage()"><a style="text-decoration: none;">&laquo;</a></li>'
    //    for (var i = 1; i < pages + 1; i++) {
    //        if (i == thePage)
    //            p += '<li class="active"><a>' + i + '</a></li>'
    //        else
    //            p += '<li onclick="getPage(this)"><a >' + i + '</a></li>'
    //        if (i == pages && i == thePage)
    //            p += '<li class="disabled" ><a >&raquo;</a></li>'
    //        else if (i == pages)
    //            p += '<li onclick="getNextPage()" ><a>&raquo;</a></li>'
    //    }


    //    p+= '</ul>'
    //    + '</nav>'
    var h = getPageHtml(pages, thePage);

    $('#divMain3').nextAll().remove();
    $('#divMain3').after(h);

    getInputStyle();

}

function getPage(node) {
    var thePage = node.innerText;
    //alert(thePage);
    changePage(thePage);
}

function changePage(page) {
    var search = $('#inputSearch').val();

    var data = {
        method: 'search',
        thePage: page,
        search: search
    }
    $.ajax({
        type: 'post',
        data: data,
        //dataType: 'html',
        url: 'Food.aspx',
        cache: false,
        success: function (data) {
            getTable(data);
            //if (!isHasResult)
            //    alert('没有结果')
        },
        error: function (err) {
            alert('cuole');
        }
    })
}

