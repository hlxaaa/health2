$(document).ready(function () {
    //每隔10秒查询新订单
        window.setInterval(remind, 10000);
})

function remind() {
    //alert(isAdmin);
    if (isAdmin == 'False') {
        //if (restId!=null&&restId != 0) {
        var data = {
            method: 'getNewOrder',
            //restId: restId
        }
        $.ajax({
            type: 'post',
            data: data,
            url: 'Order.aspx',
            cache: false,
            success: function (data) {
                if (parseInt(data) > 0) {
                    pushTips(data)
                    var h = '<div class="remind">'
                            + '您有新的订单'
                            + '</div>'
                    $('.remind').remove();
                    $('body').append(h);
                    var t = setTimeout("$('.remind').addClass('add-animation')", 5000)

                }
            },
            error: function (err) {
                alert('cuole');
            }
        })
        //}
    }
}