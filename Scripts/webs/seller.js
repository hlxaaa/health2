var isAdd = false;
var isHasResult = true;
var isClicked = false;

$(document).ready(function () {
    $('body').delegate('.modal-body', 'keydown', function (e) {
        if (e.keyCode == 13)
            $('.footer button').click();
    })

    $('#inputSearch').keydown(function (e) {
        if (e.keyCode == 13)
            $('.btnSearch').click();
    })

    $('#info-account').change(function () {
        $(this).addClass('changed')
    })

    //getTable(jsonStr);
    changePage(1)

    $('.btn-add').click(function () {
        isAdd = true;
        isClicked = false;
        $('#modal1').modal();
        $('#info-account').val('')
        $('#info-password').val('')
        var data = {
            method: 'getRest',
        }
        $.ajax({
            type: 'post',
            data: data,
            url: 'Seller.aspx',
            cache: false,
            success: function (data) {
                var ids = data.split('|')[0].split(',')
                var rests = data.split('|')[1].split(',')
                var h = ' <select>'
                h += '<option value=""></option>'
                for (var i = 0; i < ids.length; i++) {
                    h += '<option value="' + ids[i] + '">' + rests[i] + '</option>'
                }
                h += '</select>'
                $('.info:eq(0) font').next().remove();
                $('.info:eq(0) font').after(h);

            },
            error: function (err) {
                alert('cuole');
            }
        })
    })

    $('body').delegate('.btn-edit-left', 'click', function () {
        isAdd = false;
        isClicked = false;
        var id = $(this).parent().parent().children('td:eq(1)').html()
        var name = $(this).parent().parent().children('td:eq(2)').html()
        var loginname = $(this).parent().parent().children('td:eq(3)').html()
        var password = $(this).parent().parent().children('td:eq(4)').html()
        $('#sellerId').val(id)
        $('#info-account').val(loginname)
        $('#info-password').val(password)

        $('#modal1').modal();
        $('.info:eq(0) font').next().remove();
        $('.info:eq(0) font').after('<font>' + name + '</font>');
    })

    //更新或增加
    $('.footer button').click(function () {
        if (!isClicked) {
            var page = $('.pagination li[class="active"] a').text();
            var loginname = $('#info-account').val()
            if (loginname == '') {
                alert('用户名不能为空');
                return;
            }
            if (loginname.length < 6) {
                alert('账号长度至少6位')
                return;
            }
            var password = $('#info-password').val()
            if (password.length < 6) {
                alert('密码长度至少6位')
                return;
            }

            if (isAdd) {
                var restId = $('.info:eq(0) select').val();
                if (restId == '') {
                    //alert(restId);
                    alert('请选择一个餐厅');
                    return;
                }
                var data = {
                    method: 'addSeller',
                    loginname: loginname,
                    password: password,
                    restId: restId
                }
                isClicked = true;
                $.ajax({
                    type: 'post',
                    data: data,
                    url: 'Seller.aspx',
                    cache: false,
                    success: function (data) {
                        if (data == 'exist') {
                            alert('已存在该用户名')
                            return;
                        } else {
                            alert('添加成功')
                            changePage(page);
                            $('#modal1').modal('hide')
                        }
                    },
                    error: function (err) {
                        alert('cuole');
                    }
                })
            } else {
                var sellerId = $('#sellerId').val();
                var data;
                if ($('#info-account').hasClass('changed')) {
                    data = {
                        method: 'updateSeller',
                        loginname: loginname,
                        password: password,
                        sellerId: sellerId
                    }
                } else {
                    data = {
                        method: 'updateSeller',
                        password: password,
                        sellerId: sellerId
                    }
                }
                isClicked = true;
                $.ajax({
                    type: 'post',
                    data: data,
                    url: 'Seller.aspx',
                    cache: false,
                    success: function (data) {
                        if (data == 'exist') {
                            alert('已存在该用户名')
                            return;
                        } else {
                            alert('更新成功')
                            changePage(page);
                            $('#modal1').modal('hide')
                        }
                    },
                    error: function (err) {
                        alert('cuole');
                    }
                })
            }
        }
    })

    //删除
    $('body').delegate('.btn-delete', 'click', function () {
        var r = confirm('确认删除吗?')
        if (r) {
            var page = $('.pagination li[class="active"] a').text();
            var id = $(this).parent().parent().children('td:eq(1)').html()
            var data = {
                id: id,
                method: 'deleteSeller'
            }
            $.ajax({
                type: 'post',
                data: data,
                url: 'Seller.aspx',
                cache: false,
                success: function (data) {
                    alert('删除成功')
                    changePage(page);
                },
                error: function (err) {
                    alert('cuole');
                }
            })
        }
    })

    //批量
    $('.batchDelete').click(function () {
        var l = $('tbody div[class="icheckbox_minimal checked"]').length;
        if (l < 1) {
            alert('请先选择！');
            return;
        }
        var ids = new Array();
        for (var i = 0; i < l; i++) {
            var id = $('tbody div[class="icheckbox_minimal checked"]:eq(' + i + ')').parent().next().html();
            ids[i] = id;
        }
        //alert(ids);
        //return;
        var r = confirm("确认删除吗？")
        if (r == true) {

            var data = {
                method: 'batchDelete',
                ids: ids
            }
            $.ajax({
                type: 'post',
                data: data,
                url: 'Seller.aspx',
                cache: false,
                success: function (res) {
                    alert('批量删除成功！');
                    location.reload();
                },
                error: function (res) {

                }
            })
        }


    })
})


