var isClicked = false;
$(document).ready(function () {
    $('.div-upImg').click(function () {
        $('#upImg').click();
    })

    //第一张图片被删除
    //$('body').delegate('#divImgs', 'change', function () {
    //    alert(1);
    //})

    $('.discount-input').keydown(function (e) {
        if (e.keyCode == 13)
            $('.add-discount').click();
    })

    $('body').delegate('.img-del', 'click', function () {
        $(this).parent().remove();
        var isChecked = $('.div-imgs .thumb div').hasClass('checked');
        if (!isChecked) {
            $('.div-imgs .thumb div:first').addClass('checked')
        }
    })
    //local.search("1");
    //点击 取消返回 
    $('.btn-back').click(function () {
        window.history.back();
    })

    ////百度地图相关
    //$('body').delegate('#suggestId', 'keydown', function () {
    //    local.search($(this).val());
      
    //})

    //点击 删除
    $('.btn-del').click(function () {
        var r = confirm('确认删除吗？');
        if (r == true) {
            var data = {
                method: 'deleteRest',
                id: id
            }
            $.ajax({
                type: 'post',
                data: data,
                url: 'Restaurant.aspx',
                cache: false,
                success: function (res) {
                    alert('删除成功');
                    location.href = 'Restaurant.aspx';
                },
                error: function (res) {

                }
            })
        }
    })

    //打开地图模态框
    $('.openMap').click(function () {
        $('#div1').modal();
    })

    $('#startTime').datetimepicker({
        //format: 'hh:ii',
        //startView: 1,
        //minView: (0, 'hour'),
        //minuteStep: 10,

        //language: 'zh-CN',//显示中文
        format: 'hh:ii',//显示格式
        minView: (0, 'hour'),
        startView: 1,
        initialDate: new Date('2017-08-29 ' + dt1),
        minuteStep: 10,
        //minView: "hour",//设置只显示到月份
        //initialDate: new Date(),//初始化当前日期
        //autoclose: true,//选中自动关闭
        //todayBtn: true//显示今日按钮

        //format: 'yyyy-mm-dd hh:ii:ss',
        //autoclose: true,
        //minView: (0,'hour'),
        //minuteStep: 1
    });

    $('#endTime').datetimepicker({
        format: 'hh:ii',//显示格式
        minView: (0, 'hour'),
        startView: 1,
        initialDate: new Date('2017-08-29 ' + dt2),
        minuteStep: 10,
    });
    
    //var t = (new Date().getTime('08:22'))
    //alert(t);
    $('#startTime').val(dt1)
    $('#endTime').val(dt2)

    //点击 添加优惠
    $('.add-discount').click(function () {
        var text = $(this).prev().val()
        var h = '<span class="discount fl">'
+ '<input type="text" class="fl discount-input discount-input2" value="'+text+'">'
+ '<img src="/Images/base/icon-deleteX.svg" alt="Alternate Text" class="fl img-base discount-del">'
+ '</span>'
        $('.info3 font:eq(0)').after(h);
        $(this).prev().val('')
    })

    $('body').delegate('.discount-del', 'click', function () {
        $(this).parent().remove();
    })

    $('.btn-getMap').click(function () {
        var address = $('#suggestId').val();
        var coord = $('#coord-x').val() +','+ $('#coord-y').val();
        $('#address').text(address);
        $('#coordinate').text(coord);
        $('#div1').modal('hide');
    })

    //添加或更新
    $('.btn-save').click(function () {
        if (!isClicked) {
            $('#title').css('border-color', '#8c83a3');
            $('#sales').css('border-color', '#8c83a3');
            $('#consumption').css('border-color', '#8c83a3');
            $('#startTime').css('border-color', '#8c83a3');
            $('#endTime').css('border-color', '#8c83a3');

            var name = $('#title').val();
            if (name == '') {
                $('#title').css('border-color', 'red');
                return;
            }
            var address = $('#address').text().trim();
            if (address == '') {
                alert('请选择地址');
                return;
            }
            var coordinate = $('#coordinate').text().trim();
            var categoryId = $('#category').val();
            var phone = $('#phone').val();
            if (phone.length != 11) {
                alert('手机号格式不对');
                return;
            }
            var sales = $('#sales').val();
            if (sales == '') {
                $('#sales').css('border-color', 'red');
                return;
            }

            var consumption = $('#consumption').val();
            if (consumption == '') {
                $('#consumption').css('border-color', 'red');
                return;
            }
            var startTime = $('#startTime').val();
            if (startTime == '') {
                $('#startTime').css('border-color', 'red');
                return;
            }
            var endTime = $('#endTime').val();
            if (endTime == '') {
                $('#endTime').css('border-color', 'red');
                return;
            }
            var discounts = new Array();
            var isDisEmpty = false;
            $('.discount-input2').each(function () {
                if ($(this).val() == '') {

                    isDisEmpty = true;
                    return
                }
                discounts.push($(this).val());
            })
            if (isDisEmpty) {
                alert('优惠不能为空')
                return;
            }
            var imgs = new Array();
            $('.img-show').each(function () {
                imgs.push($(this).attr('src').substring(1));
            })
            if (imgs.length == 0) {
                alert('请至少选择一张图片')
                return;
            }
            var l = $('.div-imgs .thumb div').length;
            var imgIndex;
            for (var i = 0; i < l; i++) {
                if ($('.div-imgs:eq(' + i + ') .thumb div').hasClass('checked'))
                    imgIndex = i;
            }
            var oImgs = $('#oImg').val();
            isClicked = true;
            if (id == '') {
                var data = {
                    method: 'addRest',
                    imgIndex: imgIndex,
                    name: name,
                    address: address,
                    coordinate: coordinate,
                    categoryId: categoryId,
                    phone: phone,
                    sales: sales,
                    consumption: consumption,
                    startTime: startTime,
                    endTime: endTime,
                    discounts: discounts,
                    imgs: imgs,
                    oImgs: oImgs
                }
                $.ajax({
                    type: 'post',
                    data: data,
                    url: 'RestaurantContent.aspx',
                    cache: false,
                    success: function (res) {
                        alert('添加成功');
                        window.location.href = 'Restaurant.aspx';
                    },
                    error: function (res) {

                    }
                })
            }
            else {
                var data = {
                    method: 'updateRest',
                    id: id,
                    imgIndex: imgIndex,
                    name: name,
                    address: address,
                    coordinate: coordinate,
                    categoryId: categoryId,
                    phone: phone,
                    sales: sales,
                    consumption: consumption,
                    startTime: startTime,
                    endTime: endTime,
                    discounts: discounts,
                    imgs: imgs,
                    oImgs: oImgs
                }
                $.ajax({
                    type: 'post',
                    data: data,
                    url: 'RestaurantContent.aspx',
                    cache: false,
                    success: function (res) {
                        alert('更新成功');
                        window.location.href = 'Restaurant.aspx';
                        //location.reload();
                    },
                    error: function (res) {

                    }
                })
            }
        }
    })
})



function filechange(event) {
    $("#formid").ajaxSubmit(function (res) {
        var a = res.split('|');
        var oImg = $('#oImg').val();
        for (var i in a) {
            var h = '<div class="div-imgs fl">'
+ '<img src="/Images/recipe/icon-deleteSome.svg" class="img-del" />'
+ '<img class="img-show" src="/'+a[i]+'" alt="img" />'
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

//百度地图js
    // 百度地图API功能
   


function getPoint(adds) {
    // 创建地址解析器实例
    var myGeo = new BMap.Geocoder();
    // 将地址解析结果显示在地图上,并调整地图视野
    var a;
    myGeo.getPoint(adds, function (point) {
        a = JSON.stringify(point);
        // alert(a);
        var obj = JSON.parse(a);

        $('#coord-x').val(obj.lng);
        $('#coord-y').val(obj.lat);
        //$('#shopcoord').val(obj.lng + ',' + obj.lat);
    }, "北京市");

    // var a = $('#shopcoord').val();
    // var obj = JSON.parse(a);

}