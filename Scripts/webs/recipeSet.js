var restaurantId;
var isHasResult = true;
$(document).ready(function () {
    $('#selectRest').val(restaurantId);
    if (jsonStr != '')
        getTable(jsonStr);
    $('.row1-mid select').change(function () {
        restId = $(this).val();
        location.href = 'RecipeSet.aspx?id=' + restId;
    })
    //$('body').delegate('.iCheck-helper', 'click', function () {
    //    alert($(this).parent().prev().prev().val());
    //})
    //setAvailable()
})

function setAvailable() {
    $('.iCheck-helper').on('click', function () {
        var recipeId = $(this).parent().parent().prev().prev().val();
        var available = $(this).prev().val();
        var data = {
            id: recipeId,
            method: 'setAvailable',
            available: available
        }

        $.ajax({
            type: 'post',
            data: data,
            url: 'RecipeSet.aspx',
            cache: false,
            success: function (data) {
                alert('设置成功')
            },
            error: function (err) {
                alert('cuole');
            }
        })
    })
}

function changePage(page) {
    //alert(page);
    var search = $('#inputSearch').val();
    var id = $('.row1-mid select').val();
    var data = {
        id: id,
        method: 'search',
        thePage: page,
        search: search,
    }

    $.ajax({
        type: 'post',
        data: data,
        url: 'RecipeSet.aspx',
        cache: false,
        success: function (data) {
            getTable(data);
            if (!isHasResult) 
                alert('没有结果')
        },
        error: function (err) {
            alert('cuole');
        }
    })
}

