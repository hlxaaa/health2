var isAdd = false;
$(document).ready(function () {
   

    getInputStyle();

 

    //$('.btnSearch').click(function () {
    //    changePage(1);
    //})

    $('.btn-addTemplate').click(function () {
        isAdd = true;
        $('#modal1').modal();
        $('.tag-name input').val('');
        for (var i = 0; i < 9; i++) {
            $('.tag-name').nextAll('div:eq(' + i + ')').children('select').val('0');
        }
        clearCss();
    })

    $('body').delegate('.td-edit #aRightBorder', 'click', function () {
        isAdd = false;
        var id = $(this).next().next('input').val();
        $('#tagId').val(id);
        $('#modal1').modal();
        var name = $(this).parent().parent().children('td:eq(1)').text().toString().trim();
        $('.tag-name input').val(name);
        for (var i = 0; i < 9; i++) {
            var index = i + 2;
            var value =parseInt($(this).parent().parent().children('td:eq(' + index + ')').text());
            $('.tag-name').nextAll('div:eq('+i+')').children('select').val(value);
        }
        clearCss();
    })
    
    $('.btn-delete').click(function () {
        var r = confirm('确认删除吗？');
        if (r = true) {
            var id = $(this).next('input').val();
            var data = {
                method: 'deleteTag',
                id:id
            }
            $.ajax({
                type: 'post',
                data: data,
                url: 'Tag.aspx',
                cache: false,
                success: function (res) {
                    location.reload();
                },
                error: function (res) {

                }
            })
        }
    })

    $('.btn-primary').click(function () {
        var name = $('.tag-name input').val().trim();
        if (name == '') {
            $('.tag-name input').css('border', '1px solid red');
            return;
        }
        var constitutions = new Array();
        for (var i = 0; i < 9; i++) {
            constitutions[i] = $('.tag-name').nextAll('div:eq(' + i + ')').children('select').val();
        }
        if (isAdd) {
            var data = {
                method: 'addTag',
                name: name,
                constitutions:constitutions
            }
            $.ajax({
                type: 'post',
                data: data,
                url: 'Tag.aspx',
                cache: false,
                success: function (res) {
                    if (res == 'exist')
                        alert('已存在该标签！');
                    else {
                        alert('添加成功');
                        location.reload();
                    }
                },
                error: function (res) {
                    alert(2);
                }
            })
        }
        else {
            id = $('#tagId').val();
            var data = {
                method: 'updateTag',
                id:id,
                name: name,
                constitutions: constitutions
            }
            $.ajax({
                type: 'post',
                data: data,
                url: 'Tag.aspx',
                cache: false,
                success: function (res) {
                    if (res == 'exist')
                        alert('已存在该标签！');
                    else {
                        alert('更新成功');
                        location.reload();
                    }
                },
                error: function (res) {
                    alert(2);
                }
            })
        }
    })

    //全选
    $('.thead1 .iCheck-helper').click(function () {
        if ($('.thead1 div').attr('class') == 'icheckbox_minimal hover checked') {
            //var l = $('.ques-select').length;
            $('.ques-select .icheckbox_minimal').attr('class', 'icheckbox_minimal checked');
        }
        else {
            $('.ques-select .icheckbox_minimal').attr('class', 'icheckbox_minimal');
        }
    })

    $('.btn-batchDelete').click(function () {
        var l = $('tbody div[class="icheckbox_minimal checked"]').length;
        if (l < 1) {
            alert('请先选择！');
            return;
        }
        var ids = new Array();
        for (var i = 0; i < l; i++) {
            var id = $('tbody div[class="icheckbox_minimal checked"]:eq(' + i + ')').parent().children('input').val();
            ids[i] = id;
        }
        
        var r = confirm("确认删除吗？")
        if (r == true) {

                var data = {
                    method: 'batchDelete',
                    ids: ids
                }
                $.ajax({
                    type: 'post',
                    data: data,
                    url: 'Tag.aspx',
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

function clearCss() {
    $('.tag-name input').css('border','1px solid rgb(153,153,153)');
}


















