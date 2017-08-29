var quesMax = 0;
var isClicked = false;

$(document).ready(function () {
    $('#modalAdd').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#btnAdd').click();
        }
    })
    $('#modalUpdate').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#btnUpdate').click();
        }
    })

    $('body').delegate('tr', 'change', function () {
        $(this).attr('class', 'changed');
    })
    var a = $('.orderNo:first').html();
    quesMax = parseInt(a.split('.')[0]);

    $('#btn-update').click(function () {
        if (!isClicked) {
            var l = $('tr[class="changed"]').length;
            if (l < 1) {
                alert('没有修改')
                return;
            }
            var titles = new Array();
            var constitutions = new Array();
            var sexs = new Array();
            var ids = new Array();
            for (var i = 0; i < l; i++) {
                titles[i] = $('tr[class="changed"]:eq(' + i + ') td:eq(1) input').val();
                if (titles[i].trim() == '') {
                    clearBorder();
                    $('tr[class="changed"]:eq(' + i + ') td:eq(1) input').css('border-color', 'red');
                    alert('输入不能为空！');
                    return;
                }
                constitutions[i] = $('tr[class="changed"]:eq(' + i + ') td:eq(3) select').val();
                sexs[i] = $('tr[class="changed"]:eq(' + i + ') td:eq(4) select').val();
                ids[i] = $('tr[class="changed"]:eq(' + i + ') td:eq(0) input:eq(0)').val();
            }
            var data = {
                method: 'updateQues',
                ids: ids,
                titles: titles,
                constitutions: constitutions,
                sexs: sexs
            }
            isClicked = true;
            $.ajax({
                type: 'post',
                data: data,
                url: 'QuestionPro.aspx',
                cache: false,
                success: function (res) {
                    alert('成功保存！');
                    location.reload();
                },
                error: function (res) {

                }
            })
        }
    })

    $('body').delegate('.ques-delete', 'click', function () {
        var id = $(this).parent().find('td:eq(0) input:eq(0)').val();
        if (id == '0')
            $(this).parent().remove();
        else {
            var r = confirm("确认删除吗？")
            if (r == true) {
                var data = {
                    method: 'deleteQues',
                    id: id
                }
                $.ajax({
                    type: 'post',
                    data: data,
                    url: 'QuestionPro.aspx',
                    cache: false,
                    success: function (res) {
                        alert('已删除！');
                        location.reload();
                    },
                    error: function (res) {
                        alert(2);
                    }
                })
            }
        }
    })

    //$('body').delegate('.thead1 .iCheck-helper', 'click', function () {
    //    if ($('.thead1 div').attr('class') == 'icheckbox_minimal hover checked') {
    //        //var l = $('.ques-select').length;
    //        $('.ques-select .icheckbox_minimal').attr('class', 'icheckbox_minimal checked');
    //    }
    //    else {
    //        $('.ques-select .icheckbox_minimal').attr('class', 'icheckbox_minimal');
    //    }
    //})
    $('.thead1 .iCheck-helper').click(function () {
        if ($('.thead1 div').attr('class') == 'icheckbox_minimal hover checked') {
            //var l = $('.ques-select').length;
            $('.ques-select .icheckbox_minimal').attr('class', 'icheckbox_minimal checked');
        }
        else {
            $('.ques-select .icheckbox_minimal').attr('class', 'icheckbox_minimal');
        }
    })

    $('#batch-delete').click(function () {
        var l = $('tbody div[class="icheckbox_minimal checked"]').length;
        if (l < 1) {
            alert('请先选择！');
            return;
        }
        var ids = new Array();
        var j = 0;
        for (var i = 0; i < l; i++) {
            var id = $('tbody div[class="icheckbox_minimal checked"]:eq(' + i + ')').parent().children('input').val();
            if (id != '0') {
                ids[j] = id;
                j++;
            }
        }
        var r = confirm("确认删除吗？")
        if (r == true) {
            if (ids.length < 1) {
                $('tbody div[class="icheckbox_minimal checked"]').parent().parent().remove();
            } else {
                var data = {
                    method: 'batchDelete',
                    ids:ids
                }
                $.ajax({
                    type: 'post',
                    data: data,
                    url: 'QuestionPro.aspx',
                    cache: false,
                    success: function (res) {
                        $('tbody div[class="icheckbox_minimal checked"]').parent().parent().remove();
                        alert('批量删除成功！');
                    },
                    error: function (res) {

                    }
                })
            }

        }



        //$('tbody div[class="icheckbox_minimal checked"]').parent().parent().remove();
    })
})