function getTable(data) {
    var json = JSON.parse(data).Table1;
    isHasResult = true;
    if (json == null) {
        $('tbody tr').remove();
        $('nav').remove();
        isHasResult = false;
        return;
    }
    var l = json.length;
    $('tbody tr').remove();
    for (var i = 0; i < l; i++) {
        var h = '<tr style="background-color: white;"><td><input type="checkbox"></td>'
    + '<td class="sellerId">' + json[i].id + '</td>'
        var name = omit(json[i].name, 13);
        h += '<td>' + name + '<input type="hidden" class="input-name" value="' + json[i].name + '"/></td>'
        + '<td>' + json[i].loginname + '</td>'
        + '<td>' + json[i].password + '</td>'
        + '<td></td><td></td><td></td><td class="edit"><a class="btn-edit-left" style="text-decoration: none;"><img class="btn-edit" src="/Images/recipe/icon-edit.svg"><font>编辑</font></a><a style="text-decoration: none;" class="btn-delete"><img class="btn-edit" src="/Images/recipe/icon-delete.svg"><font>删除</font></a></td></tr>'
        $('tbody').append(h);
    }

    $('nav').remove();
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
    $('.table-wrap').after(h);
    getInputStyle();
    allSelectToggle();
}

function changePage(page) {
    var search = $('#inputSearch').val();
    var data = {
        thePage: page,
        method: 'search',
        search: search
    }
    $.ajax({
        type: 'post',
        data: data,
        url: 'Seller.aspx',
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

function allSelectToggle() {
    $('.thead1 .iCheck-helper').click(function () {
        if ($('.thead1 div').attr('class') == 'icheckbox_minimal hover checked') {
            //var l = $('.ques-select').length;
            $('tr td .icheckbox_minimal').attr('class', 'icheckbox_minimal checked');
        }
        else {
            $('tr td .icheckbox_minimal').attr('class', 'icheckbox_minimal');
        }
    })
}




















function modalAdd() {
    clearTips();
    isAdd = true;
    $('#modalEdit').modal();
    $('#option1').text('');
    $('#option1').prop('value', '0');
    $('#option1').prop('selected', 'selected');
    $('#inputAccount').val('');
    $('#inputPassword').val('');

    $('#btnAdd').css('display', '');
    $('#btnUpdate').css('display', 'none');
}

function modalUpdate(id, restId) {
    clearTips();
    isAdd = false;
    $('#modalEdit').modal();
    $('#inputId').val(id);
    var name = $('#tr' + id + ' td:eq(' + 1 + ')').text();
    var account = $('#tr' + id + ' td:eq(' + 2 + ')').text();
    var password = $('#tr' + id + ' td:eq(' + 3 + ')').text();

    $('#option1').text(name);
    $('#option1').prop('selected', 'selected');
    $('#option1').prop('value', restId);
    //$('#selectRest option:first').prop('selected', 'selected');
    $('#inputAccount').val(account);
    $('#inputPassword').val(password);

    $('#btnAdd').css('display', 'none');
    $('#btnUpdate').css('display', '');
}

function clearTips() {
    $('#selectRest').css('border-color', '#515151');
    $('#inputAccount').css('border-color', '#515151');
    $('#inputPassword').css('border-color', '#515151');
}

function addSeller() {
    clearTips();
    //var seller = $('#inputSeller').val();
    var restId = $('#selectRest').val();
    if (restId == '0') {
        $('#selectRest').css('border-color', 'red');
        return;
    }
    var account = $('#inputAccount').val();
    if (account == '') {
        $('#inputAccount').css('border-color', 'red');
        return;
    }
    var password = $('#inputPassword').val();
    if (password == '') {
        $('#inputPassword').css('border-color', 'red');
        return;
    }
    if (password.length < 6) {
        alert('长度为6-12之间');
        return;
    }


    //alert(seller + account + password + restId);
    var data = {
        method: 'addSeller',
        //seller: seller,
        account: account,
        password: password,
        restId: restId
    }
    $.ajax({
        type: 'post',
        data: data,
        url: 'Seller.aspx',
        cache: false,
        success: function (data) {
            location.reload();
        },
        error: function (err) {
            alert('出问题了');
        }
    })
}

function updateSeller() {
    clearTips();
    var id = $('#inputId').val();
    var restId = $('#selectRest').val();
    var account = $('#inputAccount').val();
    if (account == '') {
        $('#inputAccount').css('border-color', 'red');
        return;
    }
    var password = $('#inputPassword').val();
    if (password == '') {
        $('#inputPassword').css('border-color', 'red');
        return;
    }
    if (password.length < 6) {
        alert('长度为6-12之间');
        return;
    }

    var data = {
        id: id,
        method: 'updateSeller',
        restId: restId,
        account: account,
        password: password
    }
    $.ajax({
        type: 'post',
        data: data,
        url: 'Seller.aspx',
        cache: false,
        success: function (data) {
            location.reload();
        },
        error: function (err) {
            alert('出问题了');
        }
    })
}

function deleteSeller(id) {
    var data = {
        method: 'deleteSeller',
        id: id
    }
    var r = confirm('确认删除吗？');
    if (r == true) {
        $.ajax({
            type: 'post',
            data: data,
            url: 'Seller.aspx',
            cache: false,
            success: function (data) {
                location.reload();
            },
            error: function (err) {
                alert('出问题了');
            }
        })
    }
}