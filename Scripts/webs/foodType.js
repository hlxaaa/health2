var isAdd = false;
//var isHasResult = true;
var isClicked = false;


$(document).ready(function () {
    allSelect();

    //回车触发
    $('#inputSearch').keydown(function (e) {
        if (e.keyCode == 13)
            $('.btnSearch').click();
    })
    $('#typename').keydown(function (e) {
        if (e.keyCode == 13)
            $('.modal-body .btn-primary').click();
    })


    //批量删除
    $('body').delegate('.batchDelete', 'click', function () {
        if ($('tr td div[class="icheckbox_minimal checked"]').length < 1) {
            alert('请先选择');
            return;
        }
        var r = confirm('确认删除吗?')
        if (r == true) {
            var ids = new Array();
            $('tr td div').each(function () {
                if ($(this).attr('class') == 'icheckbox_minimal checked')
                    ids.push($(this).parent().next().next().children('input').val());
            })
            var data = {
                method: 'batchDelete',
                ids:ids
            }
            $.ajax({
                type: 'post',
                data: data,
                url: 'FoodType.aspx',
                cache: false,
                success: function (res) {
                    alert('删除成功');
                    location.reload();
                    //getTable(res);
                },
                error: function (res) {

                }
            })
        }
    })

    //getTable(jsonStr);
    changePage(1);

    $('.btn-add').click(function () {
        $('#modal1').modal();
        $('#typename').val('')
        isAdd = true;
        isClicked = false;
    })

    $('body').delegate('.btn-edit', 'click', function () {
        isAdd = false;
        isClicked = false;
        $('#modal1').modal();
        var name = $(this).parent().prev().html();
        var id = $(this).next().next().val();
        $('#typename').val(name);
        $('#typeId').val(id);
    })

    //增加或更新
    $('.btn-primary').click(function () {
        if (!isClicked) {
            var name = $('#typename').val().trim();
            if (name == "") {
                alert('不能为空');
                return;
            }
            if (isAdd) {
                var data = {
                    method: 'addType',
                    name: name
                }
                isClicked = true;
                $.ajax({
                    type: 'post',
                    data: data,
                    url: 'FoodType.aspx',
                    cache: false,
                    success: function (res) {
                        if (res == 'exist') {
                            alert('已存在该分类')
                        } else {
                            alert('添加成功');
                            location.reload();
                        }
                        //getTable(res);
                    },
                    error: function (res) {

                    }
                })
            } else {
                var id = $('#typeId').val();
                var data = {
                    method: 'updateType',
                    name: name,
                    id: id
                }
                isClicked = true;
                $.ajax({
                    type: 'post',
                    data: data,
                    url: 'FoodType.aspx',
                    cache: false,
                    success: function (res) {
                        if (res == 'exist') {
                            alert('已存在该分类')
                        } else {
                            alert('更新成功');
                            location.reload();
                        }
                        //getTable(res);
                    },
                    error: function (res) {

                    }
                })
            }
        }
    })

    $('body').delegate('.btn-delete', 'click', function () {
        var id = $(this).next().val();
        var r = confirm('确认删除吗？');
        if (r == true) {
            var data = {
                id: id,
                method:'deleteType'
            }
            $.ajax({
                type: 'post',
                data: data,
                url: 'FoodType.aspx',
                cache: false,
                success: function (res) {
                    alert('删除成功');
                    location.reload();
                    //getTable(res);
                },
                error: function (res) {

                }
            })
        }
    })
})

function allSelect() {
    $('.thead1 .iCheck-helper').click(function () {
        if ($('.thead1 div').attr('class') == 'icheckbox_minimal hover checked') {
            //var l = $('.ques-select').length;
            $('td .icheckbox_minimal').attr('class', 'icheckbox_minimal checked');
        }
        else {
            $('td .icheckbox_minimal').attr('class', 'icheckbox_minimal');
        }
    })
}

