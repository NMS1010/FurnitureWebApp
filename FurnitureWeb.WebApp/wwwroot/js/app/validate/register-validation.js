function validateForm(e, context) {
    let noError = true;
    e.preventDefault()
    let passMatch = $('#confirmPasswordNotMatch')
    let password = $('#password')
    let confirmPassword = $('#confirmPassword')
    let newPassword = $('#newPassword')
    let userId = $('#userId');
    if (userId.length === 0) {
        if (password.val() !== confirmPassword.val()) {
            passMatch.html('Mật khẩu không khớp').css('color', 'red');
            noError = false;
        } else {
            passMatch.html('')
        }
    } else if (newPassword.length !== 0) {
        if (confirmPassword != null && newPassword.val() !== confirmPassword.val()) {
            passMatch.html('Mật khẩu không khớp').css('color', 'red');
            noError = false;
        } else {
            passMatch.html('')
        }
    }
    let url = "";
    if (userId.length === 0) {
        url += `/users/check-add`
    } else {
        url += `/users/check-edit`
    }
    $.ajax({
        url: url,
        method: "POST",
        data: {
            'username': $('#username').val(),
            'email': $('#email').val(),
            'phone': $('#phoneNumber').val(),
            'password': password.val(),
            'userId': userId.val() === '' ? 0 : userId.val(),
            'hasChangePass': newPassword.val() !== '' || confirmPassword.val() !== ''
        },
        async: false,
        success: function (data) {
            console.log(data)
            let str = data.errors
            if (str.includes('username')) {
                $('#userValidateMessage').html('Tên tài khoản đã tồn tại').css('color', 'red')
                noError = false;
            } else {
                $('#userValidateMessage').html('')
            }

            if (str.includes('email')) {
                $('#emailValidateMessage').html('Email đã tồn tại').css('color', 'red')
                noError = false;
            } else {
                $('#emailValidateMessage').html('')
            }

            if (str.includes('phone')) {
                $('#phoneValidateMessage').html('Số điện thoại đã tồn tại').css('color', 'red')
                noError = false;
            } else {
                $('#phoneValidateMessage').html('')
            }

            if (str.includes('password')) {
                $('#passwordValidateMessage').html('Mật khẩu sai').css('color', 'red')
                noError = false;
            } else {
                $('#passwordValidateMessage').html('')
            }
        },
        error: function (error) {
            noError = false;
        }
    })
    if (noError) {
        $('#form-add').unbind('submit').submit();
    }
}