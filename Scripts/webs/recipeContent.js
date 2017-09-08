var isClicked = false;
$(document).ready(function () {
    //alert($('#oImg').val());

    $('body').delegate('.img-show', 'click', function () {
        $('#imgCut').modal();
        var src = $(this).attr('src').replace(/\?\w+\.\w+/, '')
        //alert(src);
        var img = new Image();
        img.src = src //+ '?' + Math.random();
        var width = img.width
        var height = img.height;

        $('.modal-body-cut img').remove();
        $('.jcrop-holder').remove();
        $('.modal-body-cut').prepend('<img src="" id="target" style="width:700px" />');

        $('.jcrop-keymgr').remove();
        imgCut();
        //$('.jcrop-tracker').css('width','700px')
        $('.jcrop-holder img').attr('src', src)
        //$('.jcrop-holder img').css('height', 700 * height / width)
        //$('.jcrop-holder img').css('width', '700px')
        $('#target').attr('src', src)


    })

    $('.btn-cutImg').click(function () {
        //var a = '321321?111111111111111';
        //a = a.replace(/\?\w+/,'')
        //alert(a);
        //return
        var x1 = $('#x1').val();
        var y1 = $('#y1').val();
        var w = $('#w').val();
        var h = $('#h').val();
        var width = 700;
        var src = $('#target').attr('src')
        var img = new Image();
        img.src = src;
        var realWidth = img.width
        var flag = 1//realWidth > width ? realWidth / width : 1;
        //alert(flag);
        var imgName = src.substring(src.lastIndexOf('/') + 1);
        var data = {
            x1: parseInt(x1 * flag),
            y1: parseInt(y1 * flag),
            w: parseInt(w * flag),
            h: parseInt(h * flag),
            //width: width,
            imgName: imgName,
            method: 'cutImg',
            id: id
        }
        var oImg = $('#oImg').val();
        $.ajax({
            type: 'post',
            data: data,
            url: 'RecipeContent.aspx',
            cache: false,
            success: function (res) {
                $('#imgCut').modal('hide')
                //alert(res);
                $('.div-imgs .img-show').each(function () {
                    var src = $(this).attr('src') //+ '?' + Math.random(); 
                    //alert(src);
                    if (src.substring(src.lastIndexOf('/') + 1) == res.substring(res.lastIndexOf('/') + 2)) {
                        var tempSrc = '/img//recipe//temp//' + res.substring(res.lastIndexOf('/') + 1) //+ '?' + Math.random();
                        $(this).attr('src', tempSrc)
                        if (oImg.length != 0)
                            oImg += '|';
                        oImg += '/img//recipe//temp//' + res.substring(res.lastIndexOf('/') + 1);
                    }
                    $('#oImg').val(oImg);
                    //$(this).attr('src', src)
                })
            },
            error: function (res) {

            }
        })
    })

    $('.btn-del').click(function () {


        if (id == '') {
            //alert("新增，删除也没用");
        }
        else {
            var r = confirm('确定删除吗？')
            if (r = true) {
                var data = {
                    method: 'deleteRecipe',
                    id: id
                }
                $.ajax({
                    type: 'post',
                    data: data,
                    url: 'Recipe.aspx',
                    cache: false,
                    success: function (res) {
                        location.href = 'Recipe.aspx';
                    },
                    error: function (res) {

                    }
                })
            }
        }
    })

    $('.btn-back').click(function () {
        window.history.back();
    })

    $('body').delegate('.img-del', 'click', function () {
        $(this).parent().remove();
        var isChecked = $('.div-imgs .thumb div').hasClass('checked');
        if (!isChecked) {
            $('.div-imgs .thumb div:first').addClass('checked')
        }
    })

    //添加菜品
    $('.btn-addfood').click(function () {
        var a = '<tr style="background-color: white;">';
        a += $('tbody tr:last').html();
        a += '</tr>';
        $('tbody tr:last').after(a);
        $('tbody tr:last .foodtype option:first').prop("selected", 'selected');
        $('tbody tr:last .food option:first').prop("selected", 'selected');
        $('tbody tr:last .weight').val('');
    })

    $('body').delegate('.btn-del-food', 'click', function () {
        if ($('tbody tr').length > 1)
            $(this).parent().parent().remove();
    })
    setAvailable();

    $('body').delegate('.tag', 'click', function () {
        if ($(this).attr('class') != 'tag fl label-select')
            $(this).attr('class', 'tag fl label-select');
        else
            $(this).attr('class', 'tag fl label-default');
    })

    $('.btn-tag-selector').click(function () {
        $('#modal1').modal();
        $('.tag').attr('class', 'tag fl label-default');
        var tags = new Array();
        //var a = $('.tags-span:eq(0) font').text();
        //alert(a);
        $('.tags-span font').each(function () {
            tags.push($(this).text());
        })
        for (var i = 0; i < tags.length; i++) {
            $('.tag').each(function () {
                if (tags[i] == $(this).text()) {
                    $(this).attr('class', 'tag fl label-select');

                }
            })
        }
    })

    $('.btn-saveTag').click(function () {
        $('.tags-span').remove();
        $('.tag[class="tag fl label-select"]').each(function () {
            var h = '<span class="fl tags-span"> <div><font class="fl">' + $(this).text() + '</font>'
+ '<button type="button" class="close fl">×</button>       </div>'
+ '</span>'
            $('.btn-tag-selector').before(h);
        })
        $('#modal1').modal('hide')
    })

    $('body').delegate('.tags-span button', 'click', function () {
        $(this).parent().parent().remove();
    })

    $('.div-upImg').click(function () {
        $('#upImg').val('')
        $('#upImg').click();
    })

    //新增或更新
    $('body').delegate('.btn-save', 'click', function () {
        if (!isClicked) {
            $('#title').css('border-color', '#8c83a3');
            $('#price').css('border-color', '#8c83a3');
            $('#sales').css('border-color', '#8c83a3');
            $('tbody tr .weight').each(function () {
                $(this).css('border-color', '#8c83a3');
            })

            var name = $('#title').val();
            if (name.trim() == '') {
                $('#title').css('border-color', 'red');
                return;
            }
            var a = $('.info1-available div[class="iradio_minimal checked"').parent().hasClass('right-radio')
            var available = a == true ? false : true;
            var restId = $('#restName').val();
            var price = $('#price').val();
            if (price == '') {
                $('#price').css('border-color', 'red');
                return;
            }
            var sales = $('#sales').val();
            if (sales == '') {
                $('#sales').css('border-color', 'red');
                return;
            }
            var tags = new Array();
            $('.tags-span font').each(function () {
                tags.push($(this).text())
            })
            if (tags.length == 0) {
                alert('请至少选择一个标签')
                return;
            }
            var typeIds = new Array();
            var foodIds = new Array();
            var weights = new Array();
            $('tbody tr .foodtype').each(function () {
                typeIds.push($(this).val());
            })
            $('tbody tr .food').each(function () {
                foodIds.push($(this).val());
            })
            //$('tbody tr .weight').each(function () {
            //    if ($(this).val().trim() == '') {
            //        $(this).css('border-color', 'red');
            //        return false
            //    }
            //    else
            //        weights.push($(this).val());
            //})
            var l = $('tbody tr .weight').length
            for (var i = 0; i < l; i++) {
                var text = $('tbody tr:eq(' + i + ') .weight').val().trim();
                if (text == '') {
                    $('tbody tr:eq(' + i + ') .weight').css('border-color', 'red');
                    return;
                }
                weights.push(text);
            }

            var imgs = new Array();
            $('.img-show').each(function () {
                imgs.push($(this).attr('src').replace(/\?\w+\.\w+/, ''));
            })
            if (imgs.length == 0) {
                alert('请至少选择一张图片')
                return
            }
            var l = $('.div-imgs .thumb div').length;
            var imgIndex;
            for (var i = 0; i < l; i++) {
                if ($('.div-imgs:eq(' + i + ') .thumb div').hasClass('checked'))
                    imgIndex = i;
            }
            //var oImgs = $('#oImg').val();
            //alert(oImg);
            //return;

            var oImgs = $('#oImg').val().split('|');
            var imgNames = new Array();
            var oImgNames = new Array();
            //var imgTemps = new Array();
            for (var i = 0; i < imgs.length; i++) {
                imgNames[i] = imgs[i].substring(imgs[i].lastIndexOf('/') + 1);
                //imgs[i] = '\\img\\restaurant\\' + imgNames[i];
                imgs[i] = imgNames[i];
                //imgTemps[i] = '\\img\\restaurant\\temp\\'+imgNames[i];
            }
            for (var i = 0; i < oImgs.length; i++) {
                oImgNames[i] = oImgs[i].substring(oImgs[i].lastIndexOf('/') + 1);
            }

            //if (a == true)
            isClicked = true;
            if (id == '') {
                var data = {
                    imgIndex: imgIndex,
                    name: name,
                    available: available,
                    restId: restId,
                    price: price,
                    sales: sales,
                    tags: tags,
                    typeIds: typeIds,
                    foodIds: foodIds,
                    weights: weights,
                    //imgs: imgs,
                    //oImgs: oImgs,
                    imgNames: imgNames,
                    oImgNames: oImgNames,
                    method: 'addRecipe'
                }
                $.ajax({
                    type: 'post',
                    data: data,
                    url: 'RecipeContent.aspx',
                    cache: false,
                    success: function (res) {
                        alert('添加成功');
                        window.location.href = 'Recipe.aspx';
                    },
                    error: function (res) {

                    }
                })

            }
            else {
                var data = {
                    imgIndex: imgIndex,
                    id: id,
                    name: name,
                    available: available,
                    restId: restId,
                    price: price,
                    sales: sales,
                    tags: tags,
                    typeIds: typeIds,
                    foodIds: foodIds,
                    weights: weights,
                    //imgs: imgs,
                    //oImgs: oImgs,
                    imgNames: imgNames,
                    oImgNames: oImgNames,
                    method: 'updateRecipe'
                }
                $.ajax({
                    type: 'post',
                    data: data,
                    url: 'RecipeContent.aspx',
                    cache: false,
                    success: function (res) {
                        alert('更新成功');
                        window.location.href = 'Recipe.aspx';
                    },
                    error: function (res) {

                    }
                })
            }
        }
    })

})