function getTable(jsonStr) {
    var json = JSON.parse(jsonStr).Table1;
    //isHasResult = true;
    if (json == null) {
        //isHasResult = false;
        //alert('没有结果');
        $('#divMain3 tbody tr').remove();
        return;
    }
    var h = '<table class="table table-striped table-hover"><thead class="thead1"><tr><th width="1%"><input type="checkbox"></th><th width="6%">菜品分类</th><th width="6%">操作</th><th width="6%"></th><th width="6%"></th><th width="6%"></th><th width="6%"></th></tr></thead><tbody>'
    for (var i = 0; i < json.length; i++) {
        var name = json[i].name
        h+='<tr style="background-color: white;"><td><input type="checkbox"></td>'
        +'<td>' + name + '</td>'
        +'<td class="td-edit"><a class="btn-edit" style="text-decoration: none;"><img class="btn-edit" src="/Images/recipe/icon-edit.svg"><font>编辑</font></a><a style="text-decoration: none;" class="btn-delete"><img class="btn-edit" src="/Images/recipe/icon-delete.svg"><font>删除</font></a><input type="hidden" class="typeId" value="'+json[i].id+'"></td><td></td><td></td><td></td><td></td></tr>'
    }
    h += '</tbody></table>';

    
    //var pages = 2;
    //var thePage = 1;

 
    $('#divMain3 table').remove();
    $('#divMain3 nav').remove();
    $('#divMain3').append(h);

    $('input').iCheck({
        checkboxClass: 'icheckbox_minimal',
        radioClass: 'iradio_minimal',
        increaseArea: '20%' // optional
    });
    $('a').css('text-decoration', 'none');

   
    allSelect();
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
        search: search,
    }
    $.ajax({
        type: 'post',
        data: data,
        url: 'FoodType.aspx',
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














function tipsClear() {
    $('#inputType').css('border-color', '#515151');
}

function addFoodType() {
    var name = $('#inputType').val();
    if (name == '') {
        $('#inputType').css('border-color', 'red');
        return;
    }
    $.ajax({
        type: "post",
        url: "FoodType.aspx?method=add&name=" + name,
        cache: false,
        success: function (data) {
            if (data == "exist")
                alert("已存在该分类")
            else
                location.reload();
        },
        error: function (err) {
            alert("出问题了");
        }
    });
}

function modalAdd() {
    tipsClear();
    document.getElementById("btnAdd").style.display = '';
    document.getElementById("btnUpdate").style.display = 'none';
    $('#modalAdd').modal();
    isAdd = true;
    $('#inputType').val('');
}

function deleteFoodType(id) {
    //var a = id;
    var r = confirm("确认删除吗？")
    if (r == true) {
        var div = document.getElementById("div" + id);
        div.parentNode.removeChild(div);
        $.ajax({
            type: "post",
            url: "FoodType.aspx?method=delete&id=" + id,
            cache: false,
            success: function (data) {
                //alert("ok");
            },
            error: function (err) {
                alert("出问题了");
            }
        });
    }
    else {
        alert("");
    }
}

function modalUpdate(id) {
    tipsClear();
    document.getElementById("btnAdd").style.display = 'none';
    document.getElementById("btnUpdate").style.display = '';
    $('#inputType').val(document.getElementById("pName" + id).innerText);
    $('#modalAdd').modal();
    $('#inputId').val(id);
    isAdd = false;
}
function updateFoodType() {

    var name = $('#inputType').val();
    if (name == '') {
        $('#inputType').css('border-color', 'red');
        return;
    }
    var id = $('#inputId').val();
    $.ajax({
        type: "post",
        url: "FoodType.aspx?method=update&name=" + name + "&id=" + id,
        cache: false,
        success: function (data) {
            if (data == "exist")
                alert("已存在该分类")
            else
                location.reload();
        },
        error: function (err) {
            alert("出问题了");
        }
    });
}