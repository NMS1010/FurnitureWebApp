function onApplyDiscount(context){
    let discountCode = document.getElementById("discount").value
    let discountValidateMessage = document.getElementById("discountValidateMessage")
    let discountValue = document.getElementById("discountValue")

    let url = context + '/discount/apply'
    $.ajax({
        url: url,
        method: "GET",
        data: {
            'discountCode' : discountCode
        },
        async: false,
        success: function (data){
            console.log(data)
            let str = data.toString()
            if(str.includes('error')){
                if(str.length <=10) {
                    discountValidateMessage.innerText = "Mã khuyến mãi không hợp lệ"
                    discountValue.value = "0 %"
                }
            }
            else if(str.includes('suspended')){
                discountValidateMessage.innerText = "Mã khuyến mãi không hợp lệ"
                discountValue.value = "0 %"
            }
            else if(str.includes('expired')){
                discountValidateMessage.innerText = "Mã khuyến mãi đã hết hạn sử dụng"
                discountValue.value = "0 %"
            }
            else if(str.includes("out")){
                discountValidateMessage.innerText = "Mã khuyến mãi đã được dùng hết"
                discountValue.value = "0 %"
            }
            else{
                discountValidateMessage.innerText = ""
                let discount = JSON.parse(data)
                discountValue.value = "-" + discount.discountValue.toString() + " %"
                let elem = document.getElementById("totalPrice")
                let prevPrice = parseFloat(elem.value)
                document.getElementById("discountId").value = discount.discountId.toString()
                elem.value = (prevPrice - prevPrice * parseFloat(discount.discountValue)).toFixed(3).toString()
                document.getElementById("discount").disabled = true
                document.getElementById("apply-btn").style.pointerEvents = "none"
                document.getElementById("apply-btn").style.backgroundColor = "black"
                document.getElementById("apply-btn").style.color = "white"
            }
        },
        error: function (error){

        }
    })
}