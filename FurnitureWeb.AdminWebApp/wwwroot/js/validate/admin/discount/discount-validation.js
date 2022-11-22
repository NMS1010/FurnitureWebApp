$(".clear-form").on("click", function (e) {
    e.preventDefault();
    document.getElementById('form-add').reset();
    $("input[type='text']").val('');
    $("input[type='date']").val('');
    $("input[type='number']").val('');
    $("input[type='datetime-local']").val('');
})
$('#form-add').submit(function (e) {
    let noError = true
    e.preventDefault()
    let discountValueValidate = $('#discountValueValidate')
    let endDateValidate = $('#endDateValidate')

    let endDateValue = $('#endDate')
    let startDateValue = $('#startDate')
    let value = parseFloat($('#discountValue').val())
    if (isNaN(value) || value > 1 || value < 0) {
        discountValueValidate.html('Vui lòng nhập số thực trong khoảng 0.0 -> 1.0').css('color', 'red');
        noError = false
    } else {
        discountValueValidate.html('')
    }

    if ((new Date(startDateValue.val()).getTime() > new Date(endDateValue.val()).getTime())) {
        endDateValidate.html('Ngày kết thúc phải lớn hơn ngày bắt đầu').css('color', 'red');
        noError = false
    } else {
        endDateValidate.html('')
    }
    if (noError) {
        $('#form-add').unbind('submit').submit();
    }
})