function getTable(data) {
    var json = JSON.parse(data).Table1;
    isHasResult = true;
    if (json == null) {
        //alert('没有结果');
        //clearFoodSelect();
        //clearTagSelect();
        isHasResult = false;
        return;
    }
    var l = json.length;
    $('#divMain3 .recipe').remove();
    for (var i = 0; i < l; i++) {
        var h = '<div class="recipe fl"><input type="hidden" class="recipeId" value="' + json[i].id + '"/><font class="fl bold">' + json[i].name + '</font><div class="fr">'
        if (json[i].available == 'True')
            h += '<font>有货</font><input class="hasRecipe" type="radio" value="True" name="recipe' + json[i].id + '" checked="checked" /><font>缺货</font><input class="noRecipe" value="False" type="radio" name="recipe' + json[i].id + '" /></div></div>'
        else
            h += '<font>有货</font><input type="radio" class="hasRecipe" value="True" name="recipe' + json[i].id + '" /><font>缺货</font><input type="radio"  class="noRecipe"  name="recipe' + json[i].id + '" value="False"  checked="checked" /></div></div>'

        $('#divMain3').prepend(h);
    }
    getInputStyle();
    $('nav').remove();

    var pages = JSON.parse(data).pages;
    var thePage = JSON.parse(data).thePage;
    var h = getPageHtml(pages, thePage);
    //alert(h);
    //if (pages <= 5) {
    //    h = '<nav style="text-align:center;display:block">'
    //    + '<ul class="pagination">'
    //    if (thePage == 1)
    //        h += '<li class="disabled"><a href="#">&laquo;</a></li>'
    //    else
    //        h += '<li onclick="getPrePage()"><a href="#">&laquo;</a></li>'
    //    //+ '<li class="active"><a href="#">1</a></li>';

    //    for (var i = 1; i < pages + 1; i++) {
    //        if (i == thePage)
    //            h += '<li class="active"><a >' + i + '</a></li>'
    //        else
    //            h += '<li onclick="getPage(this)"><a >' + i + '</a></li>'
    //    }
    //    if (pages == thePage)
    //        h += '<li class="disabled" ><a >&raquo;</a></li>'
    //    else
    //        h += '<li onclick="getNextPage()" ><a>&raquo;</a></li>'
    //    h += '</ul>'
    //    + '</nav>'
    //} else {
    //    if (thePage <= 2) {
    //        h = '<nav style="text-align:center;display:block"><ul class="pagination">'
    //        if (thePage == 1)
    //            h += '<li class="disabled"><a href="#">&laquo;</a></li>'
    //        else
    //            h += '<li onclick="getPrePage()"><a href="#">&laquo;</a></li>'

    //        for (var i = 1; i < pages + 1; i++) {
    //            if (i == pages) {
    //                h+='<li><a>...</a></li>'
    //            }
    //            if (i <= thePage + 2 || i == pages) {
    //                if (i == thePage)
    //                    h += '<li class="active"><a >' + i + '</a></li>'
    //                else
    //                    h += '<li onclick="getPage(this)"><a >' + i + '</a></li>'
    //            }
    //        }
           
    //        if (pages == thePage) {
               
    //            h += '<li class="disabled" ><a >&raquo;</a></li>'
    //        }
    //        else {
                
    //            h += '<li onclick="getNextPage()" ><a>&raquo;</a></li>'
    //        }
    //        h += '</ul></nav>'
    //    } else {
    //        if (thePage < 4) {
    //            h = '<nav style="text-align:center;display:block"><ul class="pagination">'
    //            if (thePage == 1)
    //                h += '<li class="disabled"><a href="#">&laquo;</a></li>'
    //            else
    //                h += '<li onclick="getPrePage()"><a href="#">&laquo;</a></li>'

    //            for (var i = 1; i < pages + 1; i++) {
    //                if (i == pages && thePage < pages - 2) {
    //                    h += '<li><a>...</a></li>'
    //                }
    //                if (i <= thePage + 2 && i >= thePage - 2 || i == pages) {
    //                    if (i == thePage)
    //                        h += '<li class="active"><a >' + i + '</a></li>'
    //                    else
    //                        h += '<li onclick="getPage(this)"><a >' + i + '</a></li>'
    //                }
    //            }

    //            if (pages == thePage) {

    //                h += '<li class="disabled" ><a >&raquo;</a></li>'
    //            }
    //            else {

    //                h += '<li onclick="getNextPage()" ><a>&raquo;</a></li>'
    //            }
    //            h += '</ul></nav>'
    //        } else {
    //            h = '<nav style="text-align:center;display:block"><ul class="pagination">'
    //            if (thePage == 1)
    //                h += '<li class="disabled"><a href="#">&laquo;</a></li>'
    //            else
    //                h += '<li onclick="getPrePage()"><a href="#">&laquo;</a></li>'

    //            for (var i = 1; i < pages + 1; i++) {
    //                if (i == pages && thePage < pages - 3) {
    //                    h += '<li><a>...</a></li>'
    //                }
    //                if (i <= thePage + 2 && i >= thePage - 2 || i == pages || i == 1) {
    //                    if (i == thePage)
    //                        h += '<li class="active"><a >' + i + '</a></li>'
    //                    else
    //                        h += '<li onclick="getPage(this)"><a >' + i + '</a></li>'
    //                }
    //                if (i == 1 && thePage > 4) {
    //                    h += '<li><a>...</a></li>'
    //                }
    //            }

    //            if (pages == thePage) {

    //                h += '<li class="disabled" ><a >&raquo;</a></li>'
    //            }
    //            else {

    //                h += '<li onclick="getNextPage()" ><a>&raquo;</a></li>'
    //            }
    //            h += '</ul></nav>'
    //        }
    //    }
    //}

    $('#divMain3').after(h);
    setAvailable();
}

function selectRest() {
    var id = $('#selectRest').val();
    window.location.href = "http://localhost:56349/Webs/RecipeSet.aspx?id=" + id;
}

function changeAvailable(id) {
    var available = $('#selectAvailable' + id).val();
    var data = {
        available: available,
        id: id,
        method: 'change'
    }
    $.ajax({
        type: "post",
        data: data,
        url: "RecipeSet.aspx",

        cache: false,
        success: function (data) {
            //alert("ok");
            //location.reload();
            alert('设置成功！');
        },
        error: function (err) {
            alert('出问题了');
        }
    });
}