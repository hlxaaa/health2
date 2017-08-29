var isAdd = false;
var isClicked = false;
$(document).ready(function () {
    $('body').delegate('.ques', 'keydown', function (e) {
        if (e.keyCode == 13)
            $(this).find('#aRightBorder').click();
    })
     
    

    $('#modalAdd').keydown(function (e) {
        if (e.keyCode == 13) {
            if (isAdd)
                $('#btnAdd').click();
            else
                $('#btnUpdate').click();
        }
    })

    var isClickUpdate = false;
    $('#btnUpdate').click(function () {
        if (isClickUpdate == false) {
            tipsClear();
            var ques = $('#inputQues').val();
            if (ques == '') {
                $('#inputQues').css('border-color', 'red');
                return;
            }
            var id = $('#inputId').val();
            var l = $('#divOptions div').length - 2;
            //alert(l);
            var options = new Array(l);
            var constitution = new Array(l);
            for (var i = 0; i < l; i++) {
                var index = i + 1;
                options[i] = $('#divOptions div:eq(' + index + ') input:eq(0)').val();
                if (options[i] == '') {
                    $('#divOptions div:eq(' + index + ') input:eq(0)').css('border-color', 'red');
                    return;
                }
                constitution[i] = $('#divOptions div:eq(' + index + ') select:eq(0) option:selected').text();
                //alert(1 + constitution[i]);
                //alert(constitution);
            }
            //var a = $('#divOptions div:eq(' +2+') input:eq(0)').val();
            //alert(ques+options+constitution);
            //alert(options);
            isClickUpdate = true;
            var data = {
                method: 'update',
                id: id,
                question: ques,
                options: options,
                constitution: constitution
            }

            $('#btnUpdate').attr('disable', true);

            $.ajax({
                type: "post",
                data: data,
                url: "QuestionEasy.aspx",
                cache: false,
                //async: false,
                success: function (data) {
                    //alert("ok");
                    location.reload();
                },
                error: function (err) {

                    alert('出问题了');
                }
            });
        }
    })

    var isClickAdd = false;
    $('#btnAdd').click(function () {

        if (isClickAdd == false) {
            tipsClear();
            var ques = $('#inputQues').val();
            if (ques == '') {
                $('#inputQues').css('border-color', 'red');
                return;
            }
            var l = $('#divOptions div').length - 2;
            //alert(l);
            var options = new Array(l);
            var constitution = new Array(l);
            for (var i = 0; i < l; i++) {
                var index = i + 1;
                options[i] = $('#divOptions div:eq(' + index + ') input:eq(0)').val();
                if (options[i] == '') {
                    $('#divOptions div:eq(' + index + ') input:eq(0)').css('border-color', 'red');
                    return;
                }
                constitution[i] = $('#divOptions div:eq(' + index + ') select:eq(0) option:selected').text();
                //alert(1 + constitution[i]);
                //alert(constitution);
            }
            //var a = $('#divOptions div:eq(' +2+') input:eq(0)').val();
            //alert(ques+options+constitution);

            isClickAdd = true;

            var data = {
                question: ques,
                options: options,
                constitution: constitution
            }
            //$('#btnAdd').attr('disabled', true);

            $.ajax({
                type: "post",
                data: data,
                url: "QuestionEasy.aspx?method=add",
                cache: false,
                success: function (data) {
                    //alert("ok");
                    location.reload();
                },
                error: function (err) {
                    alert('出问题了');
                }
            });
        }
    })

    $('body').delegate('#icon-add', 'click', function () {
        var h = '<div class="col-md-5 ">'
                           + '<font>选项：</font><input />'
                           + ' <font>体质：</font> <select>'
                           + '<option>平和质</option><option>气郁质</option><option>阴虚质</option><option>痰湿质</option><option>阳虚质</option><option>特禀质</option><option>湿热质</option><option>气虚质</option><option>血瘀质</option>'
                           + '</select> <img src="../Images/base/icon-trash.png" id="icon-trash"/>'
                     + '</div>';
        $(this).parent().parent().children('.col-md-5:last').after(h);
    })

    $('body').delegate('#icon-trash', 'click', function () {
        if ($(this).parent().siblings().length > 1)
            $(this).parent().remove();
    })
   

})

function clearBorder() {
    $('input').css('border-color', '#8c83a3');
}

function addTemplate() {
    h = '<div class="ques"><div class="ques-title"><div class="ques-title-content"><font>题目：</font><input  ></div><div class="ques-edit"><a id="aRightBorder" class="btn-noborder" onclick="addQues()" style="text-decoration: none;"><img class="btn-edit" src="/Images/base/icon-save.svg"><font>保存</font></a></div></div><hr><div class="ques-options"><div class="col-md-5 "><font>选项：</font><input > <font>体质：</font> <select><option selected="selected">平和质</option><option>气郁质</option><option>阴虚质</option><option>痰湿质</option><option>阳虚质</option> <option>特禀质</option><option>湿热质</option><option>气虚质</option><option>血瘀质</option></select> <img src="../Images/base/icon-trash.png" id="icon-trash"></div><div class="col-md-1 "><img src="/Images/base/icon-add.png" id="icon-add"></div></div><div class="col-md-12" style="height:30px"></div><div style="clear:both"></div>  </div>'
    $('.main2 .ques:first').before(h);
}

