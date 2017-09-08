var maxMonth = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31]
var normalMonth = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30]
var leapMonth = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29]
var normalFeb = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28]
var months = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]


$(document).ready(function () {

    setChart(jsonStr, 'day');

    $('.datetime').click(function () {
        $('.datetime').css('color', 'rgb(137,149,199)')
        $(this).css('color', '#49425b');
        $('.datetime img').css('display', 'none')
        $(this).children('img').css('display', 'block')
    })

    $('.dtDay').click(function () {
        $('.div-month').css('display', 'block')
        $('.div-year').css('display', 'block')
        getEveryDay();
    })

    $('.dtMonth').click(function () {
        $('.div-month').css('display', 'none')
        $('.div-year').css('display', 'block')
        getEveryMonth();
    })

    $('.dtYear').click(function () {
        $('.div-month').css('display', 'none')
        $('.div-year').css('display', 'none')
        getEveryYear();
    })

    $('body').delegate('.select-year', 'change', function () {
        if ($('.div-month').css('display') == 'none')
            getEveryMonth();
        else
            getEveryDay()
    })
    $('body').delegate('.select-month', 'change', function () {
        getEveryDay()
    })

    //返回
    $('#main1-right').click(function () {
        window.history.back();
    })
})

function getEveryYear() {
    var data = {
        cid: cid,
        method: 'getEveryYear'
    }
    $.ajax({
        type: 'post',
        data: data,
        url: 'CustomerSport.aspx',
        cache: false,
        success: function (data) {
            //getTable(data);
            //alert(data);
            setChart(data, 'year');
        },
        error: function (err) {
            alert('cuole');
        }
    })
}

function getEveryMonth() {

    var year = $('.select-year').val();
    var data = {
        cid: cid,
        year: year,
        method: 'getEveryMonth'
    }
    $.ajax({
        type: 'post',
        data: data,
        url: 'CustomerSport.aspx',
        cache: false,
        success: function (data) {
            //getTable(data);
            //alert(data);
            setChart(data, 'month');
        },
        error: function (err) {
            alert('cuole');
        }
    })
}

function getEveryDay() {
    var year = $('.select-year').val();
    var month = $('.select-month').val();
    var data = {
        cid: cid,
        year: year,
        month: month,
        method: 'getEveryDay'
    }
    $.ajax({
        type: 'post',
        data: data,
        url: 'CustomerSport.aspx',
        cache: false,
        success: function (data) {
            //getTable(data);
            setChart(data, 'day');
        },
        error: function (err) {
            alert('cuole');
        }
    })
}

function setChart(data, dtType) {
    //alert(data);
    var json = JSON.parse(data).Table1;
    if (dtType != 'year') {
        var year = JSON.parse(data).year;
        var preYear = year - 1;
        var nextYear = year + 1;
        var h = '<option value="' + preYear + '">' + preYear + '</option>'
    + '<option value="' + year + '" selected="selected">' + year + '</option>'
    + '<option value="' + nextYear + '">' + nextYear + '</option>'

        $('.select-year').children().remove();
        $('.select-year').prepend(h);
    }
    if (dtType == 'day') {
        var month = JSON.parse(data).month;
        $('.select-month').val(month);
    }
    //var day = JSON.parse(data).day;
    //var labels = getX(day);
    var labels = new Array();
    if (dtType == 'day') {
        if (month == 1 | month == 3 | month == 5 | month == 7 | month == 8 | month == 10 | month == 12)
            labels = maxMonth;
        if (month == 4 | month == 6 | month == 9 | month == 11)
            labels = normalMonth;
        if (month == 2) {
            if (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0))
                labels = leapMonth;
            else
                labels = normalFeb;
        }
    } else if (dtType == 'month') {
        labels = months;
    }
    //alert(labels);
    //return;
    if (json == null) {
        alert('没有结果');
        clearFoodSelect();
        clearTagSelect();
        return;
    }

    var l = json.length;
    var value = new Array();
    if (dtType == 'day') {
        for (var i = 1; i < 31 + 1; i++) {
            var v = '0';
            for (var j = 0; j < l; j++) {
                if (json[j].sDate == i) {
                    v = json[j].steps + '步'
                    //alert(v);
                }
            }
            value.push(v);
        }
    }
    else if (dtType == 'month') {
        for (var i = 1; i < 13; i++) {
            var v1 = '0';
            for (var j = 0; j < l; j++) {
                if (json[j].sDate == i) {
                    v1 = json[j].steps
                    //alert(v);
                }
            }
            value.push(v1);
            //alert(value);
        }
    }
    else {
        for (var i = 0; i < l ; i++) {
            labels.push(json[i].sDate)
            value.push(json[i].steps)
        }
        labels.push(parseInt(json[l - 1].sDate) + 1)
        value.push(0)
    }
    //alert(value);
    //alert(labels);

    var data1 = [
                {
                    value: value,
                    color: '#8c83a3',
                    line_width: 3
                }
    ];

    var chart = new iChart.LineBasic2D({
        render: 'canvasDiv',
        data: data1,
        //title: '北京2012年平均温度情况',
        width: 1600,
        height: 450,
        //pointDot: false,
        //bezierCurve: true,
        //scaleShowLabels: true,
        coordinate: { height: '90%', background_color: '#f6f9fa' },
        sub_option: {
            hollow_inside: true,//设置一个点的亮色在外环的效果
            point_size: 16
        },
        labels: labels
    });
    chart.draw();
}

function getX(day) {
    var r = new Array();
    for (var i = 1; i < day + 1; i++) {
        r.push(i);
    }
    return r;
}