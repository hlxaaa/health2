var isAdd = false;
var jsonStr;
var isHasResult = true;

$(document).ready(function () {

    getTable(jsonStr);

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
        $('#modal1').modal();
        $('.modal-body input:eq(0)').val('');
        clearCss();
    })

    $('body').delegate('#aRightBorder', 'click', function () {
        isAdd = false;
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
                search:search,
                id:id
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
        var name = $('.modal-body input:eq(0)').val().trim();
        if (name == '') {
            $('.modal-body input').css('border-color', 'red');
            return;
        }
        var search = $('#inputSearch').val();
        var page = $('.pagination li[class="active"] a').text();

        $('#modal1').modal('hide');
        //return;
        if (isAdd) {
            var data = {
                method: 'addFood',
                search: search,
                thePage:page,
                name: name
            }
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
                id:id,
                method: 'updateFood',
                search: search,
                thePage: page,
                name: name
            }
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
                        //location.reload();
                },
                error: function (res) {

                }
            })
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
    $('.modal-body input').css('border','1px solid rgb(153,153,153)');
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
        isHasResult = true;
        if (json == null) {
            isHasResult = false;
            //alert('没有结果');
            $('#divMain3 .div-food').remove();
            $('#divMain3').nextAll().remove();
            return;
        }
        $('#divMain3 .div-food').remove();

        for (var i = 0; i < json.length; i++) {
            var h = '<div class="div-food">'
    + '<div class="name-wrap">'
    + '<div class="div-name">'
    + '<input type="checkbox">'
    + '<font>' + json[i].name + '</font>'
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
                if(!isHasResult)
                    alert('没有结果')
            },
            error: function (err) {
                alert('cuole');
            }
        })
    }
















    function tipsClear() {
        $('#inputFood').css('border-color', '#515151');
    }


    function addFood() {
        var name = $('#inputFood').val();
        if (name == "") {
            $('#inputFood').css('border-color', 'red');
            return;
        }
        $.ajax({
            type: "post",
            url: "Food.aspx?method=addFood&name=" + name,
            cache: false,
            success: function (data) {
                if (data == "exist")
                    alert("已存在该菜品")
                else
                    location.reload();
            },
            error: function (err) {
                alert("错误");
            }
        });
    }

    function deleteFood(id) {
        //var a = id;
        var r = confirm("确认删除吗？")
        if (r == true) {
            var div = document.getElementById("div" + id);
            div.parentNode.removeChild(div);
            $.ajax({
                type: "post",
                url: "Food.aspx?method=delete&id=" + id,
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
    //function funChange()
    //{
    //    $.ajax({
    //        type: "post",
    //        url: "Restaurant.aspx?method=Restaurant",
    //        cache: false,
    //        success: function (data) {
    //            alert("ok");
    //            document.getElementById("body1").innerHTML = URL("Restaurant.aspx");
    //        },
    //        error: function (err) {
    //            alert("出问题了");
    //        }
    //    });
    //}
    //function food()
    //{
    //    document.getElementById("container").innerHTML = 
    //}

    function updateFood() {
        var name = $('#inputFood').val();
        if (name == '') {
            $('#inputFood').css('border-color', 'red');
            return;
        }
        var id = $('#inputId').val();
        $.ajax({
            type: "post",
            //data:data,
            url: "Food.aspx?method=update&name=" + name + "&id=" + id,
            cache: false,
            //async: true,
            success: function (data) {
                if (data == "exist")
                    alert("已存在该菜品")
                else
                    location.reload();
            },
            error: function (err) {
                alert("出问题了");
            }
        });
    }