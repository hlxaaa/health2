var isAdd = false;
//var isHasResult = true;
$(document).ready(function () {
    $('#inputSearch').keydown(function (e) {
        if (e.keyCode == 13)
            $('.btnSearch').click();
    })

    $('.batchDelete').click(function () {
        var l = $('tr div[class="icheckbox_minimal checked"]').length;
        if (l < 1) {
            alert('请先选择');
            return;
        }
        var r = confirm('确定删除这些吗?');
        if (r == true) {
            var ids = new Array();
            $('tr div[class="icheckbox_minimal checked"]').each(function () {
               ids.push($(this).parent().parent().children('#editDelete').children('input').val());
            })
            var data = {
                method: 'batchDelete',
                ids: ids
            }
            $.ajax({
                type: 'post',
                data: data,
                url: 'Restaurant.aspx',
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

    $('body').delegate('.btn-editRest', 'click', function () {
        var id = $(this).prev().val();
        location.href = "RestaurantContent.aspx?id="+id;
    })
    $('.btn-add').click(function () {
        location.href = "RestaurantContent.aspx";
    })

    $('body').delegate('.btn-delRest', 'click', function () {
        var r = confirm('确定删除吗？');
        if (r == true) {
            var id = $(this).prev().prev().val();
            var page = $('.pagination li[class="active"] a').text();
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
                    changePage(page);
                },
                error: function (res) {

                }
            })
        }
    })

    //getTable(jsonStr);
    changePage(1);

    $('input').iCheck({
        checkboxClass: 'icheckbox_minimal',
        radioClass: 'iradio_minimal',
        increaseArea: '20%' // optional
    });
    $('a').css('text-decoration', 'none');
    allselectToggle();

    $('.rest-category-right span').click(function () {
        //if ($('.rest-category-right span').hasClass('label-select'))
            
        //alert(1);
            changePage(1);
    })
})

function getTable(data) {
    var json = JSON.parse(data).Table1;
    //isHasResult = true;
    if (json == null) {
        //alert('没有结果');
        //isHasResult = false;
        $('tbody tr').remove();
        $('#divMain3 nav').remove();
        return;
    }
    var h = '<table class=\"table table-striped table-hover\">'
        + '<thead>'
        + '<tr>'
        + '<th width="1%"><input type="checkbox"></th>'
        //+ '<th width="1%">id</th>'
        + '<th width="13%">名称</th>'
        + '<th width="13%">地址</th>'
        //+ '<th width="5%">菜品分类</th>'
        + '<th width="5%">联系电话</th>'
        + '<th width="4%">分类</th>'
        + '<th width="4%">月销售量</th>'
        + '<th width="4%">人均消费</th>'
         + '<th width="5%">营业时间</th>'
          + '<th width="13%">优惠活动</th>'
        + '<th width="12.5%">操作</th>'
        + '</tr>'
        + '</thead>'
        + '<tbody>';
    for (var i = 0; i < json.length; i++) {
        var name = omit(json[i].name, 20);
        var address=omit(json[i].address,20)
        h += '<tr  style="background-color:white;">';
        h += "<td><input type=\"checkbox\"></td>";
        h += "<td>" + name + "</td>";
        h += "<td>" + address + "</td>";
        h += "<td>" + json[i].phone + "</td>";
        h += "<td>" + json[i].category + "</td>";
        h += "<td>" + json[i].sales + "</td>";
        h += "<td>" + json[i].consumption + "元" + "</td>";
        h += "<td>" + json[i].businesshours + "</td>";
        var discount = omit(json[i].discount, 20);
        discount.replace(/\|/g, ',')
        h += "<td>" + discount + "</td>";
        h += '<td id="editDelete">'
            +'<input type="hidden" value="'+json[i].id+'" />'
            + '<a id="aRightBorder" class="btn-editRest"><img class="btn-edit" src="/Images/recipe/icon-edit.svg" /><font>编辑</font></a> '
            + '<a class="btn-delRest"><img class="btn-edit" src="/Images/recipe/icon-delete.svg" /><font>删除</font></a> '
        + '</td>';
        h += "</tr>";
    }
    h += '</tbody>';


    h += '</table>';
    $('#divMain3 table').remove();
    //$('#divMain3 nav').remove();
    $('#divMain3').append(h);

    var pages = JSON.parse(data).pages;
    var thePage = JSON.parse(data).thePage;
    //h += '<nav style="text-align:center;display:block">'
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
    //$('#divMain3 table').remove();
    var h = getPageHtml(pages, thePage);

    $('#divMain3 nav').remove();
    $('#divMain3').append(h);


}

function changePage(page) {
    var search = $('#inputSearch').val();
    var cate =  $('.rest-category-right span[class="label label-select"]').text();
    
    var data = {
        method: 'search',
        thePage: page,
        search: search,
        cate:cate
    }
    $.ajax({
        type: 'post',
        data: data,
        //dataType: 'html',
        url: 'Restaurant.aspx',
        cache: false,
        success: function (data) {
            getTable(data);
            $('input').iCheck({
                checkboxClass: 'icheckbox_minimal',
                radioClass: 'iradio_minimal',
                increaseArea: '20%' // optional
            });
            $('a').css('text-decoration', 'none');
            allselectToggle();
            //if(!isHasResult)
            //    alert('没有结果')
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

function clickFood(node) {
    
    if (node.className == 'label label-default') {
        $('.rest-category-right span').attr('class', 'label label-default');
        node.className = 'label label-select'
    }
    else {
        $('.rest-category-right span').attr('class', 'label label-default');
        node.className = 'label label-default'
    }
}











function tipsClear() {
    $('#divName').css('border-color', '#515151');
    $('#suggestId').css('border-color', '#515151');
    $('#divPhone').css('border-color', '#515151');
}

function updateRest(id) {
    tipsClear();
    var id = $('#idModal').val();
    var name = $('#divName').val();
    if (name == '') {
        $('#divName').css('border-color', 'red');
        return;
    }
    var address = $('#suggestId').val();
    if (address == '')
        address = $('#divAddress').html();
    var phone = $('#divPhone').val();
    if (phone == '') {
        $('#divPhone').css('border-color', 'red');
        return;
    }
    var category = $('#selectType').val();
    var coordinate = $('#divCoordinate').val();
    var sales = $('#divSales').val();
    var consumption = $('#divConsumption').val();
    var discounts = document.getElementById("tbodyDiscount").getElementsByTagName("p");
    //alert(discounts);
    var discount = new Array();
    for (var i = 0; i < discounts.length; i++) {
        //var a = discounts.children[i];
        discount[i] = discounts[i].innerHTML;
    }

    var data = {
        method:'updateRest',
        name: name,
        address: address,
        phone: phone,
        category: category,
        coordinate: coordinate,
        discounts: discount,
        id: id,
        sales: sales,
        consumption: consumption
    }

    $.ajax({
        type: "post",
        data: data,
        url: "Restaurant.aspx",
        //url: "Restaurant.aspx?method=updateRest&name=" + name + "&address=" + address + "&phone=" + phone + "&category=" + category + "&coordinate=" + coordinate + "&discounts=" + discount + "&id=" + id+"&sales="+sales+"&consumption="+consumption,
        cache: false,
        success: function (data) {
            if (data == "exist")
                alert("已存在该餐厅")
            else
                location.reload();
        },
        error: function (err) {
            alert("出问题了");
        }
    });
}
function modalUpdate(id) {
    tipsClear();
    isAdd = false;
    document.getElementById("btnAdd").style.display = 'none';
    document.getElementById("btnUpdate").style.display = '';
    var a = document.getElementById("info" + id);
    $('#idModal').val(id);
    $('#divName').val(document.getElementById("pName" + id).innerText);
    $('#divAddress').html(a.children["address"].innerText);
    //$('#suggestId').val(a.children["address"].innerText);
    $('#divPhone').val(a.children["phone"].innerText);
    $('#selectType').val(a.children["category"].innerText);
    $('#divCoordinate').val(a.children["coordinate"].innerText);
    $('#divSales').val(a.children["sales"].innerText);
    $('#divConsumption').val(a.children["consumption"].innerText);
    var p = a.getElementsByTagName("p");
    $('#tbodyDiscount').children().remove();
    for (var i = 0; i < p.length; i++) {
        var j = p[i].cloneNode(true);
        $('#tbodyDiscount').append("<span><p>" + p[i].innerText + "</p><a style='margin-right:0px' class='btn btn-info' onclick='deleteDiscount(this)'>删除</a></span>");
        //alert(p[i].innerHTML + "<a style='margin-right:0px' class='btn btn-info' onclick='deleteDiscount(this)'>删除</a>");
    }

    $('#modalAdd').modal();
    //document.getElementById('#info' + id)
}
function deleteRest(id) {
    var r = confirm("确认删除吗？")
    if (r == true) {
        var data = {
            id: id,
            method: 'deleteRest'
        }
        $.ajax({
            type: "post",
            data:data,
            url: "Restaurant.aspx",
            cache: false,
            success: function (data) {
                if (data == "exist")
                    alert("已存在该餐厅")
                else
                    location.reload();
            },
            error: function (err) {
                alert("出问题了");
            }
        });
    }
}


function addRest() {
    tipsClear();
    var name = $('#divName').val();
    if (name == '') {
        $('#divName').css('border-color', 'red');
        return;
    }
    var address = $('#suggestId').val();
    if (address == '') {
        $('#suggestId').css('border-color', 'red');
        return;
    }
    var phone = $('#divPhone').val();
    if (phone == '') {
        $('#divPhone').css('border-color', 'red');
        return;
    }
    var category = $('#selectType').val();
    var coordinate = $('#divCoordinate').val();
    var sales = $('#divSales').val();
    var consumption = $('#divConsumption').val();
    var discounts = document.getElementById("tbodyDiscount").getElementsByTagName("p");
    //alert(discounts);
    var discount = new Array();
    for (var i = 0; i < discounts.length; i++) {
        //var a = discounts.children[i];
        discount[i] = discounts[i].innerHTML;
    }

    var data = {
        method:'addRest',
        name: name,
        address: address,
        phone: phone,
        category: category,
        coordinate: coordinate,
        discounts: discount,
        sales: sales,
        consumption: consumption
    }

    $.ajax({
        type: "post",
        data: data,
        url: "Restaurant.aspx",
        //url: "Restaurant.aspx?method=addRest&name=" + name + "&address=" + address + "&phone=" + phone + "&category=" + category + "&coordinate=" + coordinate +"&discounts=" + discount+"&sales="+sales+"&consumption="+consumption,
        cache: false,
        success: function (data) {
            alert("ok");
            location.reload();
        },
        error: function (err) {
            alert("出问题了");
        }
    });

}
function deleteDiscount(node) {
    var a = node.parentNode;
    a.parentNode.removeChild(a);
}
function addDiscount() {
    var a = $('#inputDiscount').val();
    $('#tbodyDiscount').append("<span><p>" + a + "</p><a style='margin-right:0px' class='btn btn-info' onclick='deleteDiscount(this)'>删除</a></span>");
}
function funChangeImg(i, j) {
    var a = "img" + i + "-" + j;
    document.getElementById(a).style.zIndex -= 1;
}
function modalAdd() {
    tipsClear();
    isAdd = true;
    $('#divName').val("");
    $('#divAddress').html("");
    $('#divPhone').val("");
    $('#divCategory').val("");
    $('#divCoordinate').val("");
    $('#divSales').val("");
    $('#divConsumption').val("");
    $('#tbodyDiscount').children().remove();
    $('#modalAdd').modal();
    document.getElementById("btnAdd").style.display = '';
    document.getElementById("btnUpdate").style.display = "none";
}
//function getImgUrl(node)
//{
//    for (var i = 0; i < node.files.length; i++)
//    {
//        var file = node.files[i];
//        $('#divImg').append("<p id=\"p"+i+"\">"+node.files[i].name+"<button onclick=\"deleteImg(this)\">删除</button></p>");
//    }
//}
function deleteImg(node, i, id) {
    var parent = node.parentElement.parentElement;
    var deleteNode = node.parentElement;
    var l = parent.children.length;
    for (var j = 0; j < l; j++) {
        if (parent.children[j] == node.parentElement) {
            //alert(parent.children[j].innerHTML);
            //alert(node.parentElement.innerHTML);
            i = j;
            break;
        }
    }

    //var a = $('#modalImgs span:eq(' + i + ') img')[0].src;
    //alert(a);
    deleteNode.parentElement.removeChild(deleteNode);
    var data = {
        id: id,
        index: i,
        method:'deleteImg'
    }
    $.ajax({
        type: 'post',
        data:data,
        url: 'Restaurant.aspx',
        cache: false,
        success: function (data) {
            alert('success');
        },
        error: function (err) {
            alert(2);
        }
    })
}
function modalImg(id) {
    $('#imgModal').modal();
    $('#modalImgs').children().remove();
    $('#inputId').val(id);
    var imgs = document.getElementById('divImg' + id).getElementsByTagName('img');
    $('#modalImgs').children().remove();
    for (var i = 0; i < imgs.length; i++) {
        var imgSrc = imgs[i].src;

        var img = "<img src=\'" + imgSrc + "\' style=\'width:70px\'/>"
        $('#modalImgs').append("<span>" + img + "<button onclick=\'deleteImg(this," + i + "," + id + ")\'>删除</button></span>");

    }
}

//function uploadImg() {
//    var fileObj = document.getElementById("FileUpload1").files[0];
//    var formData = new FormData();
//    formData.append("file", fileObj);
//    formData.append('method', 'upload');
//    $.ajax({
//        type: "post",
//        data: formData,
//        // 告诉jQuery不要去处理发送的数据
//        processData: false,
//        // 告诉jQuery不要去设置Content-Type请求头
//        contentType: false,
//        url: "Restaurant.aspx",
//        cache: false,
//        success: function (data) {

//            alert("ok");

//            //location.reload();
//        },
//        error: function (err) {
//            if (data == "noFile")
//                alert(1);
//            alert("出问题了");
//        }
//    });
//}
function reload() {
    location.reload();
}
function openMap() {
    $('#Div1').modal();
}


function getPoint(adds) {
    // 创建地址解析器实例
    var myGeo = new BMap.Geocoder();
    // 将地址解析结果显示在地图上,并调整地图视野
    var a;
    myGeo.getPoint(adds, function (point) {
        a = JSON.stringify(point);
        // alert(a);
        var obj = JSON.parse(a);


        $('#divCoordinate').val(obj.lng + ',' + obj.lat);
    }, "北京市");

    // var a = $('#shopcoord').val();
    // var obj = JSON.parse(a);

}