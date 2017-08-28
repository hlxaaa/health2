$(document).ready(function () {
    $('.img-vcode').click(function () {
        $(this).attr('src', '/Webs/VerificateCode.aspx?'+Math.random())
    })

    $('#loginPanel').keydown(function (e) {
        if (e.keyCode == 13) {
            $('.btn-login').click();
        }
    })
})


function login() {
    var account = $('#account').val().trim();
    if (account == '')
    {
        alert('账号不能为空');
        return;
    }
    var password = $('#password').val().trim();
    if (password == '') {
        alert('密码不能为空');
        return;
    }
    var vCode = $('#vCode').val();
    if (vCode == '') {
        alert('验证码不能为空');
        return;
    }
    var data = {
        method:'login',
        account: account,
        password: password,
        vCode:vCode
    }

    $.ajax({
        type: 'post',
        data: data,
        url: 'Login.aspx',
        cache: false,
        success: function (data) {
            if (data == 'noAccount')
                alert('没有这个账号');
            else if (data == 'passwordError')
                alert('密码错误');
            //else if (data == 'userLogin')
                //    window.location.href = "/WebSeller/Home.aspx";//普通商家登录
            else if(data=='vCodeError')
                alert('验证码错误');
            else 
                window.location.href = "/Webs/Order.aspx";
        },
        error: function (err) {

        }
    })
}