function setAvailable() {
    if (available == 'False') {
        $('.info1-available .iradio_minimal:eq(1)').attr('class', 'iradio_minimal checked')
    } else {
        $('.info1-available .iradio_minimal:eq(0)').attr('class', 'iradio_minimal checked')
    }
}

function filechange(event) {

    var obj = document.getElementById("upImg");
    var length = obj.files.length;
    var isPic = true;
    for (var i = 0; i < obj.files.length; i++) {
        var temp = obj.files[i].name;
        var fileTarr = temp.split('.');
        var filetype = fileTarr[fileTarr.length - 1];
        if (filetype != 'png' && filetype != 'jpg' && filetype != 'jpeg') {
            alert('上传文件必须为图片(后缀名为png,jpg,jpeg)');
            isPic = false;
        } else {
            var size = obj.files[i].size / 1024;
            //alert(size);
            if (parseInt(size) > 2048) {
                alert("图片大小不能超过2MB");
                isPic = false;
            }
        }
        if (!isPic)
            break;
    }



    if (!isPic)
        return;

    $("#formid").ajaxSubmit(function (res) {
        var a = res.split('|');
        var oImg = $('#oImg').val();
        for (var i in a) {
            var h = '<div class="div-imgs fl">'
    + '<img src="/Images/recipe/icon-deleteSome.svg" class="img-del"/>'
    + '<img class="img-show" src="/' + a[i] + '"  />'
    + '<div class="thumb">'
            if ($('.div-imgs').length > 0) {
                h += '<input type="radio" name="name" value=" "/><font>封面图</font>'
            } else {
                h += '<input type="radio" name="name" value=" " checked="checked"  /><font>封面图</font>'
            }
            h += '</div>'
            + '</div>'
            if ($('.div-imgs').length > 0)
                $('.div-imgs:last').after(h);
            else
                $('.div-upImg').before(h);

            if (oImg.length != 0)
                oImg += '|';
            oImg += a[i];

        }
        $('#oImg').val(oImg);
        getInputStyle();
        setAvailable();
    });
}