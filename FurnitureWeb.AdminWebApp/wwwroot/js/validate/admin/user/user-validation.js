var options = [];
let allCheckBox = document.querySelectorAll('.roleCheckBox:checked')
allCheckBox.forEach(checkbox => {
    options.push(checkbox.value)
})
$('.role.dropdown-menu a').on('click', function (event) {
    var $target = $(event.currentTarget),
        val = $target.attr('data-value'),
        $inp = $target.find('input'),
        idx;

    if ((idx = options.indexOf(val)) > -1) {
        options.splice(idx, 1);
        setTimeout(function () { $inp.prop('checked', false) }, 0);
    } else {
        options.push(val);
        setTimeout(function () { $inp.prop('checked', true) }, 0);
    }

    $(event.target).blur();

    console.log(options);
    return false;
});

$(".clear-form").on("click", function (e) {
    e.preventDefault();
    document.getElementById('form-add').reset();
    $(".modal-add-contact input[type='text']").val('');
    $(".modal-add-contact input[type='date']").val('');
    $(".modal-add-contact input[type='number']").val('');
    $(".modal-add-contact input[type='datetime-local']").val('');
})

function validateForm(e) {
    let noError = true;
    e.preventDefault()
    let passMatch = $('#confirmPasswordNotMatch')
    let roleEmpty = $('#roleEmpty')
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
    if (options.length === 0) {
        roleEmpty.html('Vui lòng chọn role').css('color', 'red');
        noError = false;
    } else {
        roleEmpty.html('')
    }
    let url = '';
    if (userId.length === 0) {
        url += `/users/check-add`
    } else {
        url += `/users/check-edit`
    }
    if (noError) {
        $.ajax({
            url: url,
            method: "POST",
            data: {
                'username': $('#username').val(),
                'email': $('#email').val(),
                'phone': $('#phone').val(),
                'password': password.val(),
                'userId': userId.val() === '' ? 0 : userId.val(),
                'isChangePassword': newPassword.val() !== '' || confirmPassword.val() !== ''
            },
            async: false,
            success: function (data) {
                console.log(data.errors)
                //let str = data.toString()
                let arr = data.errors
                console.log(data.errors)
                if (arr.includes('username')) {
                    $('#userValidateMessage').html('Tên tài khoản đã tồn tại').css('color', 'red')
                    noError = false;
                } else {
                    $('#userValidateMessage').html('')
                }

                if (arr.includes('email')) {
                    $('#emailValidateMessage').html('Email đã tồn tại').css('color', 'red')
                    noError = false;
                } else {
                    $('#emailValidateMessage').html('')
                }

                if (arr.includes('phone')) {
                    $('#phoneValidateMessage').html('Số điện thoại đã tồn tại').css('color', 'red')
                    noError = false;
                } else {
                    $('#phoneValidateMessage').html('')
                }

                if (arr.includes('password')) {
                    $('#passwordValidateMessage').html('Mật khẩu sai').css('color', 'red')
                    noError = false;
                } else {
                    $('#passwordValidateMessage').html('')
                }
            },
            error: function (error) {
                console.log(error)
                noError = false;
            }
        })
    }
    if (noError) {
        $('#form-add').unbind('submit').submit();
    }
}