function addTemplate() {
    quesMax += 1;

    var h = '<tr style="background-color: white;" class="changed"><td class="ques-select"><input type="hidden"  value="0"/><input type="checkbox"></td><td ><div class="orderNo">' + quesMax + '.</div><input class="ques-title"/><div class="vertical-line1"></div></td><td class="ques-options"><input value="完全不" readonly="readonly" /> <input value="有一点" readonly="readonly" /> <input value="非常" readonly="readonly" /><div class="vertical-line2"></div></td><td class="ques-constitution"><select><option selected="selected">平和质</option><option>气郁质</option><option>阴虚质</option><option>痰湿质</option><option>阳虚质</option> <option>特禀质</option><option>湿热质</option><option>气虚质</option><option>血瘀质</option></select><div class="vertical-line3"></div></td><td class="ques-sex"><select><option selected="selected">通用</option><option>限男性</option><option>限女性</option></select><div class="vertical-line4"></div></td><td class="ques-delete"><a><img class="btn-edit" src="/Images/recipe/icon-delete.svg" /><font>删除</font></a></td></tr>';
    $('tbody tr:first').before(h);
    $('tbody tr:first input').iCheck({
        checkboxClass: 'icheckbox_minimal',
        radioClass: 'iradio_minimal',
        increaseArea: '20%',
    });
}

function clearBorder() {
    $('input').css('border-color','rgb(153,153,153)');
}
































function clearAddTips() {
    var l = $('#divQues div').length - 2;
    for (var i = 0; i < l; i++) {
        var index = i + 1;
        $('#divQues div:eq(' + index + ') input:eq(0)').css('border-color', '#515151');

    }
}

function clearUpdateTips() {
    $('#inputUpdate').css('border-color', '#515151');
}

function modalAdd() {
    clearAddTips();
    $('#modalAdd').modal();
    clearOption();
    $('#divQues div:eq(1) select:eq(0)').val('通用');
}

function modalUpdate(id) {
    clearUpdateTips();
    $('#modalUpdate').modal();
    $('#inputId').val(id);
    var question = $('#font' + id).html();
    $('#inputUpdate').val(question);
    var constitution = $('#font' + id).parent().parent().children().eq(0).children().eq(0).text();
    $('#selectUpdate').val(constitution);
    //alert($('#span' + id).length > 0);
    if ($('#span' + id).length > 0) {
        var sex = $('#span' + id).text();
        $('#selectUpdateSex').val(sex);
    }
    else
        $('#selectUpdateSex').val('通用');
}

function clearOption() {
    var l = $('#divQues div').length;
    for (var i = 3; l > i; i++) {
        $('#divQues div:eq(1)').remove();
    }
    $('#divQues div:eq(1) input:eq(0)').val("");
}


function addQuesNode() {
    //var l = $('divQues div').length;
    //alert(l);
    $('#divQues div').last().prev().after("<div class='row'><h6>问题：</h6><input class='inputQues' type ='text' /><select id='selectSex'><option >通用</option><option >限女性</option><option >限男性</option></select><img src='../Images/delete.png' alt='Alternate Text'  onclick='DeleteQues(this)' style='cursor: pointer;width:20px;height:20px;'/></div>");
}

function deleteQuesNode(node) {
    if ($('#divQues div').length > 3) {
        var a = node;
        a.parentNode.remove(a);
    }
}

function addQues() {
    clearAddTips();
    var l = $('#divQues div').length - 2;
    var ques = new Array(l);
    var sex = new Array(l);
    for (var i = 0; i < l; i++) {
        var index = i + 1;
        ques[i] = $('#divQues div:eq(' + index + ') input:eq(0)').val();
        if (ques[i] == '') {
            $('#divQues div:eq(' + index + ') input:eq(0)').css('border-color', 'red');
            return;
        }
        sex[i] = $('#divQues div:eq(' + index + ') select:eq(0)').val();
        //alert(sex[i]);
    }
    var cons = $('#selectCons option:selected').text();
    var data = {
        question: ques,
        constitution: cons,
        sex: sex
    }

    $.ajax({
        type: "post",
        data: data,
        url: "QuestionPro.aspx?method=add",
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

function deleteQues(id) {
    var r = confirm("确认删除吗？")
    if (r == true) {
        $.ajax({
            type: "post",
            //data: data,
            url: "QuestionPro.aspx?method=delete&id=" + id,
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
}

function updateQues() {
    var id = $('#inputId').val();
    var question = $('#inputUpdate').val();
    if (question == '') {
        $('#inputUpdate').css('border-color', 'red');
        return;
    }
    var constitution = $('#selectUpdate').val();
    var sex = $('#selectUpdateSex').val();
    //alert(id + question + constitution);

    var data = {
        id: id,
        sex: sex,
        question: question,
        constitution: constitution
    }

    $.ajax({
        type: "post",
        data: data,
        url: "QuestionPro.aspx?method=update",
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