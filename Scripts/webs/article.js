var isAdd = true;
var isHasResult = true;

$(document).ready(function () {
    getTable(jsonStr);

    allselectToggle();

    //$.ajax({
    //    //要用post方式      
    //    type: "Post",
    //    //方法所在页面和方法名      
    //    url: "Article.aspx/SayHello",
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (data) {
    //        //返回的数据用data.d获取内容      
    //        alert(data.d);
    //    },
    //    error: function (err) {
    //        alert(err);
    //    }
    //});

    //图片拉伸
    $('#modal1').keyup(function () {
        $('#divEditor img').attr("width", "100%");
    })

    $('body').delegate('.batch-delete', 'click', function () {
        var l = $('tbody div[class="icheckbox_minimal checked"]').length;
        if (l < 1) {
            alert('请先选择');
            return;
        }
        var r = confirm('确定删除这些吗?');
        if (r == true) {
            var ids = new Array();
            $('tbody div[class="icheckbox_minimal checked"]').each(function () {
                var id = $(this).parent().siblings('td[id="editDelete"]').children('a').children('input').val();
                ids.push(id);
            })
            var data = {
                method: 'batchDelete',
                ids: ids
            }
            $.ajax({
                type: 'post',
                data: data,
                url: 'Article.aspx',
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

    $('.btn-add').click(function () {
        $('#modal1').modal();
        $('.tag-selector').css('display', 'none');
        $('#divTags .label-select').remove();
        $('#divTitle').val('')
        $('#divEditor').html('');
        isAdd = true;
        $('#divTitle').css('border-color', '#8c83a3');
    })

    $('.selector-body span').click(function () {
        if ($(this).attr('class') == 'label-default')
            $(this).attr('class', 'tag-selected')
        else
            $(this).attr('class', 'label-default')
    })

    $('.btn-tag').click(function () {
        $('.tag-selector').css('display', 'none');
        $('#divTags .label-select').remove();
        $('.selector-body span').each(function () {
            if ($(this).attr('class') == 'tag-selected') {
                //var h = '<span class="label label-select">'
                //        + '<font>' + $(this).text()+'</font>'
                //        + '<button type="button" class="close">×</button>'
                //        + '</span>'
                //$('.font-tag').after(h);
                addTagSpan($(this).text())
            }
        })

    })

    //点击添加标签按钮
    $('.add-selector img').click(function () {
        $('.selector-body span').attr('class', 'label-default');
        $('.tag-selector').css('display', 'block');
        var l = $('#divTags .label-select').length;
        var l2 = $('.selector-body span').length;

        for (var i = 0; i < l; i++) {
            for (var j = 0; j < l2; j++) {
                var a = $('#divTags .label-select:eq(' + i + ') font').text();
                var b = $('.selector-body span:eq(' + j + ')').text();
                //alert( b);
                if (a == b) {
                    $('.selector-body span:eq(' + j + ')').attr('class', 'tag-selected');
                    break;
                }
            }
        }
    })

    $('.selector-head .close').click(function () {
        $('.tag-selector').css('display', 'none');
    })

    $('body').delegate('.label-select .close', 'click', function () {
        $(this).parent().remove();
    })

    //添加或更新
    $('#btn-addArticle').click(function () {
        $('#divTitle').css('border-color', '#8c83a3');
        if ($('#divEditor img').length < 1) {
            alert('请至少添加一张图片');
            return;
        }
        var title = $('#divTitle').val().trim();
        if (title == '') {
            $('#divTitle').css('border-color', 'red');
            return;
        }
        var tags = new Array();
        $('#divTags span font').each(function () {
            tags.push($(this).text());
        })
        if (tags == '') {
            alert('请选择标签')
            return;
        }
        var imgs = $('#divEditor img');
        var name = new Array(imgs.length);
        var img = new Array(imgs.length);
        var imgTemp = new Array(imgs.length);
        var arr = new Array(imgs.length);

        for (var i = 0; i < imgs.length; i++) {
            arr[i] = $('#divEditor img:eq(' + i + ')').attr("src");
            imgTemp[i] = arr[i].substring(2).replace(/\//g, '\\');
            name[i] = arr[i].substring(arr[i].lastIndexOf('/') + 1);
            img[i] = '\\img\\article\\' + name[i];
            $('#divEditor img:eq(' + i + ')').attr("src", "../img/article/" + name[i]);
            $('#divEditor img:eq(' + i + ')').attr("width", "100%");
        }

        var thumbnail = "img/article/" + name[0];
        var content = $('#divEditor').html();
        content = encode(content);
        if (isAdd) {
            var data = {
                method: 'addArticle',
                title: title,
                content: content,
                tags: tags,
                img: img,
                imgTemp: imgTemp,
                thumbnail: thumbnail
            }

            $.ajax({
                type: "post",
                data: data,
                url: "Article.aspx",
                cache: false,
                success: function (data) {
                    var page = $('.pagination li[class="active"] a').text();
                    changePage(page);
                    $('#modal1').modal('hide')
                },
                error: function (err) {
                    alert('出问题了');
                }
            });
        }
        else {
            var id = $('#inputId').val();
            var oriImg = $('#originalImg').val();
            var data = {
                id: id,
                method: 'updateArticle',
                title: title,
                content: content,
                tags: tags,
                img: img,
                imgTemp: imgTemp,
                oriImg: oriImg,
                thumbnail: thumbnail
            }

            $.ajax({
                type: "post",
                data: data,
                url: "Article.aspx",
                cache: false,
                success: function (data) {
                    var page = $('.pagination li[class="active"] a').text();
                    changePage(page);
                    $('#modal1').modal('hide')
                },
                error: function (err) {
                    alert('出问题了');
                }
            });
        }
    })

    $('body').delegate('.btn-delete', 'click', function () {


        var r = confirm('确认删除吗？');
        if (r == true) {
            var page = $('.pagination li[class="active"] a').text();
            var id = $(this).children('input').val();
            var data = {
                id: id,
                method: 'deleteArticle'
            }
            $.ajax({
                type: 'post',
                data: data,
                url: 'Article.aspx',
                cache: false,
                success: function (res) {
                    changePage(page);
                },
                error: function (res) {

                }
            })
        }

    })

    //点击编辑
    $('body').delegate('.btn-editArticle', 'click', function () {
        $('#divTitle').css('border-color', '#8c83a3');
        isAdd = false;
        var title = $(this).parent().parent().children('td:eq(1)').html();
        var tags = new Array();
        var tag = $(this).parent().parent().children('td:eq(2)').html();
        tags = tag.split(' ');
        //return;
        var id = $(this).next().children('input').val();
        var data = {
            method: 'edit',
            id: id
        }
        $.ajax({
            type: 'post',
            data: data,
            url: 'Article.aspx',
            cache: false,
            success: function (res) {
                $('#divEditor').html(res);
                $('#modal1').modal();
                $('.tag-selector').css('display', 'none');
                $('#inputId').val(id);
                $('#divTitle').val(title)
                $('#divTags .label-select').remove();
                for (var i = 0; i < tags.length; i++) {
                    addTagSpan(tags[i]);
                }

                //获取原图路径
                var imgs = $('#divEditor img');
                var oriImg = new Array(imgs.length);
                var arr = new Array(imgs.length);
                for (var i = 0; i < imgs.length; i++) {
                    arr[i] = $('#divEditor img:eq(' + i + ')').attr("src");
                    oriImg[i] =arr[i].substring(2).replace(/\//g, '\\');
                }
                $('#originalImg').val(oriImg);

            },
            error: function (res) {

            }
        })
    })

    //提示标题剩余字数
    $('#divTitle').keyup(function () {
        inputTip();
    })
    $('#divTitle').change(function () {
        inputTip();
    })
})

function inputTip() {
    var l = 18 - $('#divTitle').val().length
    if (l < 0)
        l = 0;
    $('.inputTips').text('还剩' + l + '字可以输入');
}

function addTagSpan(name) {
    var h = '<span class="label label-select">'
                        + '<font>' + name + '</font>'
                        + '<button type="button" class="close">×</button>'
                        + '</span>'
    $('.font-tag').after(h);
}

function getTable(data) {
    var json = JSON.parse(data).Table1;
    isHasResult = true;
    if (json == null) {
        //alert('没有结果');
        isHasResult = false;
        $('#divMain3 table').remove();
        $('#divMain3 nav').remove();
        return;
    }
    var h = '<table class="table table-striped table-hover">'
+ '<thead>'
+ '<tr>'
+ '<th width="2%"><input type="checkbox"></th>'
+ '<th width="9%">标题</th>'
+ '<th width="9%">标签</th>'
+ '<th width="10%">发表时间</th>'
+ '<th width="2%">浏览量</th>'
+ '<th width="3%">点赞数</th>'
+ '<th width="15%">主要内容</th>'
+ '<th width="10%">操作</th>'
+ '</tr>'
+ '</thead>'
+ '<tbody>'
    for (var i = 0; i < json.length; i++) {
        h += '<tr style="background-color: white;">'
        + '<td>'
        + '<input type="checkbox"></td>'
        + '<td>' + json[i].title + '</td>'
        + '<td>' + json[i].tags + '</td>'
        + '<td>' + json[i].aTime + '</td>'
        + '<td>' + json[i].cilckCount + '</td>'
        + '<td>' + json[i].loveCount + '</td>'
        + '<td>' + json[i].content + '...</td>'
        + '<td id="editDelete">'
        + '<a id="aRightBorder" class="btn-editArticle">'
        + '<img class="btn-edit" src="/Images/recipe/icon-edit.svg" /><font>编辑</font></a>'
        + '<a class="btn-delete">'
        + '<img class="btn-edit" src="/Images/recipe/icon-delete.svg" /><font>删除</font><input type="hidden" value="' + json[i].id + '"></a>'
        + '</td>'
        + '</tr>'
    }
    h += '</tbody>'
    + '</table>'
    + '</div>'
    $('#divMain3 table').remove();
    $('#divMain3').append(h);


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
    //        h += '<li class="active"><a href="#">' + i + '</a></li>'
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
    //$('#divMain3 table').remove();  
    $('#divMain3 nav').remove();
    $('#divMain3').append(h);
    getInputStyle();
}

function changePage(page) {
    var search = $('#inputSearch').val();
    //var cate = $('.rest-category-right span[class="label label-select"]').text();

    var data = {
        method: 'search',
        thePage: page,
        search: search,
        //cate: cate
    }
    $.ajax({
        type: 'post',
        data: data,
        //dataType: 'html',
        url: 'Article.aspx',
        cache: false,
        success: function (data) {
            getTable(data);

            getInputStyle();
            allselectToggle();
            //$('input').iCheck({
            //    checkboxClass: 'icheckbox_minimal',
            //    radioClass: 'iradio_minimal',
            //    increaseArea: '20%' // optional
            //});
            //$('a').css('text-decoration', 'none');
            //allselectToggle();
            if(!isHasResult)
                alert('没有结果')
        },
        error: function (err) {
            alert('cuole');
        }
    })
}

function allselectToggle() {
    $('thead .iCheck-helper').click(function () {
        if ($(this).parent().attr('class') == 'icheckbox_minimal hover checked') {
            //var l = $('.ques-select').length;
            $('tbody .icheckbox_minimal').attr('class', 'icheckbox_minimal checked');
        }
        else {
            $('tbody .icheckbox_minimal').attr('class', 'icheckbox_minimal');
        }
    })
}



var serverPath;
var div = document.getElementById('divEditor');
var editor = new wangEditor(div);
editor.config.uploadImgUrl = 'Article.ashx';
editor.config.hideLinkImg = true;
editor.create();


function encode(str) {
    str = str.trim();
    str = str.replace(/</g, "*lt;");
    str = str.replace(/>/g, "*gt;");
    str = str.replace(/&/g, "*amp");
    //str = str.replace(/"/g, "*dquo");
    //str = str.replace(/ /g, "*nbsp;");
    //str = str.replace(/　/g, "*emsp;");
    //str = str.replace(/\//g, "*quot");
    return str;
}