function addQues() {
    if (!isClicked) {
        var title = $('.ques-title-content:eq(0) input').val();
        if (title.trim() == '') {
            clearBorder();
            $('.ques-title-content:eq(0) input').css('border-color', 'red');
            alert('题目不能为空！');
            return;
        }
        var options = new Array();
        var constitutions = new Array();
        var l = $('.ques-options:eq(0) .col-md-5').length;
        for (var i = 0; i < l; i++) {
            options[i] = $('.ques-options:eq(0) .col-md-5:eq(' + i + ') input').val();
            if (options[i].trim() == '') {
                clearBorder();
                $('.ques-options:eq(0) .col-md-5:eq(' + i + ') input').css('border-color', 'red');
                alert('选项不能为空！');
                return;
            }
            constitutions[i] = $('.ques-options:eq(0) .col-md-5:eq(' + i + ') select').val();
        }

        isClicked = true;
        var data = {
            method: 'addQues',
            title: title,
            options: options,
            constitutions: constitutions
        }
        $.ajax({
            type: 'post',
            data: data,
            url: 'QuestionEasy.aspx',
            cache: false,
            success: function (res) {
                alert('添加成功');
                location.reload();
            },
            error: function (res) {
                alert(2);
            }
        })
    }
}

function updateQues(id) {
    if (!isClicked) {
        var title = $('#ques' + id + ' .ques-title-content input').val();
        if (title.trim() == '') {
            clearBorder();
            $('#ques' + id + ' .ques-title-content input').css('border-color', 'red');
            alert('题目不能为空！');
            return;
        }
        var options = new Array();
        var constitutions = new Array();
        var l = $('#ques' + id + ' .ques-options .col-md-5').length;
        for (var i = 0; i < l; i++) {
            options[i] = $('#ques' + id + ' .ques-options .col-md-5:eq(' + i + ') input').val();
            if (options[i].trim() == '') {
                clearBorder();
                $('#ques' + id + ' .ques-options .col-md-5:eq(' + i + ') input').css('border-color', 'red');
                alert('选项不能为空！');
                return;
            }
            constitutions[i] = $('#ques' + id + ' .ques-options .col-md-5:eq(' + i + ') select').val();
        }

        isClicked = true;
        var data = {
            id: id,
            method: 'updateQues',
            title: title,
            options: options,
            constitutions: constitutions
        }
        $.ajax({
            type: 'post',
            data: data,
            url: 'QuestionEasy.aspx',
            cache: false,
            success: function (res) {
                alert('保存成功')
                location.reload();
            },
            error: function (res) {
                alert(2);
            }
        })
    }
}


function deleteQues(id) {
    var r = confirm("确认删除吗？")
    if (r == true) {
        //var div = document.getElementById("div" + id);
        //div.parentNode.removeChild(div);
        var data = {
            method: 'deleteQues',
            id:id
        }
        $.ajax({
            type: "post",
            data:data,
            url: "QuestionEasy.aspx",
            cache: false,
            success: function (data) {
                //alert("ok");
                location.reload();
            },
            error: function (err) {
                alert("出问题了");
            }
        });
    }
}






















function tipsClear() {
    $('#inputQues').css('border-color', '#515151');
    var l = $('#divOptions div').length - 2;
    for (var i = 0; i < l; i++) {
        var index = i + 1;
        $('#divOptions div:eq(' + index + ') input:eq(0)').css('border-color', '#515151');
    }
}

function modalAdd() {
    tipsClear();
    isAdd = true;
    $('#modalAdd').modal();
    document.getElementById("btnAdd").style.display = '';
    document.getElementById("btnUpdate").style.display = 'none';
    $('#inputQues').val("");
    optionClear();
}

function optionClear() {
    var l = $('#divOptions div').length;
    for (var i = 3; l > i; i++) {
        $('#divOptions div:eq(1)').remove();
    }
    $('#divOptions div:eq(1) input:eq(0)').val("");
    //$('#divOptions div:eq(1) select:eq(0) option:selected').text("");
}

function modalUpdate(id) {
    $('#btnUpdate').attr('disable', false);
    tipsClear();
    isAdd = false;
    $('#inputId').val(id);
    $('#modalAdd').modal();
    document.getElementById("btnAdd").style.display = 'none';
    document.getElementById("btnUpdate").style.display = '';
    $('#inputQues').val($('#panel' + id + ' div:eq(0) h6:eq(1)').html());

    optionClear();

    var l = $('#panel' + id + ' .panel-body').length;
    var options = new Array(l);
    var constitution = new Array(l);
    for (var i = 0; i < l; i++) {
        if (i != 0) {
            addOption();
        }

        var op = $('#panel' + id + ' .panel-body:eq(' + i + ') font').html();
        var con = $('#panel' + id + ' .panel-body:eq(' + i + ') span').html();
        var index = i + 1;
        $('#divOptions div:eq(' + index + ') input:eq(0)').val(op);
        $('#divOptions div:eq(' + index + ') select').val(con);
        //alert(options[i]);
    }
    //var data = {
    //    id: id,
    //    options: options,
    //    constitution:constitution
    //}

    //$.ajax({
    //    type: "post",
    //    data: data,
    //    url: "QuestionEasy.aspx?method=update",
    //    cache: false,
    //    success: function (data) {
    //        //alert("ok");
    //        location.reload();
    //    },
    //    error: function (err) {
    //        alert('出问题了');
    //    }
    //});
}



function deleteOption(node) {
    if ($('#divOptions div').length > 3) {
        var a = node;
        a.parentNode.remove(a);
    }
}








